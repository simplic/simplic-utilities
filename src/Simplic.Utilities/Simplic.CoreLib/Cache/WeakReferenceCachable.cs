namespace Simplic.Cache
{
    /// <summary>
    /// Stores an object as weak reference in the cache
    /// </summary>
    /// <typeparam name="T">Type of the cache object</typeparam>
    public class WeakReferenceCachable<T> : TypedWeakReference, ICacheable where T : ICacheable
    {
        /// <summary>
        /// Initialize new WearckReference cache object
        /// </summary>
        /// <param name="target">Object to cache</param>
        public WeakReferenceCachable(T target)
            : base(target)
        {
            Key = target.Key;
        }

        /// <summary>
        /// Remove weak reference from cache
        /// </summary>
        public void OnRemove()
        {
            if (IsAlive)
            {
                Object.OnRemove();
            }
        }

        /// <summary>
        /// Get the key of the nested weak reference cachable
        /// </summary>
        public string Key
        {
            get;
            private set;
        }

        /// <summary>
        /// Object that is stored in the cache
        /// </summary>
        public T Object
        {
            get
            {
                return (T)Target;
            }
        }
    }
}