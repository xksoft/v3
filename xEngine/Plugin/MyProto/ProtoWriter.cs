﻿using System;
using System.IO;
using System.Text;
using MyProto.Meta;

#if MF
using OverflowException = System.ApplicationException;
#endif

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
#endif

namespace MyProto
{
    /// <summary>
    ///     Represents an output stream for writing protobuf data.
    ///     Why is the API backwards (static methods with writer arguments)?
    ///     See: http://marcgravell.blogspot.com/2010/03/last-will-be-first-and-first-will-be.html
    /// </summary>
    public sealed class ProtoWriter : IDisposable
    {
        private const int RecursionCheckDepth = 25;
        private static readonly UTF8Encoding Encoding = new UTF8Encoding();
        private readonly SerializationContext _context;
        private readonly NetObjectCache _netCache = new NetObjectCache();
        private int _depth;
        private Stream _dest;
        private int _fieldNumber, _flushLock;
        private byte[] _ioBuffer;
        private int _ioIndex;
        private int _packedFieldNumber;
        private int _position;
        private MutableList _recursionStack;

        /// <summary>
        ///     Creates a new writer against a stream
        /// </summary>
        /// <param name="dest">The destination stream</param>
        /// <param name="model">
        ///     The model to use for serialization; this can be null, but this will impair the ability to serialize
        ///     sub-objects
        /// </param>
        /// <param name="context">Additional context about this serialization operation</param>
        public ProtoWriter(Stream dest, TypeModel model, SerializationContext context)
        {
            if (dest == null) throw new ArgumentNullException("dest");
            if (!dest.CanWrite) throw new ArgumentException("Cannot write to stream", "dest");
            //if (model == null) throw new ArgumentNullException("model");
            _dest = dest;
            _ioBuffer = BufferPool.GetBuffer();
            Model = model;
            WireType = WireType.None;
            if (context == null)
            {
                context = SerializationContext.Default;
            }
            else
            {
                context.Freeze();
            }
            _context = context;
        }

        internal NetObjectCache NetCache
        {
            get { return _netCache; }
        }

        internal WireType WireType { get; private set; }

        /// <summary>
        ///     Addition information about this serialization operation.
        /// </summary>
        public SerializationContext Context
        {
            get { return _context; }
        }

        /// <summary>
        ///     Get the TypeModel associated with this writer
        /// </summary>
        public TypeModel Model { get; private set; }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        /// <summary>
        ///     Write an encapsulated sub-object, using the supplied unique Key (reprasenting a type).
        /// </summary>
        /// <param name="value">The object to write.</param>
        /// <param name="Key">The Key that uniquely identifies the type within the model.</param>
        /// <param name="writer">The destination.</param>
        public static void WriteObject(object value, int key, ProtoWriter writer)
        {
#if FEAT_IKVM
            throw new NotSupportedException();
#else
            if (writer == null) throw new ArgumentNullException("writer");
            if (writer.Model == null)
            {
                throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
            }

            var token = StartSubItem(value, writer);
            if (key >= 0)
            {
                writer.Model.Serialize(key, value, writer);
            }
            else if (writer.Model != null &&
                     writer.Model.TrySerializeAuxiliaryType(writer, value.GetType(), DataFormat.Default,
                         Serializer.ListItemTag, value, false))
            {
                // all ok
            }
            else
            {
                TypeModel.ThrowUnexpectedType(value.GetType());
            }
            EndSubItem(token, writer);
#endif
        }

        /// <summary>
        ///     Write an encapsulated sub-object, using the supplied unique Key (reprasenting a type) - but the
        ///     caller is asserting that this relationship is non-recursive; no recursion check will be
        ///     performed.
        /// </summary>
        /// <param name="value">The object to write.</param>
        /// <param name="Key">The Key that uniquely identifies the type within the model.</param>
        /// <param name="writer">The destination.</param>
        public static void WriteRecursionSafeObject(object value, int key, ProtoWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (writer.Model == null)
            {
                throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
            }
            var token = StartSubItem(null, writer);
            writer.Model.Serialize(key, value, writer);
            EndSubItem(token, writer);
        }

        internal static void WriteObject(object value, int key, ProtoWriter writer, PrefixStyle style, int fieldNumber)
        {
#if FEAT_IKVM
            throw new NotSupportedException();
#else
            if (writer.Model == null)
            {
                throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
            }
            if (writer.WireType != WireType.None) throw CreateException(writer);

            switch (style)
            {
                case PrefixStyle.Base128:
                    writer.WireType = WireType.String;
                    writer._fieldNumber = fieldNumber;
                    if (fieldNumber > 0) WriteHeaderCore(fieldNumber, WireType.String, writer);
                    break;
                case PrefixStyle.Fixed32:
                case PrefixStyle.Fixed32BigEndian:
                    writer._fieldNumber = 0;
                    writer.WireType = WireType.Fixed32;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("style");
            }
            var token = StartSubItem(value, writer, true);
            if (key < 0)
            {
                if (
                    !writer.Model.TrySerializeAuxiliaryType(writer, value.GetType(), DataFormat.Default,
                        Serializer.ListItemTag, value, false))
                {
                    TypeModel.ThrowUnexpectedType(value.GetType());
                }
            }
            else
            {
                writer.Model.Serialize(key, value, writer);
            }
            EndSubItem(token, writer, style);
#endif
        }

        internal int GetTypeKey(ref Type type)
        {
            return Model.GetKey(ref type);
        }

        /// <summary>
        ///     Writes a field-header, indicating the format of the next data we plan to write.
        /// </summary>
        public static void WriteFieldHeader(int fieldNumber, WireType wireType, ProtoWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (writer.WireType != WireType.None)
                throw new InvalidOperationException("Cannot write a " + wireType
                                                    + " header until the " + writer.WireType + " data has been written");
            if (fieldNumber < 0) throw new ArgumentOutOfRangeException("fieldNumber");
#if DEBUG
            switch (wireType)
            {
                // validate requested header-type
                case WireType.Fixed32:
                case WireType.Fixed64:
                case WireType.String:
                case WireType.StartGroup:
                case WireType.SignedVariant:
                case WireType.Variant:
                    break; // fine
                case WireType.None:
                case WireType.EndGroup:
                default:
                    throw new ArgumentException("Invalid wire-type: " + wireType, "wireType");
            }
#endif
            if (writer._packedFieldNumber == 0)
            {
                writer._fieldNumber = fieldNumber;
                writer.WireType = wireType;
                WriteHeaderCore(fieldNumber, wireType, writer);
            }
            else if (writer._packedFieldNumber == fieldNumber)
            {
                // we'll set things up, but note we *don't* actually write the header here
                switch (wireType)
                {
                    case WireType.Fixed32:
                    case WireType.Fixed64:
                    case WireType.Variant:
                    case WireType.SignedVariant:
                        break; // fine
                    default:
                        throw new InvalidOperationException("Wire-type cannot be encoded as packed: " + wireType);
                }
                writer._fieldNumber = fieldNumber;
                writer.WireType = wireType;
            }
            else
            {
                throw new InvalidOperationException("Field mismatch during packed encoding; expected " +
                                                    writer._packedFieldNumber + " but received " + fieldNumber);
            }
        }

        internal static void WriteHeaderCore(int fieldNumber, WireType wireType, ProtoWriter writer)
        {
            var header = (((uint) fieldNumber) << 3)
                         | (((uint) wireType) & 7);
            WriteUInt32Variant(header, writer);
        }

        /// <summary>
        ///     Writes a byte-array to the stream; supported wire-types: String
        /// </summary>
        public static void WriteBytes(byte[] data, ProtoWriter writer)
        {
            if (data == null) throw new ArgumentNullException("data");
            WriteBytes(data, 0, data.Length, writer);
        }

        /// <summary>
        ///     Writes a byte-array to the stream; supported wire-types: String
        /// </summary>
        public static void WriteBytes(byte[] data, int offset, int length, ProtoWriter writer)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (writer == null) throw new ArgumentNullException("writer");
            switch (writer.WireType)
            {
                case WireType.Fixed32:
                    if (length != 4) throw new ArgumentException("length");
                    goto CopyFixedLength; // ugly but effective
                case WireType.Fixed64:
                    if (length != 8) throw new ArgumentException("length");
                    goto CopyFixedLength; // ugly but effective
                case WireType.String:
                    WriteUInt32Variant((uint) length, writer);
                    writer.WireType = WireType.None;
                    if (length == 0) return;
                    if (writer._flushLock != 0 || length <= writer._ioBuffer.Length) // write to the buffer
                    {
                        goto CopyFixedLength; // ugly but effective
                    }
                    // writing data that is bigger than the buffer (and the buffer
                    // isn't currently locked due to a sub-object needing the size backfilled)
                    Flush(writer); // commit any existing data from the buffer
                    // now just write directly to the underlying stream
                    writer._dest.Write(data, offset, length);
                    writer._position += length; // since we've flushed offset etc is 0, and remains
                    // zero since we're writing directly to the stream
                    return;
            }
            throw CreateException(writer);
            CopyFixedLength: // no point duplicating this lots of times, and don't really want another stackframe
            DemandSpace(length, writer);
            Helpers.BlockCopy(data, offset, writer._ioBuffer, writer._ioIndex, length);
            IncrementedAndReset(length, writer);
        }

        private static void CopyRawFromStream(Stream source, ProtoWriter writer)
        {
            var buffer = writer._ioBuffer;
            int space = buffer.Length - writer._ioIndex, bytesRead = 1; // 1 here to spoof case where already full

            // try filling the buffer first   
            while (space > 0 && (bytesRead = source.Read(buffer, writer._ioIndex, space)) > 0)
            {
                writer._ioIndex += bytesRead;
                writer._position += bytesRead;
                space -= bytesRead;
            }
            if (bytesRead <= 0) return; // all done using just the buffer; stream exhausted

            // at this point the stream still has data, but buffer is full; 
            if (writer._flushLock == 0)
            {
                // flush the buffer and write to the underlying stream instead
                Flush(writer);
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    writer._dest.Write(buffer, 0, bytesRead);
                    writer._position += bytesRead;
                }
            }
            else
            {
                do
                {
                    // need more space; resize (double) as necessary,
                    // requesting a reasonable minimum chunk each time
                    // (128 is the minimum; there may actually be much
                    // more space than this in the buffer)
                    DemandSpace(128, writer);
                    if ((bytesRead = source.Read(writer._ioBuffer, writer._ioIndex,
                        writer._ioBuffer.Length - writer._ioIndex)) <= 0) break;
                    writer._position += bytesRead;
                    writer._ioIndex += bytesRead;
                } while (true);
            }
        }

        private static void IncrementedAndReset(int length, ProtoWriter writer)
        {
            Helpers.DebugAssert(length >= 0);
            writer._ioIndex += length;
            writer._position += length;
            writer.WireType = WireType.None;
        }

        /// <summary>
        ///     Indicates the start of a nested record.
        /// </summary>
        /// <param name="instance">The instance to write.</param>
        /// <param name="writer">The destination.</param>
        /// <returns>A token representing the state of the stream; this token is given to EndSubItem.</returns>
        public static SubItemToken StartSubItem(object instance, ProtoWriter writer)
        {
            return StartSubItem(instance, writer, false);
        }

        private void CheckRecursionStackAndPush(object instance)
        {
            int hitLevel;
            if (_recursionStack == null)
            {
                _recursionStack = new MutableList();
            }
            else if (instance != null && (hitLevel = _recursionStack.IndexOfReference(instance)) >= 0)
            {
#if DEBUG
                Helpers.DebugWriteLine("Stack:");
                foreach (var obj in _recursionStack)
                {
                    Helpers.DebugWriteLine(obj == null ? "<null>" : obj.ToString());
                }
                Helpers.DebugWriteLine(instance == null ? "<null>" : instance.ToString());
#endif
                throw new ProtoException("Possible recursion detected (offset: " + (_recursionStack.Count - hitLevel) +
                                         " level(s)): " + instance);
            }
            _recursionStack.Add(instance);
        }

        private void PopRecursionStack()
        {
            _recursionStack.RemoveLast();
        }

        private static SubItemToken StartSubItem(object instance, ProtoWriter writer, bool allowFixed)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (++writer._depth > RecursionCheckDepth)
            {
                writer.CheckRecursionStackAndPush(instance);
            }
            if (writer._packedFieldNumber != 0)
                throw new InvalidOperationException("Cannot begin a sub-item while performing packed encoding");
            switch (writer.WireType)
            {
                case WireType.StartGroup:
                    writer.WireType = WireType.None;
                    return new SubItemToken(-writer._fieldNumber);
                case WireType.String:
#if DEBUG
                    if (writer.Model != null && writer.Model.ForwardsOnly)
                    {
                        throw new ProtoException("Should not be buffering data");
                    }
#endif
                    writer.WireType = WireType.None;
                    DemandSpace(32, writer); // make some space in anticipation...
                    writer._flushLock++;
                    writer._position++;
                    return new SubItemToken(writer._ioIndex++); // leave 1 space (optimistic) for length
                case WireType.Fixed32:
                {
                    if (!allowFixed) throw CreateException(writer);
                    DemandSpace(32, writer); // make some space in anticipation...
                    writer._flushLock++;
                    var token = new SubItemToken(writer._ioIndex);
                    IncrementedAndReset(4, writer); // leave 4 space (rigid) for length
                    return token;
                }
                default:
                    throw CreateException(writer);
            }
        }

        /// <summary>
        ///     Indicates the end of a nested record.
        /// </summary>
        /// <param name="token">The token obtained from StartubItem.</param>
        /// <param name="writer">The destination.</param>
        public static void EndSubItem(SubItemToken token, ProtoWriter writer)
        {
            EndSubItem(token, writer, PrefixStyle.Base128);
        }

        private static void EndSubItem(SubItemToken token, ProtoWriter writer, PrefixStyle style)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (writer.WireType != WireType.None)
            {
                throw CreateException(writer);
            }
            var value = token.Value;
            if (writer._depth <= 0) throw CreateException(writer);
            if (writer._depth-- > RecursionCheckDepth)
            {
                writer.PopRecursionStack();
            }
            writer._packedFieldNumber = 0; // ending the sub-item always wipes packed encoding
            if (value < 0)
            {
                // group - very simple append
                WriteHeaderCore(-value, WireType.EndGroup, writer);
                writer.WireType = WireType.None;
                return;
            }

            // so we're backfilling the length into an existing sequence
            int len;
            switch (style)
            {
                case PrefixStyle.Fixed32:
                    len = (writer._ioIndex - value) - 4;
                    WriteInt32ToBuffer(len, writer._ioBuffer, value);
                    break;
                case PrefixStyle.Fixed32BigEndian:
                    len = (writer._ioIndex - value) - 4;
                    var buffer = writer._ioBuffer;
                    WriteInt32ToBuffer(len, buffer, value);
                    // and swap the byte order
                    var b = buffer[value];
                    buffer[value] = buffer[value + 3];
                    buffer[value + 3] = b;
                    b = buffer[value + 1];
                    buffer[value + 1] = buffer[value + 2];
                    buffer[value + 2] = b;
                    break;
                case PrefixStyle.Base128:
                    // string - complicated because we only reserved one byte;
                    // if the prefix turns out to need more than this then
                    // we need to shuffle the existing data
                    len = (writer._ioIndex - value) - 1;
                    var offset = 0;
                    var tmp = (uint) len;
                    while ((tmp >>= 7) != 0) offset++;
                    if (offset == 0)
                    {
                        writer._ioBuffer[value] = (byte) (len & 0x7F);
                    }
                    else
                    {
                        DemandSpace(offset, writer);
                        var blob = writer._ioBuffer;
                        Helpers.BlockCopy(blob, value + 1, blob, value + 1 + offset, len);
                        tmp = (uint) len;
                        do
                        {
                            blob[value++] = (byte) ((tmp & 0x7F) | 0x80);
                        } while ((tmp >>= 7) != 0);
                        blob[value - 1] = (byte) (blob[value - 1] & ~0x80);
                        writer._position += offset;
                        writer._ioIndex += offset;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("style");
            }
            // and this object is no longer a blockage - also flush if sensible
            const int advisoryFlushSize = 1024;
            if (--writer._flushLock == 0 && writer._ioIndex >= advisoryFlushSize)
            {
                Flush(writer);
            }
        }

        private void Dispose()
        {
            // importantly, this does **not** own the stream, and does not dispose it
            if (_dest != null)
            {
                Flush(this);
                _dest = null;
            }
            Model = null;
            BufferPool.ReleaseBufferToPool(ref _ioBuffer);
        }

        // note that this is used by some of the unit tests and should not be removed
        internal static int GetPosition(ProtoWriter writer)
        {
            return writer._position;
        }

        private static void DemandSpace(int required, ProtoWriter writer)
        {
            // check for enough space
            if ((writer._ioBuffer.Length - writer._ioIndex) < required)
            {
                if (writer._flushLock == 0)
                {
                    Flush(writer); // try emptying the buffer
                    if ((writer._ioBuffer.Length - writer._ioIndex) >= required) return;
                }
                // either can't empty the buffer, or that didn't help; need more space
                BufferPool.ResizeAndFlushLeft(ref writer._ioBuffer, required + writer._ioIndex, 0, writer._ioIndex);
            }
        }

        /// <summary>
        ///     Flushes data to the underlying stream, and releases any resources. The underlying stream is *not* disposed
        ///     by this operation.
        /// </summary>
        public void Close()
        {
            if (_depth != 0 || _flushLock != 0)
                throw new InvalidOperationException("Unable to close stream in an incomplete state");
            Dispose();
        }

        internal void CheckDepthFlushlock()
        {
            if (_depth != 0 || _flushLock != 0)
                throw new InvalidOperationException("The writer is in an incomplete state");
        }

        /// <summary>
        ///     Writes any buffered data (if possible) to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer to flush</param>
        /// <remarks>
        ///     It is not always possible to fully flush, since some sequences
        ///     may require values to be back-filled into the byte-stream.
        /// </remarks>
        internal static void Flush(ProtoWriter writer)
        {
            if (writer._flushLock == 0 && writer._ioIndex != 0)
            {
                writer._dest.Write(writer._ioBuffer, 0, writer._ioIndex);
                writer._ioIndex = 0;
            }
        }

        /// <summary>
        ///     Writes an unsigned 32-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64
        /// </summary>
        private static void WriteUInt32Variant(uint value, ProtoWriter writer)
        {
            DemandSpace(5, writer);
            var count = 0;
            do
            {
                writer._ioBuffer[writer._ioIndex++] = (byte) ((value & 0x7F) | 0x80);
                count++;
            } while ((value >>= 7) != 0);
            writer._ioBuffer[writer._ioIndex - 1] &= 0x7F;
            writer._position += count;
        }

        internal static uint Zig(int value)
        {
            return (uint) ((value << 1) ^ (value >> 31));
        }

        internal static ulong Zig(long value)
        {
            return (ulong) ((value << 1) ^ (value >> 63));
        }

        private static void WriteUInt64Variant(ulong value, ProtoWriter writer)
        {
            DemandSpace(10, writer);
            var count = 0;
            do
            {
                writer._ioBuffer[writer._ioIndex++] = (byte) ((value & 0x7F) | 0x80);
                count++;
            } while ((value >>= 7) != 0);
            writer._ioBuffer[writer._ioIndex - 1] &= 0x7F;
            writer._position += count;
        }

        /// <summary>
        ///     Writes a string to the stream; supported wire-types: String
        /// </summary>
        public static void WriteString(string value, ProtoWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            if (writer.WireType != WireType.String) throw CreateException(writer);
            if (value == null) throw new ArgumentNullException("value"); // written header; now what?
            var len = value.Length;
            if (len == 0)
            {
                WriteUInt32Variant(0, writer);
                writer.WireType = WireType.None;
                return; // just a header
            }
#if MF
            byte[] bytes = encoding.GetBytes(value);
            int actual = bytes.Length;
            writer.WriteUInt32Variant((uint)actual);
            writer.Ensure(actual);
            Helpers.BlockCopy(bytes, 0, writer.ioBuffer, writer.ioIndex, actual);
#else
            var predicted = Encoding.GetByteCount(value);
            WriteUInt32Variant((uint) predicted, writer);
            DemandSpace(predicted, writer);
            var actual = Encoding.GetBytes(value, 0, value.Length, writer._ioBuffer, writer._ioIndex);
            Helpers.DebugAssert(predicted == actual);
#endif
            IncrementedAndReset(actual, writer);
        }

        /// <summary>
        ///     Writes an unsigned 64-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64
        /// </summary>
        public static void WriteUInt64(ulong value, ProtoWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            switch (writer.WireType)
            {
                case WireType.Fixed64:
                    WriteInt64((long) value, writer);
                    return;
                case WireType.Variant:
                    WriteUInt64Variant(value, writer);
                    writer.WireType = WireType.None;
                    return;
                case WireType.Fixed32:
                    checked
                    {
                        WriteUInt32((uint) value, writer);
                    }
                    return;
                default:
                    throw CreateException(writer);
            }
        }

        /// <summary>
        ///     Writes a signed 64-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public static void WriteInt64(long value, ProtoWriter writer)
        {
            byte[] buffer;
            int index;
            if (writer == null) throw new ArgumentNullException("writer");
            switch (writer.WireType)
            {
                case WireType.Fixed64:
                    DemandSpace(8, writer);
                    buffer = writer._ioBuffer;
                    index = writer._ioIndex;
                    buffer[index] = (byte) value;
                    buffer[index + 1] = (byte) (value >> 8);
                    buffer[index + 2] = (byte) (value >> 16);
                    buffer[index + 3] = (byte) (value >> 24);
                    buffer[index + 4] = (byte) (value >> 32);
                    buffer[index + 5] = (byte) (value >> 40);
                    buffer[index + 6] = (byte) (value >> 48);
                    buffer[index + 7] = (byte) (value >> 56);
                    IncrementedAndReset(8, writer);
                    return;
                case WireType.SignedVariant:
                    WriteUInt64Variant(Zig(value), writer);
                    writer.WireType = WireType.None;
                    return;
                case WireType.Variant:
                    if (value >= 0)
                    {
                        WriteUInt64Variant((ulong) value, writer);
                        writer.WireType = WireType.None;
                    }
                    else
                    {
                        DemandSpace(10, writer);
                        buffer = writer._ioBuffer;
                        index = writer._ioIndex;
                        buffer[index] = (byte) (value | 0x80);
                        buffer[index + 1] = (byte) ((int) (value >> 7) | 0x80);
                        buffer[index + 2] = (byte) ((int) (value >> 14) | 0x80);
                        buffer[index + 3] = (byte) ((int) (value >> 21) | 0x80);
                        buffer[index + 4] = (byte) ((int) (value >> 28) | 0x80);
                        buffer[index + 5] = (byte) ((int) (value >> 35) | 0x80);
                        buffer[index + 6] = (byte) ((int) (value >> 42) | 0x80);
                        buffer[index + 7] = (byte) ((int) (value >> 49) | 0x80);
                        buffer[index + 8] = (byte) ((int) (value >> 56) | 0x80);
                        buffer[index + 9] = 0x01; // sign bit
                        IncrementedAndReset(10, writer);
                    }
                    return;
                case WireType.Fixed32:
                    checked
                    {
                        WriteInt32((int) value, writer);
                    }
                    return;
                default:
                    throw CreateException(writer);
            }
        }

        /// <summary>
        ///     Writes an unsigned 16-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64
        /// </summary>
        public static void WriteUInt32(uint value, ProtoWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            switch (writer.WireType)
            {
                case WireType.Fixed32:
                    WriteInt32((int) value, writer);
                    return;
                case WireType.Fixed64:
                    WriteInt64((int) value, writer);
                    return;
                case WireType.Variant:
                    WriteUInt32Variant(value, writer);
                    writer.WireType = WireType.None;
                    return;
                default:
                    throw CreateException(writer);
            }
        }

        /// <summary>
        ///     Writes a signed 16-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public static void WriteInt16(short value, ProtoWriter writer)
        {
            WriteInt32(value, writer);
        }

        /// <summary>
        ///     Writes an unsigned 16-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64
        /// </summary>
        public static void WriteUInt16(ushort value, ProtoWriter writer)
        {
            WriteUInt32(value, writer);
        }

        /// <summary>
        ///     Writes an unsigned 8-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64
        /// </summary>
        public static void WriteByte(byte value, ProtoWriter writer)
        {
            WriteUInt32(value, writer);
        }

        /// <summary>
        ///     Writes a signed 8-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public static void WriteSByte(sbyte value, ProtoWriter writer)
        {
            WriteInt32(value, writer);
        }

        private static void WriteInt32ToBuffer(int value, byte[] buffer, int index)
        {
            buffer[index] = (byte) value;
            buffer[index + 1] = (byte) (value >> 8);
            buffer[index + 2] = (byte) (value >> 16);
            buffer[index + 3] = (byte) (value >> 24);
        }

        /// <summary>
        ///     Writes a signed 32-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public static void WriteInt32(int value, ProtoWriter writer)
        {
            byte[] buffer;
            int index;
            if (writer == null) throw new ArgumentNullException("writer");
            switch (writer.WireType)
            {
                case WireType.Fixed32:
                    DemandSpace(4, writer);
                    WriteInt32ToBuffer(value, writer._ioBuffer, writer._ioIndex);
                    IncrementedAndReset(4, writer);
                    return;
                case WireType.Fixed64:
                    DemandSpace(8, writer);
                    buffer = writer._ioBuffer;
                    index = writer._ioIndex;
                    buffer[index] = (byte) value;
                    buffer[index + 1] = (byte) (value >> 8);
                    buffer[index + 2] = (byte) (value >> 16);
                    buffer[index + 3] = (byte) (value >> 24);
                    buffer[index + 4] = buffer[index + 5] =
                        buffer[index + 6] = buffer[index + 7] = 0;
                    IncrementedAndReset(8, writer);
                    return;
                case WireType.SignedVariant:
                    WriteUInt32Variant(Zig(value), writer);
                    writer.WireType = WireType.None;
                    return;
                case WireType.Variant:
                    if (value >= 0)
                    {
                        WriteUInt32Variant((uint) value, writer);
                        writer.WireType = WireType.None;
                    }
                    else
                    {
                        DemandSpace(10, writer);
                        buffer = writer._ioBuffer;
                        index = writer._ioIndex;
                        buffer[index] = (byte) (value | 0x80);
                        buffer[index + 1] = (byte) ((value >> 7) | 0x80);
                        buffer[index + 2] = (byte) ((value >> 14) | 0x80);
                        buffer[index + 3] = (byte) ((value >> 21) | 0x80);
                        buffer[index + 4] = (byte) ((value >> 28) | 0x80);
                        buffer[index + 5] = buffer[index + 6] =
                            buffer[index + 7] = buffer[index + 8] = 0xFF;
                        buffer[index + 9] = 0x01;
                        IncrementedAndReset(10, writer);
                    }
                    return;
                default:
                    throw CreateException(writer);
            }
        }

        /// <summary>
        ///     Writes a double-precision number to the stream; supported wire-types: Fixed32, Fixed64
        /// </summary>
        public
#if !FEAT_SAFE
            static
#endif
            unsafe void WriteDouble(double value, ProtoWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            switch (writer.WireType)
            {
                case WireType.Fixed32:
                    var f = (float) value;
                    if (Helpers.IsInfinity(f)
                        && !Helpers.IsInfinity(value))
                    {
                        throw new OverflowException();
                    }
                    WriteSingle(f, writer);
                    return;
                case WireType.Fixed64:
#if FEAT_SAFE
                    ProtoWriter.WriteInt64(BitConverter.ToInt64(BitConverter.GetBytes(value), 0), writer);
#else
                    WriteInt64(*(long*) &value, writer);
#endif
                    return;
                default:
                    throw CreateException(writer);
            }
        }

        /// <summary>
        ///     Writes a single-precision number to the stream; supported wire-types: Fixed32, Fixed64
        /// </summary>
        public
#if !FEAT_SAFE
            static
#endif
            unsafe void WriteSingle(float value, ProtoWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            switch (writer.WireType)
            {
                case WireType.Fixed32:
#if FEAT_SAFE
                    ProtoWriter.WriteInt32(BitConverter.ToInt32(BitConverter.GetBytes(value), 0), writer);
#else
                    WriteInt32(*(int*) &value, writer);
#endif
                    return;
                case WireType.Fixed64:
                    WriteDouble(value, writer);
                    return;
                default:
                    throw CreateException(writer);
            }
        }

        /// <summary>
        ///     Throws an exception indicating that the given enum cannot be mapped to a serialized value.
        /// </summary>
        public static void ThrowEnumException(ProtoWriter writer, object enumValue)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            var rhs = enumValue == null ? "<null>" : (enumValue.GetType().FullName + "." + enumValue);
            throw new ProtoException("No wire-value is mapped to the enum " + rhs + " at position " + writer._position);
        }

        // general purpose serialization exception message
        internal static Exception CreateException(ProtoWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            return
                new ProtoException("Invalid serialization operation with wire-type " + writer.WireType + " at position " +
                                   writer._position);
        }

        /// <summary>
        ///     Writes a boolean to the stream; supported wire-types: Variant, Fixed32, Fixed64
        /// </summary>
        public static void WriteBoolean(bool value, ProtoWriter writer)
        {
            WriteUInt32(value ? 1 : (uint) 0, writer);
        }

        /// <summary>
        ///     Copies any extension data stored for the instance to the underlying stream
        /// </summary>
        public static void AppendExtensionData(IExtensible instance, ProtoWriter writer)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            if (writer == null) throw new ArgumentNullException("writer");
            // we expect the writer to be raw here; the extension data will have the
            // header detail, so we'll copy it implicitly
            if (writer.WireType != WireType.None) throw CreateException(writer);

            var extn = instance.GetExtensionObject(false);
            if (extn != null)
            {
                // unusually we *don't* want "using" here; the "finally" does that, with
                // the extension object being responsible for disposal etc
                var source = extn.BeginQuery();
                try
                {
                    CopyRawFromStream(source, writer);
                }
                finally
                {
                    extn.EndQuery(source);
                }
            }
        }

        /// <summary>
        ///     Used for packed encoding; indicates that the next field should be skipped rather than
        ///     a field header written. Note that the field number must match, else an exception is thrown
        ///     when the attempt is made to write the (incorrect) field. The wire-type is taken from the
        ///     subsequent call to WriteFieldHeader. Only primitive types can be packed.
        /// </summary>
        public static void SetPackedField(int fieldNumber, ProtoWriter writer)
        {
            if (fieldNumber <= 0) throw new ArgumentOutOfRangeException("fieldNumber");
            if (writer == null) throw new ArgumentNullException("writer");
            writer._packedFieldNumber = fieldNumber;
        }

        internal string SerializeType(Type type)
        {
            return TypeModel.SerializeType(Model, type);
        }

        /// <summary>
        ///     Specifies a known root object to use during reference-tracked serialization
        /// </summary>
        public void SetRootObject(object value)
        {
            NetCache.SetKeyedObject(NetObjectCache.Root, value);
        }

        /// <summary>
        ///     Writes a Type to the stream, using the model's DynamicTypeFormatting if appropriate; supported wire-types: String
        /// </summary>
        public static void WriteType(Type value, ProtoWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");
            WriteString(writer.SerializeType(value), writer);
        }
    }
}