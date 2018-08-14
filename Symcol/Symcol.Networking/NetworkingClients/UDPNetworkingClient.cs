using System;
using System.Net.Sockets;
using osu.Framework.Logging;

namespace Symcol.Networking.NetworkingClients
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
                UdpClient = new UdpClient();
                UdpClient.Connect(EndPoint);
                Logger.Log("Successfully Updated Peer UdpClient!", LoggingTarget.Runtime, LogLevel.Debug);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error while setting up a new Peer UdpClient!");
                Dispose();
            }
        }

        public UdpNetworkingClient(int port)
            : base(port)
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                Logger.Log("No Network Connection Detected!", LoggingTarget.Network, LogLevel.Error);

            try
            {
                UdpClient = new UdpClient(port);
                Logger.Log("Successfully Updated Server UdpClient!", LoggingTarget.Runtime, LogLevel.Debug);
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error while setting up a new Server UdpClient!");
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
