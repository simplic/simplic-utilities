using System;
using System.Collections.Generic;

namespace Simplic.ICR
{
    /// <summary>
    /// Struct which contains all tag information, which are assigned to an ICRObject
    /// </summary>
    public struct AssignedTag
    {
        /// <summary>
        /// Text of the tag
        /// </summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary>
        /// Matched string
        /// </summary>
        public IList<string> Matches
        {
            get;
            set;
        }

        /// <summary>
        /// Unique ID of the tag
        /// </summary>
        public Guid Id
        {
            get;
            set;
        }
    }
}