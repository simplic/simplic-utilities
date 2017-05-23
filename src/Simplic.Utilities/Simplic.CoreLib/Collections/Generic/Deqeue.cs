using System.Collections.Generic;
using System.Linq;

namespace Simplic.Collections.Generic
{
    /// <summary>
    /// Custom Deqeue class
    /// </summary>
    /// <typeparam name="T">Generic type of the collection</typeparam>
    public class Dequeue<T> : IEnumerable<T>
    {
        #region Private Member

        private IList<T> list;
        private int count;

        #endregion Private Member

        #region Constructor

        /// <summary>
        /// Create dequeue
        /// </summary>
        public Dequeue()
        {
            list = new List<T>();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Push an element to the end of the queue
        /// </summary>
        /// <param name="Item"></param>
        public void PushBack(T Item)
        {
            list.Add(Item);
            count++;
        }

        /// <summary>
        /// Push an element to the front of the queue
        /// </summary>
        /// <param name="Item"></param>
        public void PushFront(T Item)
        {
            list.Insert(0, Item);
            count++;
        }

        /// <summary>
        /// return and remove the last element from the queue
        /// </summary>
        /// <returns></returns>
        public T PopLast()
        {
            T returnItem = list.Last();
            list.RemoveAt(count == 0 ? 0 : count - 1);
            count--;
            return returnItem;
        }

        /// <summary>
        /// Return and remove the first element of the qeue
        /// </summary>
        /// <returns></returns>
        public T PopFirst()
        {
            T returnItem = list.First();
            list.RemoveAt(0);
            count--;
            return returnItem;
        }

        /// <summary>
        /// Return the last element of the queue
        /// </summary>
        /// <returns></returns>
        public T PeekLast()
        {
            T returnItem = list.Last();

            return returnItem;
        }

        /// <summary>
        /// Return the first element of the queue
        /// </summary>
        /// <returns></returns>
        public T PeekFirst()
        {
            T returnItem = list.First();

            return returnItem;
        }

        /// <summary>
        /// Get item from a specific index
        /// </summary>
        /// <param name="Index">Index</param>
        /// <returns>Item</returns>
        public T Get(int Index)
        {
            return list[Index];
        }

        /// <summary>
        /// Return the enumator of the intern list
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        /// <summary>
        /// Return the enumator of the intern list
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion Public Methods

        #region Public Member

        /// <summary>
        /// Count of the Items
        /// </summary>
        public int Count
        {
            get { return count; }
        }

        public T this[int i]
        {
            get
            {
                return Get(i);
            }
        }

        #endregion Public Member
    }
}