using System;
using System.Collections.Generic;
using System.Linq;

namespace Simplic.Collections.Generic
{
    public class ThreadsafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _dict;

        public ThreadsafeDictionary()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        public void Add(TKey key, TValue value)
        {
            lock (_dict)
            {
                _dict.Add(key, value);
            }
        }

        public bool ContainsKey(TKey key)
        {
            lock (_dict)
            {
                return _dict.ContainsKey(key);
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                lock (_dict)
                {
                    return _dict.Keys;
                }
            }
        }

        public bool Remove(TKey key)
        {
            lock (_dict)
            {
                return _dict.Remove(key);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_dict)
            {
                return _dict.TryGetValue(key, out value);
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (_dict)
                {
                    return _dict.Values;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_dict)
                {
                    return _dict[key];
                }
            }
            set
            {
                lock (_dict)
                {
                    _dict[key] = value;
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            lock (_dict)
            {
                _dict.Add(item.Key, item.Value);
            }
        }

        public void Clear()
        {
            lock (_dict)
            {
                _dict.Clear();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (_dict)
            {
                return _dict.Contains(item);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                lock (_dict)
                {
                    return _dict.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_dict)
            {
                return _dict.Remove(item.Key);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (_dict)
            {
                return _dict.GetEnumerator();
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            lock (_dict)
            {
                return _dict.GetEnumerator();
            }
        }
    }
}