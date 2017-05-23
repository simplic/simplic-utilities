using System;
using System.Collections.Generic;

namespace Simplic.Log
{
    /// <summary>
    /// Argument for the log write trigger
    /// </summary>
    public class LogTriggerEventArgs : EventArgs
    {
        public LogType OldLogType
        {
            get;
            set;
        }

        public LogType NewLogType
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public IDictionary<string, object> ParameterList
        {
            get;
            set;
        }

        /// <summary>
        /// Proof, whether a parameter exists
        /// </summary>
        /// <param name="parameterName">Name of the parameter to proof</param>
        /// <returns>True if the parameter exists</returns>
        public bool ParameterExists(string parameterName)
        {
            if (ParameterList == null || ParameterList.Count == 0)
            {
                return false;
            }
            else
            {
                return ParameterList.ContainsKey(parameterName);
            }
        }
    }
}