using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MyProto.Meta;

namespace MyProto
{
    internal sealed class NetObjectCache
    {
        internal const int Root = 0;
        private MutableList _underlyingList;

        private MutableList List
        {
            get
            {
                if (_underlyingList == null) _underlyingList = new MutableList();
                return _underlyingList;
            }
        }


        internal object GetKeyedObject(int key)
        {
            if (key-- == Root)
            {
                if (_rootObject == null) throw new ProtoException("No root object assigned");
                return _rootObject;
            }
            BasicList list = List;

            if (key < 0 || key >= list.Count)
            {
                Helpers.DebugWriteLine("Missing Key: " + key);
                throw new ProtoException("Internal error; a missing Key occurred");
            }

            var tmp = list[key];
            if (tmp == null) throw new ProtoException("A deferred Key does not have a value yet");
            return tmp;
        }

        internal void SetKeyedObject(int key, object value)
        {
            if (key-- == Root)
            {
                if (value == null) throw new ArgumentNullException("value");
                if (_rootObject != null && (_rootObject != value))
                    throw new ProtoException("The root object cannot be reassigned");
                _rootObject = value;
            }
            else
            {
                var list = List;
                if (key < list.Count)
                {
                    var oldVal = list[key];
                    if (oldVal == null)
                    {
                        list[key] = value;
                    }
                    else if (!ReferenceEquals(oldVal, value))
                    {
                        throw new ProtoException("Reference-tracked objects cannot change reference");
                    } // otherwise was the same; nothing to do
                }
                else if (key != list.Add(value))
                {
                    throw new ProtoException("Internal error; a Key mismatch occurred");
                }
            }
        }

        private object _rootObject;

        internal int AddObjectKey(object value, out bool existing)
        {
            if (value == null) throw new ArgumentNullException("value");

            if (value == _rootObject) // (object) here is no-op, but should be
            {
                // preserved even if this was typed - needs ref-check
                existing = true;
                return Root;
            }

            var s = value as string;
            BasicList list = List;
            int index;

#if NO_GENERICS
            
            if(s == null)
            {
                if (objectKeys == null)
                {
                    objectKeys = new ReferenceHashtable();
                    index = -1;
                }
                else
                {
                    object tmp = objectKeys[value];
                    index = tmp == null ? -1 : (int) tmp;
                }
            }
            else
            {
                if (stringKeys == null)
                {
                    stringKeys = new Hashtable();
                    index = -1;
                }
                else
                {
                    object tmp = stringKeys[s];
                    index = tmp == null ? -1 : (int) tmp;
                }
            }
#else

            if (s == null)
            {
#if CF || PORTABLE // CF has very limited proper object ref-tracking; so instead, we'll search it the hard way
                index = list.IndexOfReference(value);
#else
                if (_objectKeys == null)
                {
                    _objectKeys = new Dictionary<object, int>(ReferenceComparer.Default);
                    index = -1;
                }
                else
                {
                    if (!_objectKeys.TryGetValue(value, out index)) index = -1;
                }
#endif
            }
            else
            {
                if (_stringKeys == null)
                {
                    _stringKeys = new Dictionary<string, int>();
                    index = -1;
                }
                else
                {
                    if (!_stringKeys.TryGetValue(s, out index)) index = -1;
                }
            }
#endif

            if (!(existing = index >= 0))
            {
                index = list.Add(value);

                if (s == null)
                {
#if !CF && !PORTABLE // CF can't handle the object keys very well
                    _objectKeys.Add(value, index);
#endif
                }
                else
                {
                    _stringKeys.Add(s, index);
                }
            }
            return index + 1;
        }

        private int _trapStartIndex; // defaults to 0 - optimization for RegisterTrappedObject
        // to make it faster at seeking to find deferred-objects

        internal void RegisterTrappedObject(object value)
        {
            if (_rootObject == null)
            {
                _rootObject = value;
            }
            else
            {
                if (_underlyingList != null)
                {
                    for (var i = _trapStartIndex; i < _underlyingList.Count; i++)
                    {
                        _trapStartIndex = i + 1; // things never *become* null; whether or
                        // not the next item is null, it will never
                        // need to be checked again

                        if (_underlyingList[i] == null)
                        {
                            _underlyingList[i] = value;
                            break;
                        }
                    }
                }
            }
        }

#if NO_GENERICS
        private ReferenceHashtable objectKeys;
        private System.Collections.Hashtable stringKeys;
        private class ReferenceHashtable : System.Collections.Hashtable
        {
            protected override int GetHash(object key)
            {
                return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(key);
            }
            protected override bool KeyEquals(object item, object key)
            {
                return item == key;
            }
        }   
#else

        private Dictionary<string, int> _stringKeys;

#if !CF && !PORTABLE
        // CF lacks the ability to get a robust reference-based hash-code, so we'll do it the harder way instead
        private Dictionary<object, int> _objectKeys;

        private sealed class ReferenceComparer : IEqualityComparer<object>
        {
            public static readonly ReferenceComparer Default = new ReferenceComparer();

            private ReferenceComparer()
            {
            }

            bool IEqualityComparer<object>.Equals(object x, object y)
            {
                return x == y; // ref equality
            }

            int IEqualityComparer<object>.GetHashCode(object obj)
            {
                return RuntimeHelpers.GetHashCode(obj);
            }
        }
#endif

#endif
    }
}