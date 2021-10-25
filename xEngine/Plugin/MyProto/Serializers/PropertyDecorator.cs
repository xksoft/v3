using MyProto.Compiler;
using xEngine.Plugin.MyProto.Compiler;
#if !NO_RUNTIME
using System;
using MyProto.Meta;
#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
using IKVM.Reflection;
#else
using System.Reflection;

#endif

namespace MyProto.Serializers
{
    internal sealed class PropertyDecorator : ProtoDecoratorBase
    {
        private readonly Type _forType;
        private readonly PropertyInfo _property;
        private readonly bool _readOptionsWriteValue;
        private readonly MethodInfo _shadowSetter;

        public PropertyDecorator(TypeModel model, Type forType, PropertyInfo property, IProtoSerializer tail)
            : base(tail)
        {
            Helpers.DebugAssert(forType != null);
            Helpers.DebugAssert(property != null);
            _forType = forType;
            _property = property;
            SanityCheck(model, property, tail, out _readOptionsWriteValue, true, true);
            _shadowSetter = GetShadowSetter(model, property);
        }

        public override Type ExpectedType
        {
            get { return _forType; }
        }

        public override bool RequiresOldValue
        {
            get { return true; }
        }

        public override bool ReturnsValue
        {
            get { return false; }
        }

        private static void SanityCheck(TypeModel model, PropertyInfo property, IProtoSerializer tail,
            out bool writeValue, bool nonPublic, bool allowInternal)
        {
            if (property == null) throw new ArgumentNullException("property");

            writeValue = tail.ReturnsValue &&
                         (GetShadowSetter(model, property) != null ||
                          (property.CanWrite && Helpers.GetSetMethod(property, nonPublic, allowInternal) != null));
            if (!property.CanRead || Helpers.GetGetMethod(property, nonPublic, allowInternal) == null)
            {
                throw new InvalidOperationException("Cannot serialize property without a get accessor");
            }
            if (!writeValue && (!tail.RequiresOldValue || Helpers.IsValueType(tail.ExpectedType)))
            {
                // so we can't save the value, and the tail doesn't use it either... not helpful
                // or: can't write the value, so the struct value will be lost
                throw new InvalidOperationException("Cannot apply changes to property " +
                                                    property.DeclaringType.FullName + "." + property.Name);
            }
        }

        private static MethodInfo GetShadowSetter(TypeModel model, PropertyInfo property)
        {
#if WINRT            
            MethodInfo method = Helpers.GetInstanceMethod(property.DeclaringType.GetTypeInfo(), "Set" + property.Name, new Type[] { property.PropertyType });
#else

#if FEAT_IKVM
            Type reflectedType = property.DeclaringType;
#else
            var reflectedType = property.ReflectedType;
#endif
            var method = Helpers.GetInstanceMethod(reflectedType, "Set" + property.Name,
                new[] {property.PropertyType});
#endif
            if (method == null || !method.IsPublic || method.ReturnType != model.MapType(typeof (void))) return null;
            return method;
        }

        internal static bool CanWrite(TypeModel model, MemberInfo member)
        {
            if (member == null) throw new ArgumentNullException("member");

            var prop = member as PropertyInfo;
            if (prop != null) return prop.CanWrite || GetShadowSetter(model, prop) != null;

            return member is FieldInfo; // fields are always writeable; anything else: JUST SAY NO!
        }

#if !FEAT_IKVM
        public override void Write(object value, ProtoWriter dest)
        {
            Helpers.DebugAssert(value != null);
            value = _property.GetValue(value, null);
            if (value != null) Tail.Write(value, dest);
        }

        public override object Read(object value, ProtoReader source)
        {
            Helpers.DebugAssert(value != null);

            var oldVal = Tail.RequiresOldValue ? _property.GetValue(value, null) : null;
            var newVal = Tail.Read(oldVal, source);
            if (_readOptionsWriteValue && newVal != null) // if the tail returns a null, intepret that as *no assign*
            {
                if (_shadowSetter == null)
                {
                    _property.SetValue(value, newVal, null);
                }
                else
                {
                    _shadowSetter.Invoke(value, new[] {newVal});
                }
            }
            return null;
        }
#endif
#if FEAT_COMPILER
        protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
        {
            ctx.LoadAddress(valueFrom, ExpectedType);
            ctx.LoadValue(_property);
            ctx.WriteNullCheckedTail(_property.PropertyType, Tail, null);
        }

        protected override void EmitRead(CompilerContext ctx, Local valueFrom)
        {
            bool writeValue;
            SanityCheck(ctx.Model, _property, Tail, out writeValue, ctx.NonPublic, ctx.AllowInternal(_property));
            if (ExpectedType.IsValueType && valueFrom == null)
            {
                throw new InvalidOperationException(
                    "Attempt to mutate struct on the head of the stack; changes would be lost");
            }

            ctx.LoadAddress(valueFrom, ExpectedType); // stack is: old-addr
            if (writeValue && Tail.RequiresOldValue)
            {
                // need to read and write
                ctx.CopyValue();
            }
            // stack is: [old-addr]|old-addr
            if (Tail.RequiresOldValue)
            {
                ctx.LoadValue(_property); // stack is: [old-addr]|old-value
            }
            var propertyType = _property.PropertyType;
            ctx.ReadNullCheckedTail(propertyType, Tail, null); // stack is [old-addr]|[new-value]

            if (writeValue)
            {
                // stack is old-addr|new-value
                CodeLabel @skip = new CodeLabel(), allDone = new CodeLabel(); // <=== default structs
                if (!propertyType.IsValueType)
                {
                    // if the tail returns a null, intepret that as *no assign*
                    ctx.CopyValue(); // old-addr|new-value|new-value
                    @skip = ctx.DefineLabel();
                    allDone = ctx.DefineLabel();
                    ctx.BranchIfFalse(@skip, true); // old-addr|new-value
                }

                if (_shadowSetter == null)
                {
                    ctx.StoreValue(_property);
                }
                else
                {
                    ctx.EmitCall(_shadowSetter);
                }
                if (!propertyType.IsValueType)
                {
                    ctx.Branch(allDone, true);

                    ctx.MarkLabel(@skip); // old-addr|new-value
                    ctx.DiscardValue();
                    ctx.DiscardValue();

                    ctx.MarkLabel(allDone);
                }
            }
            else
            {
                // don't want return value; drop it if anything there
                // stack is [new-value]
                if (Tail.ReturnsValue)
                {
                    ctx.DiscardValue();
                }
            }
        }
#endif
    }
}

#endif