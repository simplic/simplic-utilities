using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplic.Log.Implementation
{
    /// <summary>
    /// Console Output
    /// </summary>
    public class ConsoleOutput : ILogOutput
    {
        /// <summary>
        /// Write the message to the console
        /// </summary>
        /// <param name="message">Content</param>
        /// <param name="ex">Exception</param>
        /// <param name="logType">Log-Type</param>
        /// <param name="parameter">List with parameter</param>
        public void Write(string message, Exception ex, LogType logType, IDictionary<string, object> parameter)
        {
            if (logType == LogType.Debug)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if (logType == LogType.Info)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else if (logType == LogType.Warning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (logType == LogType.Debug)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }

            Console.WriteLine(message);

            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Init - not used
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>True</returns>
        public bool Initialize(params object[] parameter)
        {
            return true;
        }

        /// <summary>
        /// Not used
        /// </summary>
        public void Activate()
        {

        }     
        
        /// <summary>
        /// Not used
        /// </summary>
        public void Deactivate()
        {

        }

        /// <summary>
        /// Activate - not used
        /// </summary>
        public void Remove()
        {

        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <returns>True</returns>
        public bool Shutdown()
        {
            return true;
        }

        /// <summary>
        /// Not used
        /// </summary>
        public bool IsInstalled
        {
            get { return true; }
        }

        /// <summary>
        /// Static name of the provider
        /// </summary>
        public string ProviderName
        {
            get { return "LoggingConsoleOutput"; }
        }
    }
}
