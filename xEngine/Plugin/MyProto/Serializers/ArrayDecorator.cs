using System.Collections.Generic;
using System.Reflection;
using MyProto.Compiler;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;
using System.Collections;
using MyProto.Meta;

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else

#endif

namespace MyProto.Serializers
{
    internal sealed class ArrayDecorator : ProtoDecoratorBase
    {
        private const byte
            OptionsWritePacked = 1,
            OptionsOverwriteList = 2,
            OptionsSupportNull = 4;

        private readonly Type _arrayType, _itemType; // this is, for example, typeof(int[])
        private readonly int _fieldNumber;
        private readonly byte _options;
        private readonly WireType _packedWireType;

        public ArrayDecorator(TypeModel model, IProtoSerializer tail, int fieldNumber, bool writePacked,
            WireType packedWireType, Type arrayType, bool overwriteList, bool supportNull)
            : base(tail)
        {
            Helpers.DebugAssert(arrayType != null, "arrayType should be non-null");
            Helpers.DebugAssert(arrayType.IsArray && arrayType.GetArrayRank() == 1,
                "should be single-dimension array; " + arrayType.FullName);
            _itemType = arrayType.GetElementType();
#if NO_GENERICS
            Type underlyingItemType = itemType;
#else
            var underlyingItemType = supportNull ? _itemType : (Helpers.GetUnderlyingType(_itemType) ?? _itemType);
#endif

            Helpers.DebugAssert(underlyingItemType == Tail.ExpectedType, "invalid tail");
            Helpers.DebugAssert(Tail.ExpectedType != model.MapType(typeof (byte)), "Should have used BlobSerializer");
            if ((writePacked || packedWireType != WireType.None) && fieldNumber <= 0)
                throw new ArgumentOutOfRangeException("fieldNumber");
            if (!ListDecorator.CanPack(packedWireType))
            {
                if (writePacked) throw new InvalidOperationException("Only simple data-types can use packed encoding");
                packedWireType = WireType.None;
            }
            _fieldNumber = fieldNumber;
            _packedWireType = packedWireType;
            if (writePacked) _options |= OptionsWritePacked;
            if (overwriteList) _options |= OptionsOverwriteList;
            if (supportNull) _options |= OptionsSupportNull;
            _arrayType = arrayType;
        }

        public override Type ExpectedType
        {
            get { return _arrayType; }
        }

        public override bool RequiresOldValue
        {
            get { return AppendToCollection; }
        }

        public override bool ReturnsValue
        {
            get { return true; }
        }

        private bool AppendToCollection
        {
            get { return (_options & OptionsOverwriteList) == 0; }
        }

        private bool SupportNull
        {
            get { return (_options & OptionsSupportNull) != 0; }
        }

#if FEAT_COMPILER
        protected override void EmitRead(CompilerContext ctx, Local valueFrom)
        {
            Type listType;
#if NO_GENERICS
            listType = typeof(BasicList);
#else
            listType = ctx.MapType(typeof (List<>)).MakeGenericType(_itemType);
#endif
            var expected = ExpectedType;
            using (var oldArr = AppendToCollection ? ctx.GetLocalWithValue(expected, valueFrom) : null)
            using (var newArr = new Local(ctx, expected))
            using (var list = new Local(ctx, listType))
            {
                ctx.EmitCtor(listType);
                ctx.StoreValue(list);
                ListDecorator.EmitReadList(ctx, list, Tail, listType.GetMethod("Add"), _packedWireType, false);

                // leave this "using" here, as it can share the "FieldNumber" local with EmitReadList
                using (var oldLen = AppendToCollection ? new Local(ctx, ctx.MapType(typeof (int))) : null)
                {
                    var copyToArrayInt32Args = new[] {ctx.MapType(typeof (Array)), ctx.MapType(typeof (int))};

                    if (AppendToCollection)
                    {
                        ctx.LoadLength(oldArr, true);
                        ctx.CopyValue();
                        ctx.StoreValue(oldLen);

                        ctx.LoadAddress(list, listType);
                        ctx.LoadValue(listType.GetProperty("Count"));
                        ctx.Add();
                        ctx.CreateArray(_itemType, null); // length is on the stack
                        ctx.StoreValue(newArr);

                        ctx.LoadValue(oldLen);
                        var nothingToCopy = ctx.DefineLabel();
                        ctx.BranchIfFalse(nothingToCopy, true);
                        ctx.LoadValue(oldArr);
                        ctx.LoadValue(newArr);
                        ctx.LoadValue(0); // index in target

                        ctx.EmitCall(expected.GetMethod("CopyTo", copyToArrayInt32Args));
                        ctx.MarkLabel(nothingToCopy);

                        ctx.LoadValue(list);
                        ctx.LoadValue(newArr);
                        ctx.LoadValue(oldLen);
                    }
                    else
                    {
                        ctx.LoadAddress(list, listType);
                        ctx.LoadValue(listType.GetProperty("Count"));
                        ctx.CreateArray(_itemType, null);
                        ctx.StoreValue(newArr);

                        ctx.LoadAddress(list, listType);
                        ctx.LoadValue(newArr);
                        ctx.LoadValue(0);
                    }

                    copyToArrayInt32Args[0] = expected; // // prefer: CopyTo(T[], int)
                    var copyTo = listType.GetMethod("CopyTo", copyToArrayInt32Args);
                    if (copyTo == null)
                    {
                        // fallback: CopyTo(Array, int)
                        copyToArrayInt32Args[1] = ctx.MapType(typeof (Array));
                        copyTo = listType.GetMethod("CopyTo", copyToArrayInt32Args);
                    }
                    ctx.EmitCall(copyTo);
                }
                ctx.LoadValue(newArr);
            }
        }
#endif
#if FEAT_COMPILER
        protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            // int i and T[] arr
            using (var arr = ctx.GetLocalWithValue(_arrayType, valueFrom))
            using (var i = new Local(ctx, ctx.MapType(typeof (int))))
            {
                var writePacked = (_options & OptionsWritePacked) != 0;
                using (var token = writePacked ? new Local(ctx, ctx.MapType(typeof (SubItemToken))) : null)
                {
                    var mappedWriter = ctx.MapType(typeof (ProtoWriter));
                    if (writePacked)
                    {
                        ctx.LoadValue(_fieldNumber);
                        ctx.LoadValue((int) WireType.String);
                        ctx.LoadReaderWriter();
                        ctx.EmitCall(mappedWriter.GetMethod("WriteFieldHeader"));

                        ctx.LoadValue(arr);
                        ctx.LoadReaderWriter();
                        ctx.EmitCall(mappedWriter.GetMethod("StartSubItem"));
                        ctx.StoreValue(token);

                        ctx.LoadValue(_fieldNumber);
                        ctx.LoadReaderWriter();
                        ctx.EmitCall(mappedWriter.GetMethod("SetPackedField"));
                    }
                    EmitWriteArrayLoop(ctx, i, arr);

                    if (writePacked)
                    {
                        ctx.LoadValue(token);
                        ctx.LoadReaderWriter();
                        ctx.EmitCall(mappedWriter.GetMethod("EndSubItem"));
                    }
                }
            }
        }

        private void EmitWriteArrayLoop(CompilerContext ctx, Local i, Local arr)
        {
            // i = 0
            ctx.LoadValue(0);
            ctx.StoreValue(i);

            // range test is last (to minimise branches)
            CodeLabel loopTest = ctx.DefineLabel(), processItem = ctx.DefineLabel();
            ctx.Branch(loopTest, false);
            ctx.MarkLabel(processItem);

            // {...}
            ctx.LoadArrayValue(arr, i);
            if (SupportNull)
            {
                Tail.EmitWrite(ctx, null);
            }
            else
            {
                ctx.WriteNullCheckedTail(_itemType, Tail, null);
            }

            // i++
            ctx.LoadValue(i);
            ctx.LoadValue(1);
            ctx.Add();
            ctx.StoreValue(i);

            // i < arr.Length
            ctx.MarkLabel(loopTest);
            ctx.LoadValue(i);
            ctx.LoadLength(arr, false);
            ctx.BranchIfLess(processItem, false);
        }
#endif
#if !FEAT_IKVM
        public override void Write(object value, ProtoWriter dest)
        {
            var arr = (IList) value;
            var len = arr.Count;
            SubItemToken token;
            var writePacked = (_options & OptionsWritePacked) != 0;
            if (writePacked)
            {
                ProtoWriter.WriteFieldHeader(_fieldNumber, WireType.String, dest);
                token = ProtoWriter.StartSubItem(value, dest);
                ProtoWriter.SetPackedField(_fieldNumber, dest);
            }
            else
            {
                token = new SubItemToken(); // default
            }
            var checkForNull = !SupportNull;
            for (var i = 0; i < len; i++)
            {
                var obj = arr[i];
                if (checkForNull && obj == null)
                {
                    throw new NullReferenceException();
                }
                Tail.Write(obj, dest);
            }
            if (writePacked)
            {
                ProtoWriter.EndSubItem(token, dest);
            }
        }

        public override object Read(object value, ProtoReader source)
        {
            var field = source.FieldNumber;
            var list = new BasicList();
            if (_packedWireType != WireType.None && source.WireType == WireType.String)
            {
                var token = ProtoReader.StartSubItem(source);
                while (ProtoReader.HasSubValue(_packedWireType, source))
                {
                    list.Add(Tail.Read(null, source));
                }
                ProtoReader.EndSubItem(token, source);
            }
            else
            {
                do
                {
                    list.Add(Tail.Read(null, source));
                } while (source.TryReadFieldHeader(field));
            }
            var oldLen = AppendToCollection ? ((value == null ? 0 : ((Array) value).Length)) : 0;
            var result = Array.CreateInstance(_itemType, oldLen + list.Count);
            if (oldLen != 0) ((Array) value).CopyTo(result, 0);
            list.CopyTo(result, oldLen);
            return result;
        }
#endif
    }
}

#endif