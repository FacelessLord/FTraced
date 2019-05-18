using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GlLib.Utils
{
    public class ThreadSafeList<T> : IList<T>
    {
        protected static readonly object _lock = new object();
        protected List<T> internalList = new List<T>();

        // Other Elements of IList implementation

        public IEnumerator<T> GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        public void Add(T _item)
        {
            lock (_lock)
            {
                internalList.Add(_item);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                internalList.Clear();
            }
        }

        public bool Contains(T _item)
        {
            lock (_lock)
            {
                return internalList.Contains(_item);
            }
        }

        public void CopyTo(T[] _array, int _arrayIndex)
        {
            lock (_lock)
            {
                internalList.CopyTo(_array, _arrayIndex);
            }
        }

        public bool Remove(T _item)
        {
            lock (_lock)
            {
                return internalList.Remove(_item);
            }
        }

        public int Count => internalList.Count;
        public bool IsReadOnly => false;

        public int IndexOf(T _item)
        {
            lock (_lock)
            {
                return internalList.IndexOf(_item);
            }
        }

        public void Insert(int _index, T _item)
        {
            lock (_lock)
            {
                internalList.Insert(_index, _item);
            }
        }

        public void RemoveAt(int _index)
        {
            lock (_lock)
            {
                internalList.RemoveAt(_index);
            }
        }

        public T this[int _index]
        {
            get => internalList[_index];
            set => internalList[_index] = value;
        }

        public List<T> Clone()
        {
            var newList = new List<T>();

            lock (_lock)
            {
                internalList.ForEach(_x => newList.Add(_x));
            }

            return newList;
        }
    }

    public static class LinqExtension
    {
        public static ThreadSafeList<T> ToThreadSafeList<T>(this IEnumerable<T> _items)
        {
            var l = new ThreadSafeList<T>();
            _items.ToList().ForEach(l.Add);
            return l;
        }

        public static ThreadSafeList<T> ThreadSafeWhere<T>(this IEnumerable<T> _items, Func<T, bool> _f)
        {
            var l = new ThreadSafeList<T>();
            _items.Where(_f).ToList().ForEach(l.Add);
            return l;
        }
    }
}