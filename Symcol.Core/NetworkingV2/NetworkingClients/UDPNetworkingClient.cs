using System.Net;
using System.Net.Sockets;
using Mono.Nat;
using osu.Framework.Logging;

namespace Symcol.Core.NetworkingV2.NetworkingClients
{
    public class UDPNetworkingClient : NetworkingClient
    {
        public UdpClient UdpClient { get; private set; }

        public Mapping CurrentMapping { get; private set; }

        public override int Avalable => UdpClient.Available;

        /// <summary>
        /// if false we only receive
        /// </summary>
        public readonly bool Send;

        public UDPNetworkingClient(bool send)
        {
            Send = send;

            if (!send)
            {
                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    Logger.Log("No Network Connection Detected!", LoggingTarget.Network, LogLevel.Error);
                else
                {
                    NatUtility.DeviceFound += deviceFound;
                    NatUtility.StartDiscovery();
                }

                OnPortChange += port =>
                {
                    if (CurrentMapping != null)
                        NatMapping.Remove(CurrentMapping);
                    UdpClient = new UdpClient(port);
                    EndPoint = new IPEndPoint(IPAddress.Any, port);
                    CurrentMapping = new Mapping(Protocol.Udp, port, port);
                    NatMapping.Add(CurrentMapping);
                };
            }
            else
                OnAddressChange += (i, p) => UdpClient = new UdpClient(i, p);
        }

        private void deviceFound(object sender, DeviceEventArgs args)
        {
            INatDevice device = args.Device;

            if (Equals(NatMapping.NatDevice?.LocalAddress, device.LocalAddress))
                return;

            NatMapping.NatDevice = device;
        }

        public override void SendBytes(byte[] bytes)
        {
            UdpClient.Send(bytes, bytes.Length);
        }

        public override byte[] GetBytes()
        {
            return UdpClient.Available > 0 ? UdpClient.Receive(ref EndPoint) : null;
        }

        public override void Dispose()
        {
            UdpClient?.Dispose();

            try { NatMapping.NatDevice.DeletePortMap(CurrentMapping); }
            catch { Logger.Log("Error trying to release port mapping!", LoggingTarget.Network, LogLevel.Error); }
        }
    }
}
