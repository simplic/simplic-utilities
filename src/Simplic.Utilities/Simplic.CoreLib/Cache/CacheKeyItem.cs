using System;

namespace Simplic.Cache
{
    /// <summary>
    /// Simple cache item
    /// </summary>
    internal class CacheKeyItem
    {
        /// <summary>
        /// Create new Cache item
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="type">Type</param>
        public CacheKeyItem(Type type, string name)
        {
            KeyName = CacheManager.PrepareKey(name);
            KeyType = type;
        }

        /// <summary>
        /// Name of the key
        /// </summary>
        public string KeyName
        {
            get;
            private set;
        }

        /// <summary>
        /// Type of the key
        /// </summary>
        public Type KeyType
        {
            get;
            set;
        }

        /// <summary>
        /// override object.Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            bool equal = KeyName == (obj as CacheKeyItem).KeyName && KeyType == (obj as CacheKeyItem).KeyType;
            return equal;
        }

        /// <summary>
        ///  override object.GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int nameHash = KeyName.GetHashCode();
                int typeHash = KeyType.GetHashCode();

                return nameHash + typeHash;
            }
        }

        public override string ToString()
        {
            return (KeyType.ToString() ?? "NULL") + "_" + (KeyName ?? "NULL");
        }
    }
}