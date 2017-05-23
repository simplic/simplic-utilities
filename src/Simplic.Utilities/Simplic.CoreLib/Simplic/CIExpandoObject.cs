using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Simplic
{
    /// <summary>
    /// Expandoobject with case insensitive properties
    /// </summary>
    public class CIExpandoObject : DynamicObject
    {
        private Dictionary<string, object> properties;

        #region ctor

        /// <summary>
        /// Constructor for case insensitve ExpandoObject
        /// </summary>
        public CIExpandoObject()
        {
            properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Constructor for case insensitve ExpandoObject
        /// </summary>
        /// <param name="properties">Initiali properties</param>
        public CIExpandoObject(IDictionary<string, object> properties)
        {
            this.properties = new Dictionary<string, object>(properties, StringComparer.OrdinalIgnoreCase);
        }

        #endregion ctor

        #region getter setter

        /// <summary>
        /// returns the internal dictionary
        /// </summary>
        public Dictionary<string, object> Dictionary
        {
            get
            {
                return properties;
            }
        }

        #endregion getter setter

        #region methods

        /// <summary>
        /// try to set a new member
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Dictionary[binder.Name] = value;
            return true;
        }

        /// <summary>
        /// try to get a new member
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return Dictionary.TryGetValue(binder.Name, out result);
        }

        #region [static]

        /// <summary>
        /// allows implicit cast to the internal dictionary property
        /// </summary>
        /// <param name="ciExpando"></param>
        /// <returns></returns>
        public static implicit operator Dictionary<string, object>(CIExpandoObject ciExpando)
        {
            return ciExpando.Dictionary;
        }

        #endregion [static]

        #endregion methods
    }
}