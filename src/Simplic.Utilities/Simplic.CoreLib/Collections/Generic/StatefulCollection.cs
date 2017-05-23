using System.Collections;
using System.Collections.Generic;

namespace Simplic.Collections.Generic
{
    /// <summary>
    /// Collection which handles internal a list of : new, not changed and deleted items.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    public class StatefulCollection<T> : ICollection<T>
    {
        #region Fields

        private IList<T> items;
        private IList<T> newItems;
        private IList<T> removedItems;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initialize new stateful iterable
        /// </summary>
        /// <param name="initialList">Initial list of items</param>
        public StatefulCollection(IEnumerable<T> initialList)
        {
            items = new List<T>(initialList);
            newItems = new List<T>();
            removedItems = new List<T>();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Add a new item.
        /// If the item already exists - ignore it
        /// If the item was removed, put it in the item list again
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(T item)
        {
            // If the item is not added to any list yet
            if (removedItems.Contains(item))
            {
                // Remove item from the list of removed items
                items.Add(item);
                removedItems.Remove(item);
            }
            else if (!items.Contains(item) && !newItems.Contains(item))
            {
                newItems.Add(item);
            }
        }

        /// <summary>
        /// Add a range of items. Each item will be handled as described for <see cref="Add(T)"/>
        /// </summary>
        /// <param name="items">List of items to handle</param>
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Clear the howle collection
        /// </summary>
        public void Clear()
        {
            items.Clear();
            newItems.Clear();
            removedItems.Clear();
        }

        /// <summary>
        /// Check whether an items exists in the items list or the new items list
        /// </summary>
        /// <param name="item">Item to proof</param>
        /// <returns>True if the item exists</returns>
        public bool Contains(T item)
        {
            if (items.Contains(item) || newItems.Contains(item))
                return true;

            return false;
        }

        /// <summary>
        /// Copy current items into an array
        /// </summary>
        /// <param name="array">Array to copy items to</param>
        /// <param name="arrayIndex">Start index</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            int i = arrayIndex;
            foreach (var item in this)
            {
                array[i] = item;
                i++;
            }
        }

        /// <summary>
        /// Get enumerator for the list of items and new items
        /// </summary>
        /// <returns>Returns an instance of an enumerator</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var _tmpCompleteList = new List<T>(items);
            _tmpCompleteList.AddRange(newItems);

            return _tmpCompleteList.GetEnumerator();
        }

        /// <summary>
        /// Remove an item and put it into the removed items list
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if the item was removed, false if not</returns>
        public bool Remove(T item)
        {
            if (newItems.Contains(item))
            {
                // Remove from new items and do nothing else
                newItems.Remove(item);

                return true;
            }
            else if (items.Contains(item))
            {
                // Remove from existing items and add to removed items list

                items.Remove(item);
                removedItems.Add(item);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Get the enumerator to iterate over the current collection content without removed items
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in newItems)
            {
                yield return item;
            }
            foreach (var item in items)
            {
                yield return newItems;
            }
        }

        /// <summary>
        /// Enumerator to iteratore over all removed items
        /// </summary>
        /// <returns>Enumerable of removed items</returns>
        public IEnumerable<T> GetRemovedItems()
        {
            return removedItems;
        }

        /// <summary>
        /// Enumerator to iteratore over all new items
        /// </summary>
        /// <returns>Enumerable of new items</returns>
        public IEnumerable<T> GetNewItems()
        {
            return newItems;
        }

        /// <summary>
        /// Enumerator to all comitted items
        /// </summary>
        /// <returns>Enumerable of comitted items</returns>
        public IEnumerable<T> GetItems()
        {
            return items;
        }

        /// <summary>
        /// Mark all items in the items list as removed and put them in the removed list
        /// </summary>
        public void MarkAllAsRemoved()
        {
            foreach (var item in items)
            {
                removedItems.Add(item);
            }
            items.Clear();
            newItems.Clear();
        }

        /// <summary>
        /// Cleares the to remove list and add all items from new item to items
        /// </summary>
        public void Commit()
        {
            foreach (var newItem in newItems)
            {
                items.Add(newItem);
            }

            newItems.Clear();
            removedItems.Clear();
        }

        #endregion Public Methods

        #region Public Member

        /// <summary>
        /// Gets the amount of items (new and current items)
        /// </summary>
        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        /// <summary>
        /// Gets the amount of removed items
        /// </summary>
        public int CountRemovedItems
        {
            get
            {
                return removedItems.Count;
            }
        }

        /// <summary>
        /// Get the amount of new items
        /// </summary>
        public int CountNewItems
        {
            get
            {
                return newItems.Count;
            }
        }

        /// <summary>
        /// Gets whether the current collection is readonly or not
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return items.IsReadOnly;
            }
        }

        #endregion Public Member
    }
}