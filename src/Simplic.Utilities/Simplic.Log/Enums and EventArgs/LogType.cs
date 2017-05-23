namespace Simplic.Log
{
    /// <summary>
    /// Defines all available Log-Types
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// The logged information is for debug-reasons only
        /// </summary>
        Debug = 0,

        /// <summary>
        /// The logged information is a general information,
        /// for example some output on application starup
        /// </summary>
        Info = 1,

        /// <summary>
        /// The logged information is a warning, for example if
        /// something not critical happens
        /// </summary>
        Warning = 10,

        /// <summary>
        /// The logged information is an error, which interrupt
        /// the application
        /// </summary>
        Error = 20
    }
}