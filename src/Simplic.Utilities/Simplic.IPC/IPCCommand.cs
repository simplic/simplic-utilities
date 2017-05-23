namespace Simplic.IPC
{
    /// <summary>
    /// Abstract ipc command, which must be implemented in all commands that should be send
    /// </summary>
    public abstract class IPCCommand
    {
        /// <summary>
        /// Create a new command instance and prepare every thing
        /// </summary>
        public IPCCommand()
        {
        }

        /// <summary>
        /// Returns an unique command name
        /// </summary>
        public abstract string CommandName
        {
            get;
        }
    }
}