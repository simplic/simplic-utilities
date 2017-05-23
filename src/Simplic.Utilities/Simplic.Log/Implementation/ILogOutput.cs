using Simplic.SAAP;
using System;
using System.Collections.Generic;

namespace Simplic.Log
{
    /// <summary>
    /// LogOutput interface, must be implemented in all log provider
    /// </summary>
    public interface ILogOutput : IProvider
    {
        /// <summary>
        /// Write message to implemented output
        /// </summary>
        /// <param name="message">Formatted message content</param>
        /// <param name="ex">Exception</param>
        /// <param name="logType">Type of the log</param>
        /// <param name="parameter">List with all parameter</param>
        void Write(string message, string logArea, Exception ex, LogType logType, IDictionary<string, object> parameter);
    }
}