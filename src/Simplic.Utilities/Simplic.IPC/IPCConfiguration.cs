namespace Simplic.IPC
{
    /// <summary>
    /// Configuration for an ipc controller. Must be used for sending and receiving data
    /// </summary>
    public class IPCConfiguration
    {
        #region Private Member

        private string connectionString;
        private string name;

        #endregion Private Member

        #region Public Member

        /// <summary>
        /// Must be an unique configuration name for the controller (client/server like)
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        #endregion Public Member
    }
}