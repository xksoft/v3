using System;
using System.Runtime.Serialization;

namespace MyProto
{
    /// <summary>
    ///     Additional information about a serialization operation
    /// </summary>
    public sealed class SerializationContext
    {
        private static readonly SerializationContext @default;
        private object _context;
        private bool _frozen;

        static SerializationContext()
        {
            @default = new SerializationContext();
            @default.Freeze();
        }

        /// <summary>
        ///     Gets or sets a user-defined object containing additional information about this serialization/deserialization
        ///     operation.
        /// </summary>
        public object Context
        {
            get { return _context; }
            set
            {
                if (_context != value)
                {
                    ThrowIfFrozen();
                    _context = value;
                }
            }
        }

        /// <summary>
        ///     A default SerializationContext, with minimal information.
        /// </summary>
        internal static SerializationContext Default
        {
            get { return @default; }
        }

        internal void Freeze()
        {
            _frozen = true;
        }

        private void ThrowIfFrozen()
        {
            if (_frozen)
                throw new InvalidOperationException("The serialization-context cannot be changed once it is in use");
        }

#if PLAT_BINARYFORMATTER || (SILVERLIGHT && NET_4_0)

#if !(WINRT || PHONE7 || PHONE8)
        private StreamingContextStates _state = StreamingContextStates.Persistence;

        /// <summary>
        ///     Gets or sets the source or destination of the transmitted data.
        /// </summary>
        public StreamingContextStates State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    ThrowIfFrozen();
                    _state = value;
                }
            }
        }
#endif

        /// <summary>
        ///     Convert a SerializationContext to a StreamingContext
        /// </summary>
        public static implicit operator StreamingContext(SerializationContext ctx)
        {
#if WINRT || PHONE7 || PHONE8
            return new System.Runtime.Serialization.StreamingContext();
#else
            if (ctx == null) return new StreamingContext(StreamingContextStates.Persistence);
            return new StreamingContext(ctx._state, ctx._context);
#endif
        }

        /// <summary>
        ///     Convert a StreamingContext to a SerializationContext
        /// </summary>
        public static implicit operator SerializationContext(StreamingContext ctx)
        {
            var result = new SerializationContext();
#if !(WINRT || PHONE7 || PHONE8)
            result.Context = ctx.Context;
            result.State = ctx.State;
#endif
            return result;
        }
#endif
    }
}