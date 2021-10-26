using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MyProto.Meta;

#if FEAT_IKVM
using Type = IKVM.Reflection.Type;
#endif

#if MF
using EndOfStreamException = System.ApplicationException;
using OverflowException = System.ApplicationException;
#endif

namespace MyProto
{
    /// <summary>
    ///     A stateful reader, used to read a protobuf stream. Typical usage would be (sequentially) to call
    ///     ReadFieldHeader and (after matching the field) an appropriate Read* method.
    /// </summary>
    public sealed class ProtoReader : IDisposable
    {
        private Stream _source;
        private byte[] _ioBuffer;

        private WireType _wireType = WireType.None;

        /// <summary>
        ///     Gets the number of the field being processed.
        /// </summary>
        public int FieldNumber { 
            get; 
            private set;
        }

        /// <summary>
        ///     Indicates the underlying proto serialization format on the wire.
        /// </summary>
        public WireType WireType
        {
            get { return _wireType; }
        }

        /// <summary>
        ///     Creates a new reader against a stream
        /// </summary>
        /// <param name="source">The source stream</param>
        /// <param name="model">
        ///     The model to use for serialization; this can be null, but this will impair the ability to
        ///     deserialize sub-objects
        /// </param>
        /// <param name="context">Additional context about this serialization operation</param>
        public ProtoReader(Stream source, TypeModel model, SerializationContext context) :
            this(source, model, context, -1)
        {
        }


        private int _dataRemaining;
        private readonly bool _isFixedLength;
        private bool _internStrings = true;

        /// <summary>
        ///     Gets / sets a flag indicating whether strings should be checked for repetition; if
        ///     true, any repeated UTF-8 byte sequence will result in the same String instance, rather
        ///     than a second instance of the same string. Enabled by default. Note that this uses
        ///     a <i>custom</i> interner - the system-wide string interner is not used.
        /// </summary>
        public bool InternStrings
        {
            get { return _internStrings; }
            set { _internStrings = value; }
        }

        /// <summary>
        ///     Creates a new reader against a stream
        /// </summary>
        /// <param name="source">The source stream</param>
        /// <param name="model">
        ///     The model to use for serialization; this can be null, but this will impair the ability to
        ///     deserialize sub-objects
        /// </param>
        /// <param name="context">Additional context about this serialization operation</param>
        /// <param name="length">The number of bytes to read, or -1 to read until the end of the stream</param>
        public ProtoReader(Stream source, TypeModel model, SerializationContext context, int length)
        {
           
            if (source == null) throw new ArgumentNullException("source");
            if (!source.CanRead) throw new ArgumentException("Cannot read from stream", "source");
            _source = source;
            _ioBuffer = BufferPool.GetBuffer();
            Model = model;
            _isFixedLength = length >= 0;
            _dataRemaining = _isFixedLength ? length : 0;

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

        private readonly SerializationContext _context;

        /// <summary>
        ///     Addition information about this deserialization operation.
        /// </summary>
        public SerializationContext Context
        {
            get { return _context; }
        }

        /// <summary>
        ///     Releases resources used by the reader, but importantly <b>does not</b> Dispose the
        ///     underlying stream; in many typical use-cases the stream is used for different
        ///     processes, so it is assumed that the consumer will Dispose their stream separately.
        /// </summary>
        public void Dispose()
        {
            // importantly, this does **not** own the stream, and does not dispose it
            _source = null;
            Model = null;
            BufferPool.ReleaseBufferToPool(ref _ioBuffer);
        }

        internal int TryReadUInt32VariantWithoutMoving(bool trimNegative, out uint value)
        {
            if (_available < 10) Ensure(10, false);
            if (_available == 0)
            {
                value = 0;
                return 0;
            }
            var readPos = _ioIndex;
            value = _ioBuffer[readPos++];
            if ((value & 0x80) == 0) return 1;
            value &= 0x7F;
            if (_available == 1) throw EoF(this);

            uint chunk = _ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 7;
            if ((chunk & 0x80) == 0) return 2;
            if (_available == 2) throw EoF(this);

            chunk = _ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 14;
            if ((chunk & 0x80) == 0) return 3;
            if (_available == 3) throw EoF(this);

            chunk = _ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 21;
            if ((chunk & 0x80) == 0) return 4;
            if (_available == 4) throw EoF(this);

            chunk = _ioBuffer[readPos];
            value |= chunk << 28; // can only use 4 bits from this chunk
            if ((chunk & 0xF0) == 0) return 5;

            if (trimNegative // allow for -ve values
                && (chunk & 0xF0) == 0xF0
                && _available >= 10
                && _ioBuffer[++readPos] == 0xFF
                && _ioBuffer[++readPos] == 0xFF
                && _ioBuffer[++readPos] == 0xFF
                && _ioBuffer[++readPos] == 0xFF
                && _ioBuffer[++readPos] == 0x01)
            {
                return 10;
            }
            throw AddErrorData(new OverflowException(), this);
        }

        private uint ReadUInt32Variant(bool trimNegative)
        {
            uint value;
            var read = TryReadUInt32VariantWithoutMoving(trimNegative, out value);
            if (read > 0)
            {
                _ioIndex += read;
                _available -= read;
                Position += read;
                return value;
            }
            throw EoF(this);
        }

        private bool TryReadUInt32Variant(out uint value)
        {
            var read = TryReadUInt32VariantWithoutMoving(false, out value);
            if (read > 0)
            {
                _ioIndex += read;
                _available -= read;
                Position += read;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Reads an unsigned 32-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64
        /// </summary>
        public uint ReadUInt32()
        {
            switch (_wireType)
            {
                case WireType.Variant:
                    return ReadUInt32Variant(false);
                case WireType.Fixed32:
                    if (_available < 4) Ensure(4, true);
                    Position += 4;
                    _available -= 4;
                    return _ioBuffer[_ioIndex++]
                           | (((uint) _ioBuffer[_ioIndex++]) << 8)
                           | (((uint) _ioBuffer[_ioIndex++]) << 16)
                           | (((uint) _ioBuffer[_ioIndex++]) << 24);
                case WireType.Fixed64:
                    var val = ReadUInt64();
                    checked
                    {
                        return (uint) val;
                    }
                default:
                    throw CreateWireTypeException();
            }
        }

        private int _ioIndex, _available; // maxPosition

        /// <summary>
        ///     Returns the position of the current reader (note that this is not necessarily the same as the position
        ///     in the underlying stream, if multiple readers are used on the same stream)
        /// </summary>
        public int Position { get; private set; }

        internal void Ensure(int count, bool strict)
        {
            Helpers.DebugAssert(_available <= count, "Asking for data without checking first");
            if (count > _ioBuffer.Length)
            {
                BufferPool.ResizeAndFlushLeft(ref _ioBuffer, count, _ioIndex, _available);
                _ioIndex = 0;
            }
            else if (_ioIndex + count >= _ioBuffer.Length)
            {
                // need to shift the buffer data to the left to make space
                Helpers.BlockCopy(_ioBuffer, _ioIndex, _ioBuffer, 0, _available);
                _ioIndex = 0;
            }
            count -= _available;
            int writePos = _ioIndex + _available, bytesRead;
            var canRead = _ioBuffer.Length - writePos;
            if (_isFixedLength)
            {
                // throttle it if needed
                if (_dataRemaining < canRead) canRead = _dataRemaining;
            }
            while (count > 0 && canRead > 0 && (bytesRead = _source.Read(_ioBuffer, writePos, canRead)) > 0)
            {
                _available += bytesRead;
                count -= bytesRead;
                canRead -= bytesRead;
                writePos += bytesRead;
                if (_isFixedLength)
                {
                    _dataRemaining -= bytesRead;
                }
            }
            if (strict && count > 0)
            {
                throw EoF(this);
            }
        }

        /// <summary>
        ///     Reads a signed 16-bit integer from the stream: Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public short ReadInt16()
        {
            checked
            {
                return (short) ReadInt32();
            }
        }

        /// <summary>
        ///     Reads an unsigned 16-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64
        /// </summary>
        public ushort ReadUInt16()
        {
            checked
            {
                return (ushort) ReadUInt32();
            }
        }

        /// <summary>
        ///     Reads an unsigned 8-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64
        /// </summary>
        public byte ReadByte()
        {
            checked
            {
                return (byte) ReadUInt32();
            }
        }

        /// <summary>
        ///     Reads a signed 8-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public sbyte ReadSByte()
        {
            checked
            {
                return (sbyte) ReadInt32();
            }
        }

        /// <summary>
        ///     Reads a signed 32-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public int ReadInt32()
        {
            switch (_wireType)
            {
                case WireType.Variant:
                    return (int) ReadUInt32Variant(true);
                case WireType.Fixed32:
                    if (_available < 4) Ensure(4, true);
                    Position += 4;
                    _available -= 4;
                    return _ioBuffer[_ioIndex++]
                           | (_ioBuffer[_ioIndex++] << 8)
                           | (_ioBuffer[_ioIndex++] << 16)
                           | (_ioBuffer[_ioIndex++] << 24);
                case WireType.Fixed64:
                    var l = ReadInt64();
                    checked
                    {
                        return (int) l;
                    }
                case WireType.SignedVariant:
                    return Zag(ReadUInt32Variant(true));
                default:
                    throw CreateWireTypeException();
            }
        }

        private const long Int64Msb = ((long) 1) << 63;
        private const int Int32Msb = 1 << 31;

        private static int Zag(uint ziggedValue)
        {
            var value = (int) ziggedValue;
            return (-(value & 0x01)) ^ ((value >> 1) & ~Int32Msb);
        }

        private static long Zag(ulong ziggedValue)
        {
            var value = (long) ziggedValue;
            return (-(value & 0x01L)) ^ ((value >> 1) & ~Int64Msb);
        }

        /// <summary>
        ///     Reads a signed 64-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public long ReadInt64()
        {
            switch (_wireType)
            {
                case WireType.Variant:
                    return (long) ReadUInt64Variant();
                case WireType.Fixed32:
                    return ReadInt32();
                case WireType.Fixed64:
                    if (_available < 8) Ensure(8, true);
                    Position += 8;
                    _available -= 8;

                    return _ioBuffer[_ioIndex++]
                           | (((long) _ioBuffer[_ioIndex++]) << 8)
                           | (((long) _ioBuffer[_ioIndex++]) << 16)
                           | (((long) _ioBuffer[_ioIndex++]) << 24)
                           | (((long) _ioBuffer[_ioIndex++]) << 32)
                           | (((long) _ioBuffer[_ioIndex++]) << 40)
                           | (((long) _ioBuffer[_ioIndex++]) << 48)
                           | (((long) _ioBuffer[_ioIndex++]) << 56);

                case WireType.SignedVariant:
                    return Zag(ReadUInt64Variant());
                default:
                    throw CreateWireTypeException();
            }
        }

        private int TryReadUInt64VariantWithoutMoving(out ulong value)
        {
            if (_available < 10) Ensure(10, false);
            if (_available == 0)
            {
                value = 0;
                return 0;
            }
            var readPos = _ioIndex;
            value = _ioBuffer[readPos++];
            if ((value & 0x80) == 0) return 1;
            value &= 0x7F;
            if (_available == 1) throw EoF(this);

            ulong chunk = _ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 7;
            if ((chunk & 0x80) == 0) return 2;
            if (_available == 2) throw EoF(this);

            chunk = _ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 14;
            if ((chunk & 0x80) == 0) return 3;
            if (_available == 3) throw EoF(this);

            chunk = _ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 21;
            if ((chunk & 0x80) == 0) return 4;
            if (_available == 4) throw EoF(this);

            chunk = _ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 28;
            if ((chunk & 0x80) == 0) return 5;
            if (_available == 5) throw EoF(this);

            chunk = _ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 35;
            if ((chunk & 0x80) == 0) return 6;
            if (_available == 6) throw EoF(this);

            chunk = _ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 42;
            if ((chunk & 0x80) == 0) return 7;
            if (_available == 7) throw EoF(this);


            chunk = _ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 49;
            if ((chunk & 0x80) == 0) return 8;
            if (_available == 8) throw EoF(this);

            chunk = _ioBuffer[readPos++];
            value |= (chunk & 0x7F) << 56;
            if ((chunk & 0x80) == 0) return 9;
            if (_available == 9) throw EoF(this);

            chunk = _ioBuffer[readPos];
            value |= chunk << 63; // can only use 1 bit from this chunk

            if ((chunk & ~(ulong) 0x01) != 0) throw AddErrorData(new OverflowException(), this);
            return 10;
        }

        private ulong ReadUInt64Variant()
        {
            ulong value;
            var read = TryReadUInt64VariantWithoutMoving(out value);
            if (read > 0)
            {
                _ioIndex += read;
                _available -= read;
                Position += read;
                return value;
            }
            throw EoF(this);
        }

#if NO_GENERICS
        private System.Collections.Hashtable stringInterner;
        private string Intern(string value)
        {
            if (value == null) return null;
            if (value.Length == 0) return "";
            if (stringInterner == null)
            {
                stringInterner = new System.Collections.Hashtable();
                stringInterner.Add(value, value);      
            }
            else if (stringInterner.ContainsKey(value))
            {
                value = (string)stringInterner[value];
            }
            else
            {
                stringInterner.Add(value, value);
            }
            return value;
        }
#else
        private Dictionary<string, string> _stringInterner;

        private string Intern(string value)
        {
            if (value == null) return null;
            if (value.Length == 0) return "";
            string found;
            if (_stringInterner == null)
            {
                _stringInterner = new Dictionary<string, string>();
                _stringInterner.Add(value, value);
            }
            else if (_stringInterner.TryGetValue(value, out found))
            {
                value = found;
            }
            else
            {
                _stringInterner.Add(value, value);
            }
            return value;
        }
#endif

        private static readonly UTF8Encoding Encoding = new UTF8Encoding();

        /// <summary>
        ///     Reads a string from the stream (using UTF8); supported wire-types: String
        /// </summary>
        public string ReadString()
        {
            if (_wireType == WireType.String)
            {
                var bytes = (int) ReadUInt32Variant(false);
                if (bytes == 0) return "";
                if (_available < bytes) Ensure(bytes, true);
#if MF
                byte[] tmp;
                if(ioIndex == 0 && bytes == ioBuffer.Length) {
                    // unlikely, but...
                    tmp = ioBuffer;
                } else {
                    tmp = new byte[bytes];
                    Helpers.BlockCopy(ioBuffer, ioIndex, tmp, 0, bytes);
                }
                string s = new string(encoding.GetChars(tmp));
#else
                var s = Encoding.GetString(_ioBuffer, _ioIndex, bytes);
#endif
                if (_internStrings)
                {
                    s = Intern(s);
                }
                _available -= bytes;
                Position += bytes;
                _ioIndex += bytes;
                return s;
            }
            
            throw CreateWireTypeException();
        }

        /// <summary>
        ///     Throws an exception indication that the given value cannot be mapped to an enum.
        /// </summary>
        public void ThrowEnumException(Type type, int value)
        {
            var desc = type == null ? "<null>" : type.FullName;
            throw AddErrorData(new ProtoException("No " + desc + " enum is mapped to the wire-value " + value), this);
        }

        private Exception CreateWireTypeException()
        {

            return
                CreateException(
                    "Invalid wire-type; this usually means you have over-written a file without truncating or setting the length");
        }

        private Exception CreateException(string message)
        {
            return AddErrorData(new ProtoException(message), this);
        }

        /// <summary>
        ///     Reads a double-precision number from the stream; supported wire-types: Fixed32, Fixed64
        /// </summary>
        public
#if !FEAT_SAFE
            unsafe
#endif
            double ReadDouble()
        {
            switch (_wireType)
            {
                case WireType.Fixed32:
                    return ReadSingle();
                case WireType.Fixed64:
                    var value = ReadInt64();
#if FEAT_SAFE
                    return BitConverter.ToDouble(BitConverter.GetBytes(value), 0);
#else
                    return *(double*) &value;
#endif
                default:
                    throw CreateWireTypeException();
            }
        }

        /// <summary>
        ///     Reads (merges) a sub-message from the stream, internally calling StartSubItem and EndSubItem, and (in between)
        ///     parsing the message in accordance with the model associated with the reader
        /// </summary>
        public static object ReadObject(object value, int key, ProtoReader reader)
        {
#if FEAT_IKVM
            throw new NotSupportedException();
#else
            return ReadTypedObject(value, key, reader, null);
#endif
        }

#if !FEAT_IKVM
        internal static object ReadTypedObject(object value, int key, ProtoReader reader, Type type)
        {
            if (reader.Model == null)
            {
                throw AddErrorData(
                    new InvalidOperationException("Cannot deserialize sub-objects unless a model is provided"), reader);
            }
            var token = StartSubItem(reader);
            if (key >= 0)
            {
                value = reader.Model.Deserialize(key, value, reader);
            }
            else if (type != null &&
                     reader.Model.TryDeserializeAuxiliaryType(reader, DataFormat.Default, Serializer.ListItemTag, type,
                         ref value, true, false, true, false))
            {
                // ok
            }
            else
            {
                TypeModel.ThrowUnexpectedType(type);
            }
            EndSubItem(token, reader);
            return value;
        }
#endif

        /// <summary>
        ///     Makes the end of consuming a nested message in the stream; the stream must be either at the correct EndGroup
        ///     marker, or all fields of the sub-message must have been consumed (in either case, this means ReadFieldHeader
        ///     should return zero)
        /// </summary>
        public static void EndSubItem(SubItemToken token, ProtoReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            var value = token.Value;
            switch (reader._wireType)
            {
                case WireType.EndGroup:
                    if (value >= 0) throw AddErrorData(new ArgumentException("token"), reader);
                    if (-value != reader.FieldNumber)
                        throw reader.CreateException("Wrong group was ended"); // wrong group ended!
                    reader._wireType = WireType.None; // this releases ReadFieldHeader
                    reader._depth--;
                    break;
                // case WireType.None: // TODO reinstate once reads reset the wire-type
                default:
                    if (value < reader.Position) throw reader.CreateException("Sub-message not read entirely");
                    if (reader._blockEnd != reader.Position && reader._blockEnd != int.MaxValue)
                    {
                        throw reader.CreateException("Sub-message not read correctly");
                    }
                    reader._blockEnd = value;
                    reader._depth--;
                    break;
                /*default:
                    throw reader.BorkedIt(); */
            }
        }

        /// <summary>
        ///     Begins consuming a nested message in the stream; supported wire-types: StartGroup, String
        /// </summary>
        /// <remarks>The token returned must be help and used when callining EndSubItem</remarks>
        public static SubItemToken StartSubItem(ProtoReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            switch (reader._wireType)
            {
                case WireType.StartGroup:
                    reader._wireType = WireType.None; // to prevent glitches from double-calling
                    reader._depth++;
                    return new SubItemToken(-reader.FieldNumber);
                case WireType.String:
                    var len = (int) reader.ReadUInt32Variant(false);
                    if (len < 0) throw AddErrorData(new InvalidOperationException(), reader);
                    var lastEnd = reader._blockEnd;
                    reader._blockEnd = reader.Position + len;
                    reader._depth++;
                    return new SubItemToken(lastEnd);
                default:
                    throw reader.CreateWireTypeException(); // throws
            }
        }

        private int _depth, _blockEnd = int.MaxValue;

        /// <summary>
        ///     Reads a field header from the stream, setting the wire-type and retuning the field number. If no
        ///     more fields are available, then 0 is returned. This methods respects sub-messages.
        /// </summary>
        public int ReadFieldHeader()
        {
            // at the end of a group the caller must call EndSubItem to release the
            // reader (which moves the status to Error, since ReadFieldHeader must
            // then be called)
            if (_blockEnd <= Position || _wireType == WireType.EndGroup)
            {
                return 0;
            }
            uint tag;
            if (TryReadUInt32Variant(out tag))
            {
                _wireType = (WireType) (tag & 7);
                FieldNumber = (int) (tag >> 3);
                if (FieldNumber < 1) throw new ProtoException("Invalid field in source data: " + FieldNumber);
            }
            else
            {
                _wireType = WireType.None;
                FieldNumber = 0;
            }
            if (_wireType == WireType.EndGroup)
            {
                if (_depth > 0) return 0; // spoof an end, but note we still set the field-number
                throw new ProtoException(
                    "Unexpected end-group in source data; this usually means the source data is corrupt");
            }
            return FieldNumber;
        }

        /// <summary>
        ///     Looks ahead to see whether the next field in the stream is what we expect
        ///     (typically; what we've just finished reading - for example ot read successive list items)
        /// </summary>
        public bool TryReadFieldHeader(int field)
        {
            // check for virtual end of stream
            if (_blockEnd <= Position || _wireType == WireType.EndGroup)
            {
                return false;
            }
            uint tag;
            var read = TryReadUInt32VariantWithoutMoving(false, out tag);
            WireType tmpWireType; // need to catch this to exclude (early) any "end group" tokens
            if (read > 0 && ((int) tag >> 3) == field
                && (tmpWireType = (WireType) (tag & 7)) != WireType.EndGroup)
            {
                _wireType = tmpWireType;
                FieldNumber = field;
                Position += read;
                _ioIndex += read;
                _available -= read;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Get the TypeModel associated with this reader
        /// </summary>
        public TypeModel Model { get; private set; }

        /// <summary>
        ///     Compares the streams current wire-type to the hinted wire-type, updating the reader if necessary; for example,
        ///     a Variant may be updated to SignedVariant. If the hinted wire-type is unrelated then no change is made.
        /// </summary>
        public void Hint(WireType wireType)
        {
            if (_wireType == wireType)
            {
            } // fine; everything as we expect
            else if (((int) wireType & 7) == (int) _wireType)
            {
                // the underling type is a match; we're customising it with an extension
                _wireType = wireType;
            }
            // note no error here; we're OK about using alternative data
        }

        /// <summary>
        ///     Verifies that the stream's current wire-type is as expected, or a specialized sub-type (for example,
        ///     SignedVariant) - in which case the current wire-type is updated. Otherwise an exception is thrown.
        /// </summary>
        public void Assert(WireType wireType)
        {
            if (_wireType == wireType)
            {
            } // fine; everything as we expect
            else if (((int) wireType & 7) == (int) _wireType)
            {
                // the underling type is a match; we're customising it with an extension
                _wireType = wireType;
            }
            else
            {
                // nope; that is *not* what we were expecting!
                throw CreateWireTypeException();
            }
        }

        /// <summary>
        ///     Discards the data for the current field.
        /// </summary>
        public void SkipField()
        {
            switch (_wireType)
            {
                case WireType.Fixed32:
                    if (_available < 4) Ensure(4, true);
                    _available -= 4;
                    _ioIndex += 4;
                    Position += 4;
                    return;
                case WireType.Fixed64:
                    if (_available < 8) Ensure(8, true);
                    _available -= 8;
                    _ioIndex += 8;
                    Position += 8;
                    return;
                case WireType.String:
                    var len = (int) ReadUInt32Variant(false);
                    if (len <= _available)
                    {
                        // just jump it!
                        _available -= len;
                        _ioIndex += len;
                        Position += len;
                        return;
                    }
                    // everything remaining in the buffer is garbage
                    Position += len; // assumes success, but if it fails we're screwed anyway
                    len -= _available; // discount anything we've got to-hand
                    _ioIndex = _available = 0; // note that we have no data in the buffer
                    if (_isFixedLength)
                    {
                        if (len > _dataRemaining) throw EoF(this);
                        // else assume we're going to be OK
                        _dataRemaining -= len;
                    }
                    Seek(_source, len, _ioBuffer);
                    return;
                case WireType.Variant:
                case WireType.SignedVariant:
                    ReadUInt64Variant(); // and drop it
                    return;
                case WireType.StartGroup:
                    var originalFieldNumber = FieldNumber;
                    _depth++; // need to satisfy the sanity-checks in ReadFieldHeader
                    while (ReadFieldHeader() > 0)
                    {
                        SkipField();
                    }
                    _depth--;
                    if (_wireType == WireType.EndGroup && FieldNumber == originalFieldNumber)
                    {
                        // we expect to exit in a similar state to how we entered
                        _wireType = WireType.None;
                        return;
                    }
                    throw CreateWireTypeException();
                case WireType.None: // treat as explicit errorr
                case WireType.EndGroup: // treat as explicit error
                default: // treat as implicit error
                    throw CreateWireTypeException();
            }
        }

        /// <summary>
        ///     Reads an unsigned 64-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64
        /// </summary>
        public ulong ReadUInt64()
        {
            switch (_wireType)
            {
                case WireType.Variant:
                    return ReadUInt64Variant();
                case WireType.Fixed32:
                    return ReadUInt32();
                case WireType.Fixed64:
                    if (_available < 8) Ensure(8, true);
                    Position += 8;
                    _available -= 8;

                    return _ioBuffer[_ioIndex++]
                           | (((ulong) _ioBuffer[_ioIndex++]) << 8)
                           | (((ulong) _ioBuffer[_ioIndex++]) << 16)
                           | (((ulong) _ioBuffer[_ioIndex++]) << 24)
                           | (((ulong) _ioBuffer[_ioIndex++]) << 32)
                           | (((ulong) _ioBuffer[_ioIndex++]) << 40)
                           | (((ulong) _ioBuffer[_ioIndex++]) << 48)
                           | (((ulong) _ioBuffer[_ioIndex++]) << 56);
                default:
                    throw CreateWireTypeException();
            }
        }

        /// <summary>
        ///     Reads a single-precision number from the stream; supported wire-types: Fixed32, Fixed64
        /// </summary>
        public
#if !FEAT_SAFE
            unsafe
#endif
            float ReadSingle()
        {
            switch (_wireType)
            {
                case WireType.Fixed32:
                {
                    var value = ReadInt32();
#if FEAT_SAFE
                        return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
#else
                    return *(float*) &value;
#endif
                }
                case WireType.Fixed64:
                {
                    var value = ReadDouble();
                    var f = (float) value;
                    if (Helpers.IsInfinity(f)
                        && !Helpers.IsInfinity(value))
                    {
                        throw AddErrorData(new OverflowException(), this);
                    }
                    return f;
                }
                default:
                    throw CreateWireTypeException();
            }
        }

        /// <summary>
        ///     Reads a boolean value from the stream; supported wire-types: Variant, Fixed32, Fixed64
        /// </summary>
        /// <returns></returns>
        public bool ReadBoolean()
        {
            switch (ReadUInt32())
            {
                case 0:
                    return false;
                case 1:
                    return true;
                default:
                    throw CreateException("Unexpected boolean value");
            }
        }

        private static readonly byte[] EmptyBlob = new byte[0];

        /// <summary>
        ///     Reads a byte-sequence from the stream, appending them to an existing byte-sequence (which can be null); supported
        ///     wire-types: String
        /// </summary>
        public static byte[] AppendBytes(byte[] value, ProtoReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            switch (reader._wireType)
            {
                case WireType.String:
                    var len = (int) reader.ReadUInt32Variant(false);
                    reader._wireType = WireType.None;
                    if (len == 0) return value == null ? EmptyBlob : value;
                    int offset;
                    if (value == null || value.Length == 0)
                    {
                        offset = 0;
                        value = new byte[len];
                    }
                    else
                    {
                        offset = value.Length;
                        var tmp = new byte[value.Length + len];
                        Helpers.BlockCopy(value, 0, tmp, 0, value.Length);
                        value = tmp;
                    }
                    // value is now sized with the final length, and (if necessary)
                    // contains the old data up to "offset"
                    reader.Position += len; // assume success
                    while (len > reader._available)
                    {
                        if (reader._available > 0)
                        {
                            // copy what we *do* have
                            Helpers.BlockCopy(reader._ioBuffer, reader._ioIndex, value, offset, reader._available);
                            len -= reader._available;
                            offset += reader._available;
                            reader._ioIndex = reader._available = 0; // we've drained the buffer
                        }
                        //  now refill the buffer (without overflowing it)
                        var count = len > reader._ioBuffer.Length ? reader._ioBuffer.Length : len;
                        if (count > 0) reader.Ensure(count, true);
                    }
                    // at this point, we know that len <= available
                    if (len > 0)
                    {
                        // still need data, but we have enough buffered
                        Helpers.BlockCopy(reader._ioBuffer, reader._ioIndex, value, offset, len);
                        reader._ioIndex += len;
                        reader._available -= len;
                    }
                    return value;
                default:
                    throw reader.CreateWireTypeException();
            }
        }

        //static byte[] ReadBytes(Stream stream, int length)
        //{
        //    if (stream == null) throw new ArgumentNullException("stream");
        //    if (length < 0) throw new ArgumentOutOfRangeException("length");
        //    byte[] buffer = new byte[length];
        //    int offset = 0, read;
        //    while (length > 0 && (read = stream.Read(buffer, offset, length)) > 0)
        //    {
        //        length -= read;
        //    }
        //    if (length > 0) throw EoF(null);
        //    return buffer;
        //}
        private static int ReadByteOrThrow(Stream source)
        {
            var val = source.ReadByte();
            if (val < 0) throw EoF(null);
            return val;
        }

        /// <summary>
        ///     Reads the length-prefix of a message from a stream without buffering additional data, allowing a fixed-length
        ///     reader to be created.
        /// </summary>
        public static int ReadLengthPrefix(Stream source, bool expectHeader, PrefixStyle style, out int fieldNumber)
        {
            int bytesRead;
            return ReadLengthPrefix(source, expectHeader, style, out fieldNumber, out bytesRead);
        }

        /// <summary>
        ///     Reads a little-endian encoded integer. An exception is thrown if the data is not all available.
        /// </summary>
        public static int DirectReadLittleEndianInt32(Stream source)
        {
            return ReadByteOrThrow(source)
                   | (ReadByteOrThrow(source) << 8)
                   | (ReadByteOrThrow(source) << 16)
                   | (ReadByteOrThrow(source) << 24);
        }

        /// <summary>
        ///     Reads a big-endian encoded integer. An exception is thrown if the data is not all available.
        /// </summary>
        public static int DirectReadBigEndianInt32(Stream source)
        {
            return (ReadByteOrThrow(source) << 24)
                   | (ReadByteOrThrow(source) << 16)
                   | (ReadByteOrThrow(source) << 8)
                   | ReadByteOrThrow(source);
        }

        /// <summary>
        ///     Reads a varint encoded integer. An exception is thrown if the data is not all available.
        /// </summary>
        public static int DirectReadVarintInt32(Stream source)
        {
            uint val;
            var bytes = TryReadUInt32Variant(source, out val);
            if (bytes <= 0) throw EoF(null);
            return (int) val;
        }

        /// <summary>
        ///     Reads a string (of a given lenth, in bytes) directly from the source into a pre-existing buffer. An exception is
        ///     thrown if the data is not all available.
        /// </summary>
        public static void DirectReadBytes(Stream source, byte[] buffer, int offset, int count)
        {
            int read;
            if (source == null) throw new ArgumentNullException("source");
            while (count > 0 && (read = source.Read(buffer, offset, count)) > 0)
            {
                count -= read;
                offset += read;
            }
            if (count > 0) throw EoF(null);
        }

        /// <summary>
        ///     Reads a given number of bytes directly from the source. An exception is thrown if the data is not all available.
        /// </summary>
        public static byte[] DirectReadBytes(Stream source, int count)
        {
            var buffer = new byte[count];
            DirectReadBytes(source, buffer, 0, count);
            return buffer;
        }

        /// <summary>
        ///     Reads a string (of a given lenth, in bytes) directly from the source. An exception is thrown if the data is not all
        ///     available.
        /// </summary>
        public static string DirectReadString(Stream source, int length)
        {
            var buffer = new byte[length];
            DirectReadBytes(source, buffer, 0, length);
            return System.Text.Encoding.UTF8.GetString(buffer, 0, length);
        }

        /// <summary>
        ///     Reads the length-prefix of a message from a stream without buffering additional data, allowing a fixed-length
        ///     reader to be created.
        /// </summary>
        public static int ReadLengthPrefix(Stream source, bool expectHeader, PrefixStyle style, out int fieldNumber,
            out int bytesRead)
        {
            fieldNumber = 0;
            switch (style)
            {
                case PrefixStyle.None:
                    bytesRead = 0;
                    return int.MaxValue;
                case PrefixStyle.Base128:
                    uint val;
                    int tmpBytesRead;
                    bytesRead = 0;
                    if (expectHeader)
                    {
                        tmpBytesRead = TryReadUInt32Variant(source, out val);
                        bytesRead += tmpBytesRead;
                        if (tmpBytesRead > 0)
                        {
                            if ((val & 7) != (uint) WireType.String)
                            {
                                // got a header, but it isn't a string
                                throw new InvalidOperationException();
                            }
                            fieldNumber = (int) (val >> 3);
                            tmpBytesRead = TryReadUInt32Variant(source, out val);
                            bytesRead += tmpBytesRead;
                            if (bytesRead == 0)
                            {
                                // got a header, but no length
                                throw EoF(null);
                            }
                            return (int) val;
                        }
                        // no header
                        bytesRead = 0;
                        return -1;
                    }
                    // check for a length
                    tmpBytesRead = TryReadUInt32Variant(source, out val);
                    bytesRead += tmpBytesRead;
                    return bytesRead < 0 ? -1 : (int) val;

                case PrefixStyle.Fixed32:
                {
                    var b = source.ReadByte();
                    if (b < 0)
                    {
                        bytesRead = 0;
                        return -1;
                    }
                    bytesRead = 4;
                    return b
                           | (ReadByteOrThrow(source) << 8)
                           | (ReadByteOrThrow(source) << 16)
                           | (ReadByteOrThrow(source) << 24);
                }
                case PrefixStyle.Fixed32BigEndian:
                {
                    var b = source.ReadByte();
                    if (b < 0)
                    {
                        bytesRead = 0;
                        return -1;
                    }
                    bytesRead = 4;
                    return (b << 24)
                           | (ReadByteOrThrow(source) << 16)
                           | (ReadByteOrThrow(source) << 8)
                           | ReadByteOrThrow(source);
                }
                default:
                    throw new ArgumentOutOfRangeException("style");
            }
        }

        /// <returns>The number of bytes consumed; 0 if no data available</returns>
        private static int TryReadUInt32Variant(Stream source, out uint value)
        {
            value = 0;
            var b = source.ReadByte();
            if (b < 0)
            {
                return 0;
            }
            value = (uint) b;
            if ((value & 0x80) == 0)
            {
                return 1;
            }
            value &= 0x7F;

            b = source.ReadByte();
            if (b < 0) throw EoF(null);
            value |= ((uint) b & 0x7F) << 7;
            if ((b & 0x80) == 0) return 2;

            b = source.ReadByte();
            if (b < 0) throw EoF(null);
            value |= ((uint) b & 0x7F) << 14;
            if ((b & 0x80) == 0) return 3;

            b = source.ReadByte();
            if (b < 0) throw EoF(null);
            value |= ((uint) b & 0x7F) << 21;
            if ((b & 0x80) == 0) return 4;

            b = source.ReadByte();
            if (b < 0) throw EoF(null);
            value |= (uint) b << 28; // can only use 4 bits from this chunk
            if ((b & 0xF0) == 0) return 5;

            throw new OverflowException();
        }

        internal static void Seek(Stream source, int count, byte[] buffer)
        {
            if (source.CanSeek)
            {
                source.Seek(count, SeekOrigin.Current);
                count = 0;
            }
            else if (buffer != null)
            {
                int bytesRead;
                while (count > buffer.Length && (bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    count -= bytesRead;
                }
                while (count > 0 && (bytesRead = source.Read(buffer, 0, count)) > 0)
                {
                    count -= bytesRead;
                }
            }
            else // borrow a buffer
            {
                buffer = BufferPool.GetBuffer();
                try
                {
                    int bytesRead;
                    while (count > buffer.Length && (bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        count -= bytesRead;
                    }
                    while (count > 0 && (bytesRead = source.Read(buffer, 0, count)) > 0)
                    {
                        count -= bytesRead;
                    }
                }
                finally
                {
                    BufferPool.ReleaseBufferToPool(ref buffer);
                }
            }
            if (count > 0) throw EoF(null);
        }

        internal static Exception AddErrorData(Exception exception, ProtoReader source)
        {
#if !CF && !FX11 && !PORTABLE
            if (exception != null && source != null && !exception.Data.Contains("protoSource"))
            {
                exception.Data.Add("protoSource", string.Format("tag={0}; wire-type={1}; offset={2}; depth={3}",
                    source.FieldNumber, source._wireType, source.Position, source._depth));
            }
#endif
            return exception;
        }

        private static Exception EoF(ProtoReader source)
        {
            return AddErrorData(new EndOfStreamException(), source);
        }

        /// <summary>
        ///     Copies the current field into the instance as extension data
        /// </summary>
        public void AppendExtensionData(IExtensible instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            var extn = instance.GetExtensionObject(true);
            var commit = false;
            // unusually we *don't* want "using" here; the "finally" does that, with
            // the extension object being responsible for disposal etc
            var dest = extn.BeginAppend();
            try
            {
                //TODO: replace this with stream-based, buffered raw copying
                using (var writer = new ProtoWriter(dest, Model, null))
                {
                    AppendExtensionField(writer);
                    writer.Close();
                }
                commit = true;
            }
            finally
            {
                extn.EndAppend(dest, commit);
            }
        }

        private void AppendExtensionField(ProtoWriter writer)
        {
            //TODO: replace this with stream-based, buffered raw copying
            ProtoWriter.WriteFieldHeader(FieldNumber, _wireType, writer);
            switch (_wireType)
            {
                case WireType.Fixed32:
                    ProtoWriter.WriteInt32(ReadInt32(), writer);
                    return;
                case WireType.Variant:
                case WireType.SignedVariant:
                case WireType.Fixed64:
                    ProtoWriter.WriteInt64(ReadInt64(), writer);
                    return;
                case WireType.String:
                    ProtoWriter.WriteBytes(AppendBytes(null, this), writer);
                    return;
                case WireType.StartGroup:
                    SubItemToken readerToken = StartSubItem(this),
                        writerToken = ProtoWriter.StartSubItem(null, writer);
                    while (ReadFieldHeader() > 0)
                    {
                        AppendExtensionField(writer);
                    }
                    EndSubItem(readerToken, this);
                    ProtoWriter.EndSubItem(writerToken, writer);
                    return;
                case WireType.None: // treat as explicit errorr
                case WireType.EndGroup: // treat as explicit error
                default: // treat as implicit error
                    throw CreateWireTypeException();
            }
        }

        /// <summary>
        ///     Indicates whether the reader still has data remaining in the current sub-item,
        ///     additionally setting the wire-type for the next field if there is more data.
        ///     This is used when decoding packed data.
        /// </summary>
        public static bool HasSubValue(WireType wireType, ProtoReader source)
        {
            if (source == null) throw new ArgumentNullException("source");
            // check for virtual end of stream
            if (source._blockEnd <= source.Position || wireType == WireType.EndGroup)
            {
                return false;
            }
            source._wireType = wireType;
            return true;
        }

        internal int GetTypeKey(ref Type type)
        {
            return Model.GetKey(ref type);
        }

        private readonly NetObjectCache _netCache = new NetObjectCache();

        internal NetObjectCache NetCache
        {
            get { return _netCache; }
        }

        internal Type DeserializeType(string value)
        {
            return TypeModel.DeserializeType(Model, value);
        }

        internal void SetRootObject(object value)
        {
            _netCache.SetKeyedObject(NetObjectCache.Root, value);
            _trapCount--;
        }

        // this is how many outstanding objects do not currently have
        // values for the purposes of reference tracking; we'll default
        // to just trapping the root object
        // note: objects are trapped (the ref and Key mapped) via NoteObject
        private uint _trapCount = 1; // uint is so we can use beq/bne more efficiently than bgt

        /// <summary>
        ///     Utility method, not intended for public use; this helps maintain the root object is complex scenarios
        /// </summary>
        public static void NoteObject(object value, ProtoReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            if (reader._trapCount != 0)
            {
                reader._netCache.RegisterTrappedObject(value);
                reader._trapCount--;
            }
        }

        /// <summary>
        ///     Reads a Type from the stream, using the model's DynamicTypeFormatting if appropriate; supported wire-types: String
        /// </summary>
        public Type ReadType()
        {
            return TypeModel.DeserializeType(Model, ReadString());
        }

        internal void TrapNextObject(int newObjectKey)
        {
            _trapCount++;
            _netCache.SetKeyedObject(newObjectKey, null); // use null as a temp
        }

        internal void CheckFullyConsumed()
        {
            if (_isFixedLength)
            {
                if (_dataRemaining != 0) throw new ProtoException("Incorrect number of bytes consumed");
            }
            else
            {
                if (_available != 0)
                    throw new ProtoException("Unconsumed data left in the buffer; this suggests corrupt input");
            }
        }

        /// <summary>
        ///     Merge two objects using the details from the current reader; this is used to change the type
        ///     of objects when an inheritance relationship is discovered later than usual during deserilazation.
        /// </summary>
        public static object Merge(ProtoReader parent, object from, object to)
        {
            if (parent == null) throw new ArgumentNullException("parent");
            var model = parent.Model;
            var ctx = parent.Context;
            if (model == null)
                throw new InvalidOperationException("Types cannot be merged unless a type-model has been specified");
            using (var ms = new MemoryStream())
            {
                model.Serialize(ms, from, ctx);
                ms.Position = 0;
                return model.Deserialize(ms, to, null);
            }
        }
    }
}