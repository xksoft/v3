using xEngine.Plugin.MyProto.Compiler;
#if FEAT_COMPILER
using System;
#if FEAT_IKVM
using IKVM.Reflection.Emit;
using Type  = IKVM.Reflection.Type;
#else
using System.Reflection.Emit;

#endif

namespace MyProto.Compiler
{
    internal sealed class Local : IDisposable
    {
        private readonly Type _type;
        private CompilerContext _ctx;
        // public static readonly Local InputValue = new Local(null, null);
        private LocalBuilder _value;

        private Local(LocalBuilder value, Type type)
        {
            _value = value;
            _type = type;
        }

        internal Local(CompilerContext ctx, Type type)
        {
            _ctx = ctx;
            if (ctx != null)
            {
                _value = ctx.GetFromPool(type);
            }
            _type = type;
        }

        public Type Type
        {
            get { return _type; }
        }

        internal LocalBuilder Value
        {
            get
            {
                if (_value == null)
                {
                    throw new ObjectDisposedException(GetType().Name);
                }
                return _value;
            }
        }

        public void Dispose()
        {
            if (_ctx != null)
            {
                // only *actually* dispose if this is context-bound; note that non-bound
                // objects are cheekily re-used, and *must* be left intact agter a "using" etc
                _ctx.ReleaseToPool(_value);
                _value = null;
                _ctx = null;
            }
        }

        public Local AsCopy()
        {
            if (_ctx == null) return this; // can re-use if context-free
            return new Local(_value, _type);
        }

        internal bool IsSame(Local other)
        {
            if (this == other) return true;

            object ourVal = _value; // use prop to ensure obj-disposed etc
            return other != null && ourVal == other._value;
        }
    }
}

#endif