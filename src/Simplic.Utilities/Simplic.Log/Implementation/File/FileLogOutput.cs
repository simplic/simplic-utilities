using System;
using System.Collections.Generic;

namespace Simplic.Log.FileSystem
{
    /// <summary>
    /// Logprovider to write all log messages to the file system
    /// </summary>
    public class FileLogOutput : ILogOutput
    {
        private FileLogConfiguration configuration;

        /// <summary>
        /// Create new Log-Provider
        /// </summary>
        public FileLogOutput()
        {
        }

        /// <summary>
        /// Write messages to the log files
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="logArea">Area where the message should be printed</param>
        /// <param name="ex">Passed exception</param>
        /// <param name="logType">Type of the log entry</param>
        /// <param name="parameter">Passed parameter to the log provider</param>
        public void Write(string message, string logArea, Exception ex, LogType logType, IDictionary<string, object> parameter)
        {
            lock (configuration)
            {
                string fileName = "";

                if (string.IsNullOrWhiteSpace(logArea))
                {
                    fileName = configuration.LogfileDirectory + configuration.LogfileName;
                }
                else
                {
                    fileName = configuration.LogfileDirectory + logArea + "." + configuration.LogfileName;
                }

                Simplic.IO.DirectoryHelper.CreateDirectoryIfNotExists(configuration.LogfileDirectory);

                using (var writer = System.IO.File.AppendText(fileName))
                {
                    writer.WriteLine(message);
                }
            }
        }

        /// <summary>
        /// Initialize Console log
        /// </summary>
        /// <param name="parameter">Need an instance of <see cref="FileLogConfiguration">FileLogConfiguration</see>/> as the parameter</param>
        /// <returns>True if the provider was initialized correctly</returns>
        public bool Initialize(params object[] parameter)
        {
            if (parameter.Length == 0)
            {
                return false;
            }
            else if ((parameter[0] is FileLogConfiguration) == false)
            {
                return false;
            }
            else
            {
                configuration = parameter[0] as FileLogConfiguration;
                if (configuration.LogfileDirectory.EndsWith("\\") == false)
                {
                    configuration.LogfileDirectory += "\\";
                }
            }

            if (configuration.Append == false)
            {
                if (System.IO.Directory.Exists(configuration.LogfileDirectory))
                {
                    System.IO.Directory.Delete(configuration.LogfileDirectory);
                }
            }

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
        /// Name of the current provider
        /// </summary>
        public string ProviderName
        {
            get { return "FileLogOutput"; }
        }
    }
}