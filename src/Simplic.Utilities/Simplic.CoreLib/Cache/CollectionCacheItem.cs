using System;

namespace Simplic.Cache
{
    /// <summary>
    /// Gets an implementation to save a collection of
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class GenericCacheItem<T> : ICacheable
    {
        private T data;

        /// <summary>
        /// Initialize a new generic cache item
        /// </summary>
        /// <param name="data"></param>
        public GenericCacheItem(T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            this.data = data;
        }

        /// <summary>
        /// Gets or sets the cache item data
        /// </summary>
        public T Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        /// <summary>
        /// Gets the cache item key (type of data)
        /// </summary>
        public string Key
        {
            get
            {
                return $"{typeof(T)}";
            }
        }

        /// <summary>
        /// Remove the data and dispose if possible
        /// </summary>
        public void OnRemove()
        {
            if (data is IDisposable)
                (data as IDisposable).Dispose();
        }
    }
}