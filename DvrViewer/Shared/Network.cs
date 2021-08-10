using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace DvrViewer.Shared
{
    public static class Network
    {
        internal static bool PingSuccessful { get; set; }

        /// <summary>
        /// Checks to see if a supplied IP address is valid
        /// </summary>
        /// <param name="source">The value to check to see if it is a valid IP address</param>
        /// <returns>Returns whether the value is a valid IP address</returns>
        public static bool IsValidIpAddress(string source)
        {
            return IPAddress.TryParse(source, out _);
        }

        public static bool CanPingIpAddress(string ip)
        {
            Ping ping = null;
            bool pingable = false;

            try
            {
                ping = new Ping();
                PingReply pingReply = ping.Send(ip);

                pingable = pingReply.Status == IPStatus.Success;
            }
            catch
            {
                // Do nothing
            }
            finally
            {
                ping?.Dispose();
            }

            return pingable;
        }

        public static bool IsTcpPortOpen(string ip, int port)
        {
            bool result;

            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    tcpClient.Connect(ip, port);
                    result = true;
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
