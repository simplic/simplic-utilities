using System;

namespace Simplic.IPC
{
    /// <summary>
    /// Information about a received command
    /// </summary>
    public class IPCReceiveEventArgs : EventArgs
    {
        /// <summary>
        /// Instance von an IPCComannd
        /// </summary>
        public string CommandName
        {
            get;
            set;
        }

        /// <summary>
        /// The current command as json
        /// </summary>
        public string JSON
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Argument which contains all information for a received command which fails
    /// </summary>
    public class IPCErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Command which failed. If not command was deliverd this can be null
        /// </summary>
        public string Command
        {
            get;
            set;
        }

        /// <summary>
        /// Message which may contains the error text
        /// </summary>
        public string ErrorMessage
        {
            get;
            set;
        }
    }
}