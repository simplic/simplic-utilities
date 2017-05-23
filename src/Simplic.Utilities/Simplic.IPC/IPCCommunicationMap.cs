using System;
using System.Collections.Generic;

namespace Simplic.IPC
{
    /// <summary>
    /// Tuple containing some command information for the communication file
    /// </summary>
    public class CommandItem
    {
        /// <summary>
        /// Id of the sended command
        /// </summary>
        public Guid PackageId
        {
            get;
            set;
        }

        /// <summary>
        /// Time when the file was send
        /// </summary>
        public long TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the sended command
        /// </summary>
        public string CommandName
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the sender
        /// </summary>
        public string SenderName
        {
            get;
            set;
        }

        /// <summary>
        /// Override equals method
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return (Guid)obj == PackageId;
        }

        /// <summary>
        /// Override GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return PackageId.GetHashCode();
        }
    }

    /// <summary>
    /// Content of a single communication file (map)
    /// </summary>
    public class IPCCommunicationMap
    {
        /// <summary>
        /// Contains all sended files from another controller
        /// </summary>
        public IList<CommandItem> SendedCommands
        {
            get;
            set;
        }

        /// <summary>
        /// Contains all readed data from another controller
        /// </summary>
        public IList<CommandItem> ReadedCommands
        {
            get;
            set;
        }
    }
}