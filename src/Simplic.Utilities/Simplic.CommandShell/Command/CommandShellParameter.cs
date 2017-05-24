using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplic.CommandShell
{
    /// <summary>
    /// Shell parameter
    /// </summary>
    public class CommandShellParameter
    {
        #region Constructor
        /// <summary>
        /// Create a parameter
        /// </summary>
        /// <param name="name">Name of the parameter</param>
        /// <param name="value">Value of the parameter</param>
        public CommandShellParameter(string name, string[] value)
        {
            this.Name = name;
            this.Value = value;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Equals, compare the name of two ShellParameter caseinsensitive
        /// </summary>
        /// <param name="obj">Instance of the object to compare</param>
        /// <returns>True if the objects are equal</returns>
        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }
            else if ((obj is CommandShellParameter) == false)
            {
                return false;
            }
            else
            {
                return (obj as CommandShellParameter).Name.ToLower() == Name.ToLower();
            }
        }

        /// <summary>
        /// Hash-Code of the Parameter-Name
        /// </summary>
        /// <returns>Hashcode</returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Name of the parameter
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Value of the parameter
        /// </summary>
        public string[] Value
        {
            get;
            set;
        }
        #endregion
    }
}
