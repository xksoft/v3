using MyProto.Compiler;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;
using MyProto.Meta;

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else

#endif

namespace MyProto.Serializers
{
    internal sealed class EnumSerializer : IProtoSerializer
    {
        private readonly Type _enumType;
        private readonly EnumPair[] _map;

        public EnumSerializer(Type enumType, EnumPair[] map)
        {
            if (enumType == null) throw new ArgumentNullException("enumType");
            _enumType = enumType;
            _map = map;
            if (map != null)
            {
                for (var i = 1; i < map.Length; i++)
                    for (var j = 0; j < i; j++)
                    {
                        if (map[i].WireValue == map[j].WireValue && !Equals(map[i].RawValue, map[j].RawValue))
                        {
                            throw new ProtoException("Multiple enums with wire-value " + map[i].WireValue);
                        }
                        if (Equals(map[i].RawValue, map[j].RawValue) && map[i].WireValue != map[j].WireValue)
                        {
                            throw new ProtoException("Multiple enums with deserialized-value " + map[i].RawValue);
                        }
                    }
            }
        }

        public Type ExpectedType
        {
            get { return _enumType; }
        }

        bool IProtoSerializer.RequiresOldValue
        {
            get { return false; }
        }

        bool IProtoSerializer.ReturnsValue
        {
            get { return true; }
        }

        private ProtoTypeCode GetTypeCode()
        {
            var type = Helpers.GetUnderlyingType(_enumType);
            if (type == null) type = _enumType;
            return Helpers.GetTypeCode(type);
        }

        public struct EnumPair
        {
            public readonly object RawValue; // note that this is boxing, but I'll live with it
#if !FEAT_IKVM
            public readonly Enum TypedValue; // note that this is boxing, but I'll live with it
#endif
            public readonly int WireValue;

            public EnumPair(int wireValue, object raw, Type type)
            {
                WireValue = wireValue;
                RawValue = raw;
#if !FEAT_IKVM
                TypedValue = (Enum) Enum.ToObject(type, raw);
#endif
            }
        }

#if !FEAT_IKVM
        private int EnumToWire(object value)
        {
            unchecked
            {
                switch (GetTypeCode())
                {
                    // unbox then convert to int
                    case ProtoTypeCode.Byte:
                        return (byte) value;
                    case ProtoTypeCode.SByte:
                        return (sbyte) value;
                    case ProtoTypeCode.Int16:
                        return (short) value;
                    case ProtoTypeCode.Int32:
                        return (int) value;
                    case ProtoTypeCode.Int64:
                        return (int) (long) value;
                    case ProtoTypeCode.UInt16:
                        return (ushort) value;
                    case ProtoTypeCode.UInt32:
                        return (int) (uint) value;
                    case ProtoTypeCode.UInt64:
                        return (int) (ulong) value;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private object WireToEnum(int value)
        {
            unchecked
            {
                switch (GetTypeCode())
                {
                    // convert from int then box 
                    case ProtoTypeCode.Byte:
                        return Enum.ToObject(_enumType, (byte) value);
                    case ProtoTypeCode.SByte:
                        return Enum.ToObject(_enumType, (sbyte) value);
                    case ProtoTypeCode.Int16:
                        return Enum.ToObject(_enumType, (short) value);
                    case ProtoTypeCode.Int32:
                        return Enum.ToObject(_enumType, value);
                    case ProtoTypeCode.Int64:
                        return Enum.ToObject(_enumType, (long) value);
                    case ProtoTypeCode.UInt16:
                        return Enum.ToObject(_enumType, (ushort) value);
                    case ProtoTypeCode.UInt32:
                        return Enum.ToObject(_enumType, (uint) value);
                    case ProtoTypeCode.UInt64:
                        return Enum.ToObject(_enumType, (ulong) value);
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value == null); // since replaces
            var wireValue = source.ReadInt32();
            if (_map == null)
            {
                return WireToEnum(wireValue);
            }
            for (var i = 0; i < _map.Length; i++)
            {
                if (_map[i].WireValue == wireValue)
                {
                    return _map[i].TypedValue;
                }
            }
            source.ThrowEnumException(ExpectedType, wireValue);
            return null; // to make compiler happy
        }

        public void Write(object value, ProtoWriter dest)
        {
            if (_map == null)
            {
                ProtoWriter.WriteInt32(EnumToWire(value), dest);
            }
            else
            {
                for (var i = 0; i < _map.Length; i++)
                {
                    if (Equals(_map[i].TypedValue, value))
                    {
                        ProtoWriter.WriteInt32(_map[i].WireValue, dest);
                        return;
                    }
                }
                ProtoWriter.ThrowEnumException(dest, value);
            }
        }
#endif
#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            var typeCode = GetTypeCode();
            if (_map == null)
            {
                ctx.LoadValue(valueFrom);
                ctx.ConvertToInt32(typeCode, false);
                ctx.EmitBasicWrite("WriteInt32", null);
            }
            else
            {
                using (var loc = ctx.GetLocalWithValue(ExpectedType, valueFrom))
                {
                    var @continue = ctx.DefineLabel();
                    for (var i = 0; i < _map.Length; i++)
                    {
                        CodeLabel tryNextValue = ctx.DefineLabel(), processThisValue = ctx.DefineLabel();
                        ctx.LoadValue(loc);
                        WriteEnumValue(ctx, typeCode, _map[i].RawValue);
                        ctx.BranchIfEqual(processThisValue, true);
                        ctx.Branch(tryNextValue, true);
                        ctx.MarkLabel(processThisValue);
                        ctx.LoadValue(_map[i].WireValue);
                        ctx.EmitBasicWrite("WriteInt32", null);
                        ctx.Branch(@continue, false);
                        ctx.MarkLabel(tryNextValue);
                    }
                    ctx.LoadReaderWriter();
                    ctx.LoadValue(loc);
                    ctx.CastToObject(ExpectedType);
                    ctx.EmitCall(ctx.MapType(typeof (ProtoWriter)).GetMethod("ThrowEnumException"));
                    ctx.MarkLabel(@continue);
                }
            }
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            var typeCode = GetTypeCode();
            if (_map == null)
            {
                ctx.EmitBasicRead("ReadInt32", ctx.MapType(typeof (int)));
                ctx.ConvertFromInt32(typeCode, false);
            }
            else
            {
                var wireValues = new int[_map.Length];
                var values = new object[_map.Length];
                for (var i = 0; i < _map.Length; i++)
                {
                    wireValues[i] = _map[i].WireValue;
                    values[i] = _map[i].RawValue;
                }
                using (var result = new Local(ctx, ExpectedType))
                using (var wireValue = new Local(ctx, ctx.MapType(typeof (int))))
                {
                    ctx.EmitBasicRead("ReadInt32", ctx.MapType(typeof (int)));
                    ctx.StoreValue(wireValue);
                    var @continue = ctx.DefineLabel();
                    foreach (BasicList.Group group in BasicList.GetContiguousGroups(wireValues, values))
                    {
                        var tryNextGroup = ctx.DefineLabel();
                        var groupItemCount = group.Items.Count;
                        if (groupItemCount == 1)
                        {
                            // discreet group; use an equality test
                            ctx.LoadValue(wireValue);
                            ctx.LoadValue(group.First);
                            var processThisValue = ctx.DefineLabel();
                            ctx.BranchIfEqual(processThisValue, true);
                            ctx.Branch(tryNextGroup, false);
                            WriteEnumValue(ctx, typeCode, processThisValue, @continue, group.Items[0], @result);
                        }
                        else
                        {
                            // implement as a jump-table-based switch
                            ctx.LoadValue(wireValue);
                            ctx.LoadValue(group.First);
                            ctx.Subtract(); // jump-tables are zero-based
                            var jmp = new CodeLabel[groupItemCount];
                            for (var i = 0; i < groupItemCount; i++)
                            {
                                jmp[i] = ctx.DefineLabel();
                            }
                            ctx.Switch(jmp);
                            // write the default...
                            ctx.Branch(tryNextGroup, false);
                            for (var i = 0; i < groupItemCount; i++)
                            {
                                WriteEnumValue(ctx, typeCode, jmp[i], @continue, group.Items[i], @result);
                            }
                        }
                        ctx.MarkLabel(tryNextGroup);
                    }
                    // throw source.CreateEnumException(ExpectedType, wireValue);
                    ctx.LoadReaderWriter();
                    ctx.LoadValue(ExpectedType);
                    ctx.LoadValue(wireValue);
                    ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("ThrowEnumException"));
                    ctx.MarkLabel(@continue);
                    ctx.LoadValue(result);
                }
            }
        }

        private static void WriteEnumValue(CompilerContext ctx, ProtoTypeCode typeCode, object value)
        {
            switch (typeCode)
            {
                case ProtoTypeCode.Byte:
                    ctx.LoadValue((byte) value);
                    break;
                case ProtoTypeCode.SByte:
                    ctx.LoadValue((sbyte) value);
                    break;
                case ProtoTypeCode.Int16:
                    ctx.LoadValue((short) value);
                    break;
                case ProtoTypeCode.Int32:
                    ctx.LoadValue((int) value);
                    break;
                case ProtoTypeCode.Int64:
                    ctx.LoadValue((long) value);
                    break;
                case ProtoTypeCode.UInt16:
                    ctx.LoadValue((ushort) value);
                    break;
                case ProtoTypeCode.UInt32:
                    ctx.LoadValue((int) (uint) value);
                    break;
                case ProtoTypeCode.UInt64:
                    ctx.LoadValue((long) (ulong) value);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static void WriteEnumValue(CompilerContext ctx, ProtoTypeCode typeCode, CodeLabel handler,
            CodeLabel @continue, object value, Local local)
        {
            ctx.MarkLabel(handler);
            WriteEnumValue(ctx, typeCode, value);
            ctx.StoreValue(local);
            ctx.Branch(@continue, false); // "continue"
        }
#endif
    }
}

#endif