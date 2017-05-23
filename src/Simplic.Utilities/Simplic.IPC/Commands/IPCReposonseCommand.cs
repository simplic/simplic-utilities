using System;

namespace Simplic.IPC
{
    /// <summary>
    /// Command which will be automatically send, if a command was received
    /// </summary>
    public class IPCReposonseCommand : IPCCommand
    {
        /// <summary>
        /// Id of the command
        /// </summary>
        public Guid CommandId
        {
            get;
            set;
        }

        /// <summary>
        /// name of the command
        /// </summary>
        public override string CommandName
        {
            get { return "IPCReposonseCommand"; }
        }
    }
}