using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplic.CommandShell
{
    /// <summary>
    /// Contains a list of parameter
    /// </summary>
    public class CommandShellParameterCollection
    {
        #region Private Member
        private IDictionary<string, CommandShellParameter> parameter;
        #endregion

        #region Constructor
        /// <summary>
        /// Create parameter collection
        /// </summary>
        public CommandShellParameterCollection()
        {
            parameter = new Dictionary<string, CommandShellParameter>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Get a parameter from the collection
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <returns>Instance of a shellparameter if found, else null</returns>
        public CommandShellParameter GetParameter(string parameterName)
        {
            CommandShellParameter returnValue = null;

            if (parameter.ContainsKey(parameterName.ToLower()))
            { 
                returnValue = parameter[parameterName.ToLower()];
            }

            return returnValue;
        }

        /// <summary>
        /// Get a parameter value as a string.
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <returns>Returns empty string if the parameter does not exists</returns>
        public string GetParameterValueAsString(string parameterName)
        {
            StringBuilder rt = new StringBuilder();

            if (this.ContainsParameter(parameterName.ToLower()))
            {
                if (this.GetParameter(parameterName.ToLower()).Value != null)
                {
                    foreach (string val in this.GetParameter(parameterName).Value)
                    {
                        rt.Append(val);
                    }
                }
            }

            return rt.ToString();
        }

        /// <summary>
        /// Proof wether a parameter exists
        /// </summary>
        /// <param name="parameterName">Name of the parameter</param>
        /// <returns>True if the parameter exists</returns>
        public bool ContainsParameter(string parameterName)
        {
            return parameter.ContainsKey(parameterName.ToString());
        }

        /// <summary>
        /// Add shell-parameter
        /// </summary>
        /// <param name="shellParameter">Instance of the parameter</param>
        internal void AddParameter(CommandShellParameter shellParameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("Parameter");
            }

            if (parameter.ContainsKey(shellParameter.Name.ToLower()))
            {
                parameter[shellParameter.Name.ToLower()] = shellParameter;
            }
            else
            {
                parameter.Add(shellParameter.Name.ToLower(), shellParameter);
            }
        }
        #endregion

        #region Public Member
        /// <summary>
        /// List with all parameter
        /// </summary>
        public IDictionary<string, CommandShellParameter> Parameter
        {
            get { return parameter; }
        }
        #endregion
    }
}
