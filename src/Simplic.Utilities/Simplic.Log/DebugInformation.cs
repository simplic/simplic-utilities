using System.Diagnostics;

namespace Simplic.Log
{
    /// <summary>
    /// Contains all information for debugging purpose
    /// </summary>
    public class DebugInformation
    {
        /// <summary>
        /// Name of the debug area
        /// </summary>
        public string AreaName
        {
            get;
            set;
        }

        /// <summary>
        /// Defines, whether the debug-area is activated or not
        /// </summary>
        public bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Defines, whether to use and print Stopwatch results with debug information
        /// </summary>
        public bool UseStopwatch
        {
            get;
            set;
        }

        /// <summary>
        /// Internal stopwatch instance
        /// </summary>
        internal Stopwatch Stopwatch
        {
            get;
            set;
        }
    }
}