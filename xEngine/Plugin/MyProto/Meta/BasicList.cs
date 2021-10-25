using System;
using System.Collections;

namespace MyProto.Meta
{
    internal sealed class MutableList : BasicList
    {
        /*  Like BasicList, but allows existing values to be changed
         */

        public new object this[int index]
        {
            get { return Head[index]; }
            set { Head[index] = value; }
        }

        public void RemoveLast()
        {
            Head.RemoveLastWithMutate();
        }
    }

    internal class BasicList : IEnumerable
    {
        /* Requirements:
         *   - Fast access by index
         *   - Immutable in the tail, so a node can be read (iterated) without locking
         *   - Lock-free tail handling must match the memory mode; struct for Node
         *     wouldn't work as "read" would not be atomic
         *   - Only operation required is append, but this shouldn't go out of its
         *     way to be inefficient
         *   - Assume that the caller is handling thread-safety (to co-ordinate with
         *     other code); no attempt to be thread-safe
         *   - Assume that the data is private; internal data structure is allowed to
         *     be mutable (i.e. array is fine as long as we don't screw it up)
         */
        private static readonly Node Nil = new Node(null, 0);
        protected Node Head = Nil;

        public object this[int index]
        {
            get { return Head[index]; }
        }

        public int Count
        {
            get { return Head.Length; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new NodeEnumerator(Head);
        }

        public void CopyTo(Array array, int offset)
        {
            Head.CopyTo(array, offset);
        }

        public int Add(object value)
        {
            return (Head = Head.Append(value)).Length - 1;
        }

        //public object TryGet(int index)
        //{
        //    return head.TryGet(index);
        //}
        public void Trim()
        {
            Head = Head.Trim();
        }

        public NodeEnumerator GetEnumerator()
        {
            return new NodeEnumerator(Head);
        }

        internal int IndexOf(IPredicate predicate)
        {
            return Head.IndexOf(predicate);
        }

        internal int IndexOfReference(object instance)
        {
            return Head.IndexOfReference(instance);
        }

        internal bool Contains(object value)
        {
            foreach (var obj in this)
            {
                if (Equals(obj, value)) return true;
            }
            return false;
        }

        internal static BasicList GetContiguousGroups(int[] keys, object[] values)
        {
            if (keys == null) throw new ArgumentNullException("keys");
            if (values == null) throw new ArgumentNullException("values");
            if (values.Length < keys.Length)
                throw new ArgumentException("Not all keys are covered by values", "values");
            var outer = new BasicList();
            Group group = null;
            for (var i = 0; i < keys.Length; i++)
            {
                if (i == 0 || keys[i] != keys[i - 1])
                {
                    group = null;
                }
                if (group == null)
                {
                    group = new Group(keys[i]);
                    outer.Add(group);
                }
                group.Items.Add(values[i]);
            }
            return outer;
        }

        internal sealed class Group
        {
            public readonly int First;
            public readonly BasicList Items;

            public Group(int first)
            {
                First = first;
                Items = new BasicList();
            }
        }

        internal interface IPredicate
        {
            bool IsMatch(object obj);
        }

        internal sealed class Node
        {
            //public object TryGet(int index)
            //{
            //    return (index >= 0 && index < length) ? data[index] : null;
            //}
            private readonly object[] _data;

            internal Node(object[] data, int length)
            {
                Helpers.DebugAssert((data == null && length == 0) ||
                                    (data != null && length > 0 && length <= data.Length));
                _data = data;

                Length = length;
            }

            public object this[int index]
            {
                get
                {
                    if (index >= 0 && index < Length)
                    {
                        return _data[index];
                    }
                    throw new ArgumentOutOfRangeException("index");
                }
                set
                {
                    if (index >= 0 && index < Length)
                    {
                        _data[index] = value;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }
                }
            }

            public int Length { get; private set; }

            public void RemoveLastWithMutate()
            {
                if (Length == 0) throw new InvalidOperationException();
                Length -= 1;
            }

            public Node Append(object value)
            {
                object[] newData;
                var newLength = Length + 1;
                if (_data == null)
                {
                    newData = new object[10];
                }
                else if (Length == _data.Length)
                {
                    newData = new object[_data.Length*2];
                    Array.Copy(_data, newData, Length);
                }
                else
                {
                    newData = _data;
                }
                newData[Length] = value;
                return new Node(newData, newLength);
            }

            public Node Trim()
            {
                if (Length == 0 || Length == _data.Length) return this;
                var newData = new object[Length];
                Array.Copy(_data, newData, Length);
                return new Node(newData, Length);
            }

            internal int IndexOfReference(object instance)
            {
                for (var i = 0; i < Length; i++)
                {
                    if (instance == _data[i]) return i;
                } // ^^^ (object) above should be preserved, even if this was typed; needs
                // to be a reference check
                return -1;
            }

            internal int IndexOf(IPredicate predicate)
            {
                for (var i = 0; i < Length; i++)
                {
                    if (predicate.IsMatch(_data[i])) return i;
                }
                return -1;
            }

            internal void CopyTo(Array array, int offset)
            {
                if (Length > 0)
                {
                    Array.Copy(_data, 0, array, offset, Length);
                }
            }
        }

        public struct NodeEnumerator : IEnumerator
        {
            private readonly Node _node;
            private int _position;

            internal NodeEnumerator(Node node)
            {
                _position = -1;
                _node = node;
            }

            void IEnumerator.Reset()
            {
                _position = -1;
            }

            public object Current
            {
                get { return _node[_position]; }
            }

            public bool MoveNext()
            {
                var len = _node.Length;
                return (_position <= len) && (++_position < len);
            }
        }
    }
}