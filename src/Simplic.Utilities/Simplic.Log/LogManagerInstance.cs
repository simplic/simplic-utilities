namespace Simplic.Log
{
    /// <summary>
    /// Represents an instance of the simplic logging system. The only member in this class is Instance, which
    /// is an instance <see cref="LogManager"/>
    /// </summary>
    public static class LogManagerInstance
    {
        /// <summary>
        /// Create instance of a log manager
        /// </summary>
        private static readonly LogManager logManager = new LogManager();

        /// <summary>
        /// Represents an instance of the default LogManager. This instance can be used for applicate wide logging
        /// </summary>
        public static LogManager Instance
        {
            get { return LogManagerInstance.logManager; }
        }
    }
}