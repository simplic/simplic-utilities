using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplic.CommandShell
{
    /// <summary>
    /// Description attribute for parameter
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ParameterDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Create new parameter description
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <param name="isRequired">Defines wether the parameter is required</param>
        /// <param name="description">Descriptiontext of the parameter</param>
        public ParameterDescriptionAttribute(string parameterName, bool isRequired, string description)
        {
            this.ParameterName = parameterName;
            this.IsRequired = isRequired;
            this.Description = description;
        }

        /// <summary>
        /// Name of the parameter
        /// </summary>
        public string ParameterName
        {
            get;
            set;
        }

        /// <summary>
        /// Is required
        /// </summary>
        public bool IsRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Description text
        /// </summary>
        public string Description
        {
            get;
            set;
        }
    }
}
