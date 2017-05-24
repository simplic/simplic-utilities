using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplic.CommandShell
{
    /// <summary>
    /// Command description attributes (for the help text)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Create new command description
        /// </summary>
        /// <param name="description">Description text</param>
        public CommandDescriptionAttribute(string description)
        {
            this.Description = description;
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
