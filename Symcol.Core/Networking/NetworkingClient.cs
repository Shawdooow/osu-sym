using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Mono.Nat;
using osu.Framework.Logging;

namespace Symcol.Core.Networking
{
    public class NetworkingClient
    {
        public readonly UdpClient UdpClient;

        public readonly NatMapping NatMapping;

        public IPEndPoint EndPoint;

        public Action OnStartedUPnPMapping;

        /// <summary>
        /// if false we only receive
        /// </summary>
        public readonly bool Send;

        public readonly int Port;

        public readonly string IP;

        public NetworkingClient(bool send, string ip, int port = 25570)
        {
            Port = port;
            IP = ip;

            NatMapping = new NatMapping(new Mapping(Protocol.Udp, Port, Port));

            if (!send)
            {
                foreach (INatDevice device in NatMapping.NatDevices)
                    device.CreatePortMap(NatMapping.UdpMapping);

                if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    Logger.Log("No Network Connection Detected!", LoggingTarget.Network, LogLevel.Error);

                else
                {
                    NatUtility.DeviceFound += deviceFound;
                    NatUtility.DeviceLost += deviceLost;
                    NatUtility.StartDiscovery();
                }
            }

            if (send)
                UdpClient = new UdpClient(IP, Port);
            else
            {
                UdpClient = new UdpClient(Port);
                EndPoint = new IPEndPoint(IPAddress.Any, Port);
            }
        }

        private void deviceFound(object sender, DeviceEventArgs args)
        {
            INatDevice device = args.Device;

            foreach (INatDevice d in NatMapping.NatDevices)
                if (d.LocalAddress == device.LocalAddress)
                    return;

            device.CreatePortMap(NatMapping.UdpMapping);
            NatMapping.NatDevices.Add(device);

            if (OnStartedUPnPMapping != null)
            {
                OnStartedUPnPMapping();
                OnStartedUPnPMapping = null;
            }
        }

        private void deviceLost(object sender, DeviceEventArgs args)
        {
            INatDevice device = args.Device;
            device.DeletePortMap(NatMapping.UdpMapping);
            NatMapping.NatDevices.Remove(device);
        }

        private void sendByte(byte[] data)
        {
            UdpClient.Send(data, data.Length);
        }

        private byte[] receiveByte()
        {
            return UdpClient.Receive(ref EndPoint);
        }

        /// <summary>
        /// Send a Packet somewhere
        /// </summary>
        /// <param name="packet"></param>
        public void SendPacket(Packet packet)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, packet);

                stream.Position = 0;

                int i = packet.PacketSize;
                retry:
                byte[] data = new byte[i];

                try
                {
                    stream.Read(data, 0, (int)stream.Length);
                }
                catch
                {
                    i *= 2;
                    goto retry;
                }

                sendByte(data);
            }
        }

        /// <summary>
        /// Receive a Packet from somewhere
        /// </summary>
        /// <returns></returns>
        public Packet ReceivePacket(bool force = false)
        {
            if (UdpClient.Available > 0 || force)
                using (MemoryStream stream = new MemoryStream())
                {
                    byte[] data = receiveByte();
                    stream.Write(data, 0, data.Length);

                    stream.Position = 0;

                    BinaryFormatter formatter = new BinaryFormatter();
                    Packet packet = (Packet)formatter.Deserialize(stream);
                    packet.ClientInfo.IP = EndPoint.Address.ToString();

                    return packet;
                }
            else
                return null;
        }

        public void Clear()
        {
            if (UdpClient != null)
                UdpClient.Dispose();

            foreach (INatDevice device in NatMapping.NatDevices)
                try
                {
                    device.DeletePortMap(NatMapping.UdpMapping);
                }
                catch { Logger.Log("Error trying to release port mapping!", LoggingTarget.Network, LogLevel.Error); }
        }
    }
}
