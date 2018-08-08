using System;
using System.Net;
using System.Net.Sockets;
using osu.Framework.Logging;

namespace Symcol.Core.Networking.NetworkingClients
{
    public class UdpNetworkingClient : NetworkingClient
    {
        public readonly UdpClient UdpClient;

        public override int Available => UdpClient?.Available ?? 0;

        public UdpNetworkingClient(string address)
            : base(address)
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                Logger.Log("No Network Connection Detected!", LoggingTarget.Network, LogLevel.Error);

            try
            {
                string[] a = IP.Split('.');
                byte[] addressBytes = { (byte)int.Parse(a[0]), (byte)int.Parse(a[1]), (byte)int.Parse(a[2]), (byte)int.Parse(a[3]) };

                EndPoint = new IPEndPoint(new IPAddress(addressBytes), Port);
                UdpClient = new UdpClient(EndPoint);
                UdpClient.Connect(EndPoint);
                Logger.Log("Successfully Updated UdpClient!", LoggingTarget.Runtime, LogLevel.Debug);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error while setting up a new UdpClient!");
                Dispose();
            }
        }

        public override void SendBytes(byte[] bytes)
        {
            try
            {
                UdpClient.Send(bytes, bytes.Length);
                Logger.Log("Successfully sent bytes!", LoggingTarget.Runtime, LogLevel.Debug);
            }
            catch (Exception e) { Logger.Error(e, "Error sending bytes!"); }
        }

        public override byte[] GetBytes()
        {
            return Available > 0 ? UdpClient.Receive(ref EndPoint) : null;
        }

        public override void Dispose()
        {
            UdpClient?.Close();
            UdpClient?.Dispose();
        }
    }
}
