﻿using MyProto.Compiler;
using MyProto.Meta;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
#endif

namespace MyProto.Serializers
{
    internal sealed class GuidSerializer : IProtoSerializer
    {
#if FEAT_IKVM
        readonly Type expectedType;
#else
        private static readonly Type expectedType = typeof (Guid);
#endif

        public GuidSerializer(TypeModel model)
        {
#if FEAT_IKVM
            expectedType = model.MapType(typeof(Guid));
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
        public void Write(object value, ProtoWriter dest)
        {
            BclHelpers.WriteGuid((Guid) value, dest);
        }

        public object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value == null); // since replaces
            return BclHelpers.ReadGuid(source);
        }
#endif

#if FEAT_COMPILER
        void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitWrite(ctx.MapType(typeof (BclHelpers)), "WriteGuid", valueFrom);
        }

        void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
        {
            ctx.EmitBasicRead(ctx.MapType(typeof (BclHelpers)), "ReadGuid", ExpectedType);
        }
#endif
    }
}

#endif