using System;
using System.Collections.Generic;

namespace Simplic.Log.Console
{
    /// <summary>
    /// Log provider to write log message to the windows console
    /// </summary>
    public class ConsoleLogOutput : ILogOutput
    {
        /// <summary>
        /// Write a message to the windows console
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="logArea">Area where the message should be printed</param>
        /// <param name="ex">Passed exception</param>
        /// <param name="logType">Type of the log entry</param>
        /// <param name="parameter">Passed parameter to the log provider</param>
        public void Write(string message, string logArea, Exception ex, LogType logType, IDictionary<string, object> parameter)
        {
            if (logType == LogType.Debug)
            {
                System.Console.ForegroundColor = ConsoleColor.White;
            }
            else if (logType == LogType.Info)
            {
                System.Console.ForegroundColor = ConsoleColor.Gray;
            }
            else if (logType == LogType.Warning)
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (logType == LogType.Error)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
            }

            System.Console.WriteLine(message);

            System.Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Initialize Console log
        /// </summary>
        /// <param name="parameter">Parameter - no needed</param>
        /// <returns>True</returns>
        public bool Initialize(params object[] parameter)
        {
            return true;
        }

        /// <summary>
        /// Activate the current log provider
        /// </summary>
        public void Activate()
        {
        }

        /// <summary>
        /// Deactive the current log provider
        /// </summary>
        public void Deactivate()
        {
        }

        /// <summary>
        /// Remove the current log provider
        /// </summary>
        public void Remove()
        {
        }

        /// <summary>
        /// Showdown the current log provider
        /// </summary>
        /// <returns></returns>
        public bool Shutdown()
        {
            return true;
        }

        /// <summary>
        /// Returns true, if the provder is installerd
        /// </summary>
        public bool IsInstalled
        {
            get { return true; }
        }

        /// <summary>
        /// Name of the provlder
        /// </summary>
        public string ProviderName
        {
            get { return "ConsoleLogOutput"; }
        }
    }
}