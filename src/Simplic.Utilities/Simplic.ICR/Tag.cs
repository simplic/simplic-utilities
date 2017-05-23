using System;
using System.Collections.Generic;

namespace Simplic.ICR
{
    /// <summary>
    /// Represents a tag, which can be selected over ICRFilter and ICR selectors.
    /// A tag contains the information about a tag text and it's unique id (guid)
    /// </summary>
    public class Tag
    {
        #region Private Member

        private string text;
        private Guid id;
        private IList<Filter> filter;

        #endregion Private Member

        #region Constructor

        /// <summary>
        /// Create tag which can be added to an ICRObject
        /// </summary>
        /// <param name="text">Text of the tag, like customer, or bill</param>
        /// <param name="id">Id of the tag</param>
        public Tag(string text, Guid id)
        {
            this.text = text;
            this.id = id;
            filter = new List<Filter>();
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Add a filter to the tag
        /// </summary>
        /// <param name="newFilter">New filter instance</param>
        public void AddFilter(Filter newFilter)
        {
            if (newFilter == null)
            {
                throw new ArgumentNullException("newFilter");
            }

            this.filter.Add(newFilter);
        }

        #endregion Public Methods

        #region Public Member

        /// <summary>
        /// List of filters, which are required for the tag
        /// </summary>
        public IList<Filter> Filter
        {
            get { return filter; }
        }

        /// <summary>
        /// Text of the tag
        /// </summary>
        public string Text
        {
            get { return text; }
        }

        /// <summary>
        /// Id of the tag
        /// </summary>
        public Guid Id
        {
            get { return id; }
        }

        #endregion Public Member
    }
}