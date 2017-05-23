namespace Simplic.Log.FileSystem
{
    /// <summary>
    /// Contains all configurations for the FileLog-Provider
    /// </summary>
    public class FileLogConfiguration
    {
        /// <summary>
        /// Name of the log file
        /// </summary>
        public string LogfileName
        {
            get;
            set;
        }

        /// <summary>
        /// Directory to which the logfiles should write
        /// </summary>
        public string LogfileDirectory
        {
            get;
            set;
        }

        /// <summary>
        /// If set to true, the log message will be append after startup a new application instance
        /// If set to false, the files will be truncated at first
        /// </summary>
        public bool Append
        {
            get;
            set;
        }
    }
}