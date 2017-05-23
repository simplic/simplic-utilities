using System.Net;
using System.Net.Sockets;

namespace Simplic.Net
{
    /// <summary>
    /// provide some network helper
    /// </summary>
    public class NetworkHelper
    {
        /// <summary>
        /// Proof wether a TCP-Port is in use
        /// </summary>
        /// <param name="port">Port number</param>
        /// <returns>True, if the port is in use, false if not</returns>
        public static bool IsPortInUse(int port)
        {
            string host = "localhost";

            IPAddress addr = (IPAddress)Dns.GetHostAddresses(host)[0];

            try
            {
                TcpListener tcpList = new TcpListener(addr, port);
                tcpList.Start();
                tcpList.Stop();

                return false;
            }
            catch (SocketException)
            {
                return true;
            }
        }
    }
}