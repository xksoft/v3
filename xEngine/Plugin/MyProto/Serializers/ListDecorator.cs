using System.Collections.Generic;
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
using System.Reflection;

#endif

namespace MyProto.Serializers
{
    internal sealed class ListDecorator : ProtoDecoratorBase
    {
        internal static bool CanPack(WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Fixed32:
                case WireType.Fixed64:
                case WireType.SignedVariant:
                case WireType.Variant:
                    return true;
                default:
                    return false;
            }
        }

        private readonly byte _options;

        private const byte OptionsIsList = 1,
            OptionsSuppressIList = 2,
            OptionsWritePacked = 4,
            OptionsReturnList = 8,
            OptionsOverwriteList = 16,
            OptionsSupportNull = 32;

        private readonly Type _declaredType, _concreteType;

        private readonly MethodInfo _add;

        private readonly int _fieldNumber;

        private bool IsList
        {
            get { return (_options & OptionsIsList) != 0; }
        }

        private bool SuppressIList
        {
            get { return (_options & OptionsSuppressIList) != 0; }
        }

        private bool WritePacked
        {
            get { return (_options & OptionsWritePacked) != 0; }
        }

        private bool SupportNull
        {
            get { return (_options & OptionsSupportNull) != 0; }
        }

        private bool ReturnList
        {
            get { return (_options & OptionsReturnList) != 0; }
        }

        private readonly WireType _packedWireType;

        public ListDecorator(TypeModel model, Type declaredType, Type concreteType, IProtoSerializer tail,
            int fieldNumber, bool writePacked, WireType packedWireType, bool returnList, bool overwriteList,
            bool supportNull)
            : base(tail)
        {
            if (returnList) _options |= OptionsReturnList;
            if (overwriteList) _options |= OptionsOverwriteList;
            if (supportNull) _options |= OptionsSupportNull;
            if ((writePacked || packedWireType != WireType.None) && fieldNumber <= 0)
                throw new ArgumentOutOfRangeException("fieldNumber");
            if (!CanPack(packedWireType))
            {
                if (writePacked) throw new InvalidOperationException("Only simple data-types can use packed encoding");
                packedWireType = WireType.None;
            }

            _fieldNumber = fieldNumber;
            if (writePacked) _options |= OptionsWritePacked;
            _packedWireType = packedWireType;
            if (declaredType == null) throw new ArgumentNullException("declaredType");
            if (declaredType.IsArray) throw new ArgumentException("Cannot treat arrays as lists", "declaredType");
            _declaredType = declaredType;
            _concreteType = concreteType;

            // look for a public list.Add(typedObject) method
            bool isList;
            _add = TypeModel.ResolveListAdd(model, declaredType, tail.ExpectedType, out isList);
            if (isList)
            {
                _options |= OptionsIsList;
                var fullName = declaredType.FullName;
                if (fullName != null && fullName.StartsWith("System.Data.Linq.EntitySet`1[["))
                {
                    _options |= OptionsSuppressIList;
                }
            }
            if (_add == null)
                throw new InvalidOperationException("Unable to resolve a suitable Add method for " +
                                                    declaredType.FullName);
        }

        public override Type ExpectedType
        {
            get { return _declaredType; }
        }

        public override bool RequiresOldValue
        {
            get { return AppendToCollection; }
        }

        public override bool ReturnsValue
        {
            get { return ReturnList; }
        }

        private bool AppendToCollection
        {
            get { return (_options & OptionsOverwriteList) == 0; }
        }

#if FEAT_COMPILER
        protected override void EmitRead(CompilerContext ctx, Local valueFrom)
        {
            /* This looks more complex than it is. Look at the non-compiled Read to
             * see what it is trying to do, but note that it needs to cope with a
             * few more scenarios. Note that it picks the **most specific** Add,
             * unlike the runtime version that uses IList when possible. The core
             * is just a "do {list.Add(readValue())} while {thereIsMore}"
             * 
             * The complexity is due to:
             *  - value types vs reference types (boxing etc)
             *  - initialization if we need to pass in a value to the tail
             *  - handling whether or not the tail *returns* the value vs updates the input
             */
            var returnList = ReturnList;
            var castListForAdd = !_add.DeclaringType.IsAssignableFrom(_declaredType);
            using (
                var list = AppendToCollection
                    ? ctx.GetLocalWithValue(ExpectedType, valueFrom)
                    : new Local(ctx, _declaredType))
            using (var origlist = (returnList && AppendToCollection) ? new Local(ctx, ExpectedType) : null)
            {
                if (!AppendToCollection)
                {
                    // always new
                    ctx.LoadNullRef();
                    ctx.StoreValue(list);
                }
                else if (returnList)
                {
                    // need a copy
                    ctx.LoadValue(list);
                    ctx.StoreValue(origlist);
                }
                if (_concreteType != null)
                {
                    ctx.LoadValue(list);
                    var notNull = ctx.DefineLabel();
                    ctx.BranchIfTrue(notNull, true);
                    ctx.EmitCtor(_concreteType);
                    ctx.StoreValue(list);
                    ctx.MarkLabel(notNull);
                }

                EmitReadList(ctx, list, Tail, _add, _packedWireType, castListForAdd);

                if (returnList)
                {
                    if (AppendToCollection)
                    {
                        // remember ^^^^ we had a spare copy of the list on the stack; now we'll compare
                        ctx.LoadValue(origlist);
                        ctx.LoadValue(list); // [orig] [new-value]
                        CodeLabel sameList = ctx.DefineLabel(), allDone = ctx.DefineLabel();
                        ctx.BranchIfEqual(sameList, true);
                        ctx.LoadValue(list);
                        ctx.Branch(allDone, true);
                        ctx.MarkLabel(sameList);
                        ctx.LoadNullRef();
                        ctx.MarkLabel(allDone);
                    }
                    else
                    {
                        ctx.LoadValue(list);
                    }
                }
            }
        }

        internal static void EmitReadList(CompilerContext ctx, Local list, IProtoSerializer tail, MethodInfo add,
            WireType packedWireType, bool castListForAdd)
        {
            using (var fieldNumber = new Local(ctx, ctx.MapType(typeof (int))))
            {
                var readPacked = packedWireType == WireType.None ? new CodeLabel() : ctx.DefineLabel();
                if (packedWireType != WireType.None)
                {
                    ctx.LoadReaderWriter();
                    ctx.LoadValue(typeof (ProtoReader).GetProperty("WireType"));
                    ctx.LoadValue((int) WireType.String);
                    ctx.BranchIfEqual(readPacked, false);
                }
                ctx.LoadReaderWriter();
                ctx.LoadValue(typeof (ProtoReader).GetProperty("FieldNumber"));
                ctx.StoreValue(fieldNumber);

                var @continue = ctx.DefineLabel();
                ctx.MarkLabel(@continue);

                EmitReadAndAddItem(ctx, list, tail, add, castListForAdd);

                ctx.LoadReaderWriter();
                ctx.LoadValue(fieldNumber);
                ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("TryReadFieldHeader"));
                ctx.BranchIfTrue(@continue, false);

                if (packedWireType != WireType.None)
                {
                    var allDone = ctx.DefineLabel();
                    ctx.Branch(allDone, false);
                    ctx.MarkLabel(readPacked);

                    ctx.LoadReaderWriter();
                    ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("StartSubItem"));

                    CodeLabel testForData = ctx.DefineLabel(), noMoreData = ctx.DefineLabel();
                    ctx.MarkLabel(testForData);
                    ctx.LoadValue((int) packedWireType);
                    ctx.LoadReaderWriter();
                    ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("HasSubValue"));
                    ctx.BranchIfFalse(noMoreData, false);

                    EmitReadAndAddItem(ctx, list, tail, add, castListForAdd);
                    ctx.Branch(testForData, false);

                    ctx.MarkLabel(noMoreData);
                    ctx.LoadReaderWriter();
                    ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("EndSubItem"));
                    ctx.MarkLabel(allDone);
                }
            }
        }

        private static void EmitReadAndAddItem(CompilerContext ctx, Local list, IProtoSerializer tail, MethodInfo add,
            bool castListForAdd)
        {
            ctx.LoadValue(list);
            if (castListForAdd) ctx.Cast(add.DeclaringType);
            var itemType = tail.ExpectedType;
            var tailReturnsValue = tail.ReturnsValue;
            if (tail.RequiresOldValue)
            {
                if (itemType.IsValueType || !tailReturnsValue)
                {
                    // going to need a variable
                    using (var item = new Local(ctx, itemType))
                    {
                        if (itemType.IsValueType)
                        {
                            // initialise the struct
                            ctx.LoadAddress(item, itemType);
                            ctx.EmitCtor(itemType);
                        }
                        else
                        {
                            // assign null
                            ctx.LoadNullRef();
                            ctx.StoreValue(item);
                        }
                        tail.EmitRead(ctx, item);
                        if (!tailReturnsValue)
                        {
                            ctx.LoadValue(item);
                        }
                    }
                }
                else
                {
                    // no variable; pass the null on the stack and take the value *off* the stack
                    ctx.LoadNullRef();
                    tail.EmitRead(ctx, null);
                }
            }
            else
            {
                if (tailReturnsValue)
                {
                    // out only (on the stack); just emit it
                    tail.EmitRead(ctx, null);
                }
                else
                {
                    // doesn't take anything in nor return anything! WTF?
                    throw new InvalidOperationException();
                }
            }
            // our "Add" is chosen either to take the correct type, or to take "object";
            // we may need to box the value

            var addParamType = add.GetParameters()[0].ParameterType;
            if (addParamType != itemType)
            {
                if (addParamType == ctx.MapType(typeof (object)))
                {
                    ctx.CastToObject(itemType);
                }
#if !NO_GENERICS
                else if (Helpers.GetUnderlyingType(addParamType) == itemType)
                {
                    // list is nullable
                    var ctor = Helpers.GetConstructor(addParamType, new[] {itemType}, false);
                    ctx.EmitCtor(ctor); // the itemType on the stack is now a Nullable<ItemType>
                }
#endif
                else
                {
                    throw new InvalidOperationException("Conflicting item/add type");
                }
            }
            ctx.EmitCall(add);
            if (add.ReturnType != ctx.MapType(typeof (void)))
            {
                ctx.DiscardValue();
            }
        }
#endif

#if WINRT
        private static readonly TypeInfo ienumeratorType = typeof(IEnumerator).GetTypeInfo(), ienumerableType = typeof (IEnumerable).GetTypeInfo();
#else
        private static readonly Type IenumeratorType = typeof (IEnumerator), IenumerableType = typeof (IEnumerable);
#endif

        private MethodInfo GetEnumeratorInfo(TypeModel model, out MethodInfo moveNext, out MethodInfo current)
        {
#if WINRT
            TypeInfo enumeratorType = null, iteratorType, expectedType = ExpectedType.GetTypeInfo();
#else
            Type enumeratorType = null, iteratorType, expectedType = ExpectedType;
#endif

            // try a custom enumerator
            var getEnumerator = Helpers.GetInstanceMethod(expectedType, "GetEnumerator", null);
            var itemType = Tail.ExpectedType;

            Type getReturnType = null;
            if (getEnumerator != null)
            {
                getReturnType = getEnumerator.ReturnType;
                iteratorType = getReturnType
#if WINRT
                    .GetTypeInfo()
#endif
                    ;
                moveNext = Helpers.GetInstanceMethod(iteratorType, "MoveNext", null);
                var prop = Helpers.GetProperty(iteratorType, "Current", false);
                current = prop == null ? null : Helpers.GetGetMethod(prop, false, false);
                if (moveNext == null && (model.MapType(IenumeratorType).IsAssignableFrom(iteratorType)))
                {
                    moveNext = Helpers.GetInstanceMethod(model.MapType(IenumeratorType), "MoveNext", null);
                }
                // fully typed
                if (moveNext != null && moveNext.ReturnType == model.MapType(typeof (bool))
                    && current != null && current.ReturnType == itemType)
                {
                    return getEnumerator;
                }
                moveNext = current = getEnumerator = null;
            }

#if !NO_GENERICS
            // try IEnumerable<T>
            var tmp = model.MapType(typeof (IEnumerable<>), false);

            if (tmp != null)
            {
                tmp = tmp.MakeGenericType(itemType);

#if WINRT
                enumeratorType = tmp.GetTypeInfo();
#else
                enumeratorType = tmp;
#endif
            }
            ;
            if (enumeratorType != null && enumeratorType.IsAssignableFrom(expectedType))
            {
                getEnumerator = Helpers.GetInstanceMethod(enumeratorType, "GetEnumerator");
                getReturnType = getEnumerator.ReturnType;

#if WINRT
                iteratorType = getReturnType.GetTypeInfo();
#else
                iteratorType = getReturnType;
#endif

                moveNext = Helpers.GetInstanceMethod(model.MapType(IenumeratorType), "MoveNext");
                current = Helpers.GetGetMethod(Helpers.GetProperty(iteratorType, "Current", false), false, false);
                return getEnumerator;
            }
#endif
            // give up and fall-back to non-generic IEnumerable
            enumeratorType = model.MapType(IenumerableType);
            getEnumerator = Helpers.GetInstanceMethod(enumeratorType, "GetEnumerator");
            getReturnType = getEnumerator.ReturnType;
            iteratorType = getReturnType
#if WINRT
                .GetTypeInfo()
#endif
                ;
            moveNext = Helpers.GetInstanceMethod(iteratorType, "MoveNext");
            current = Helpers.GetGetMethod(Helpers.GetProperty(iteratorType, "Current", false), false, false);
            return getEnumerator;
        }

#if FEAT_COMPILER
        protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            using (var list = ctx.GetLocalWithValue(ExpectedType, valueFrom))
            {
                MethodInfo moveNext, current, getEnumerator = GetEnumeratorInfo(ctx.Model, out moveNext, out current);
                Helpers.DebugAssert(moveNext != null);
                Helpers.DebugAssert(current != null);
                Helpers.DebugAssert(getEnumerator != null);
                var enumeratorType = getEnumerator.ReturnType;
                var writePacked = WritePacked;
                using (var iter = new Local(ctx, enumeratorType))
                using (var token = writePacked ? new Local(ctx, ctx.MapType(typeof (SubItemToken))) : null)
                {
                    if (writePacked)
                    {
                        ctx.LoadValue(_fieldNumber);
                        ctx.LoadValue((int) WireType.String);
                        ctx.LoadReaderWriter();
                        ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("WriteFieldHeader"));

                        ctx.LoadValue(list);
                        ctx.LoadReaderWriter();
                        ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("StartSubItem"));
                        ctx.StoreValue(token);

                        ctx.LoadValue(_fieldNumber);
                        ctx.LoadReaderWriter();
                        ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("SetPackedField"));
                    }

                    ctx.LoadAddress(list, ExpectedType);
                    ctx.EmitCall(getEnumerator);
                    ctx.StoreValue(iter);
                    using (ctx.Using(iter))
                    {
                        CodeLabel body = ctx.DefineLabel(),
                            @next = ctx.DefineLabel();


                        ctx.Branch(@next, false);

                        ctx.MarkLabel(body);

                        ctx.LoadAddress(iter, enumeratorType);
                        ctx.EmitCall(current);
                        var itemType = Tail.ExpectedType;
                        if (itemType != ctx.MapType(typeof (object)) &&
                            current.ReturnType == ctx.MapType(typeof (object)))
                        {
                            ctx.CastFromObject(itemType);
                        }
                        Tail.EmitWrite(ctx, null);

                        ctx.MarkLabel(@next);
                        ctx.LoadAddress(iter, enumeratorType);
                        ctx.EmitCall(moveNext);
                        ctx.BranchIfTrue(body, false);
                    }

                    if (writePacked)
                    {
                        ctx.LoadValue(token);
                        ctx.LoadReaderWriter();
                        ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("EndSubItem"));
                    }
                }
            }
        }
#endif

#if !FEAT_IKVM
        public override void Write(object value, ProtoWriter dest)
        {
            SubItemToken token;
            var writePacked = WritePacked;
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
            foreach (var subItem in (IEnumerable) value)
            {
                if (checkForNull && subItem == null)
                {
                    throw new NullReferenceException();
                }
                Tail.Write(subItem, dest);
            }
            if (writePacked)
            {
                ProtoWriter.EndSubItem(token, dest);
            }
        }

        public override object Read(object value, ProtoReader source)
        {
            var field = source.FieldNumber;
            var origValue = value;
            if (value == null) value = Activator.CreateInstance(_concreteType);
            var isList = IsList && !SuppressIList;
            if (_packedWireType != WireType.None && source.WireType == WireType.String)
            {
                var token = ProtoReader.StartSubItem(source);
                if (isList)
                {
                    var list = (IList) value;
                    while (ProtoReader.HasSubValue(_packedWireType, source))
                    {
                        list.Add(Tail.Read(null, source));
                    }
                }
                else
                {
                    var args = new object[1];
                    while (ProtoReader.HasSubValue(_packedWireType, source))
                    {
                        args[0] = Tail.Read(null, source);
                        _add.Invoke(value, args);
                    }
                }
                ProtoReader.EndSubItem(token, source);
            }
            else
            {
                if (isList)
                {
                    var list = (IList) value;
                    do
                    {
                        list.Add(Tail.Read(null, source));
                    } while (source.TryReadFieldHeader(field));
                }
                else
                {
                    var args = new object[1];
                    do
                    {
                        args[0] = Tail.Read(null, source);
                        _add.Invoke(value, args);
                    } while (source.TryReadFieldHeader(field));
                }
            }
            return origValue == value ? null : value;
        }
#endif
    }
}

#endif