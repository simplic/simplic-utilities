using System.Collections.Generic;

namespace Simplic.Collections.Generic
{
    public class ConcurrentList<T> : IList<T>
    {
        private List<T> _list;

        public ConcurrentList()
        {
            _list = new List<T>();
        }

        public ConcurrentList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        public ConcurrentList(IEnumerable<T> enumerator)
        {
            _list = new List<T>(enumerator);
        }

        public int IndexOf(T item)
        {
            lock (_list)
            {
                return _list.IndexOf(item);
            }
        }

        public void Insert(int index, T item)
        {
            lock (_list)
            {
                _list.Insert(index, item);
            }
        }

        public void RemoveAt(int index)
        {
            lock (_list)
            {
                _list.RemoveAt(index);
            }
        }

        public T this[int index]
        {
            get
            {
                lock (_list)
                {
                    return _list[index];
                }
            }
            set
            {
                lock (_list)
                {
                    _list[index] = value;
                }
            }
        }

        public void Add(T item)
        {
            lock (_list)
            {
                _list.Add(item);
            }
        }

        public void Clear()
        {
            lock (_list)
            {
                _list.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (_list)
            {
                return _list.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_list)
            {
                _list.CopyTo(array, arrayIndex);
            }
        }

        public int Count
        {
            get
            {
                lock (_list)
                {
                    return _list.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                //lock (_list)
                {
                    return false;
                }
            }
        }

        public bool Remove(T item)
        {
            lock (_list)
            {
                return _list.Remove(item);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            lock (_list)
            {
                return _list.GetEnumerator();
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            lock (_list)
            {
                return _list.GetEnumerator();
            }
        }
    }
}