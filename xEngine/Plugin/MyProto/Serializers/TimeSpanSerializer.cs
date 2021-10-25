﻿using MyProto.Compiler;
using MyProto.Meta;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else

#endif

namespace MyProto.Serializers
{
    internal sealed class TimeSpanSerializer : IProtoSerializer
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (TimeSpan);
#endif

        public TimeSpanSerializer(TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(TimeSpan));
#endif
        }

        public Type ExpectedType
        {
            get { return expectedType; }
        }

        bool IProtoSerializer.RequiresOldValue
        {
            get { return false; }
        }

        bool IProtoSerializer.ReturnsValue
        {
            get { return true; }
        }

#if !FEAT_IKVM
        public object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value == null); // since replaces
            return BclHelpers.ReadTimeSpan(source);
        }

        public void Write(object value, ProtoWriter dest)
        {
            BclHelpers.WriteTimeSpan((TimeSpan) value, dest);
        }
#endif
#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitWrite(ctx.MapType(typeof (BclHelpers)), "WriteTimeSpan", valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicRead(ctx.MapType(typeof (BclHelpers)), "ReadTimeSpan", ExpectedType);
        }
#endif
    }
}

#endif