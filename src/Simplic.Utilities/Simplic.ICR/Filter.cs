using System;
using System.Collections.Generic;

namespace Simplic.ICR
{
    /// <summary>
    /// Filte which contains all selector for a tag. If all selector return true, the tag will be assigned
    /// </summary>
    public class Filter
    {
        #region Private Member

        private IList<ISelector> selectorList;

        #endregion Private Member

        #region Constructor

        /// <summary>
        /// Create a new filter
        /// </summary>
        public Filter()
        {
            selectorList = new List<ISelector>();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Add a new selector to the filter
        /// </summary>
        /// <param name="selector">Instance of an ISelector</param>
        public void AddSelector(ISelector selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            selectorList.Add(selector);
        }

        #endregion Public Methods

        #region Public Member

        /// <summary>
        /// List of all selector
        /// </summary>
        public IList<ISelector> Selector
        {
            get { return selectorList; }
        }

        #endregion Public Member
    }
}