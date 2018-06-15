using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using Mono.Nat;
using osu.Framework.Logging;
using Symcol.Core.Networking.Packets;

// ReSharper disable InconsistentNaming

namespace Symcol.Core.Networking.NetworkingClients
{
    public abstract class NetworkingClient : IDisposable
    {
        public IPEndPoint EndPoint;

        public abstract int Avalable { get; }

        /// <summary>
        /// Called when the address is changed
        /// </summary>
        public event Action<string, int> OnAddressChange;

        public string Address
        {
            get => IP + ":" + Port;
            set
            {
                string[] split = value.Split(':');
                
                string i = split[0];
                int p = int.Parse(split[1]);

                if (IP + Port != value)
                {
                    foreach (Mapping m in NatMapping.Mappings)
                        if (m.PrivatePort == Port)
                        {
                            NatMapping.Remove(m);
                            break;
                        }

                    NatMapping.Add(new Mapping(Protocol.Udp, p, p));
                    IP = i;
                    Port = p;
                    OnAddressChange?.Invoke(i, p);
                }
            }
        }

        /// <summary>
        /// Called when the ip is changed
        /// </summary>
        public event Action<string> OnIPChange;

        public string IP
        {
            get => ip;
            private set
            {
                if (ip != value)
                {
                    ip = value;
                    OnIPChange?.Invoke(ip);
                }
            }
        }

        private string ip;

        /// <summary>
        /// Called when the port is changed
        /// </summary>
        public event Action<int> OnPortChange;

        public int Port
        {
            get => port;
            private set
            {
                if (port != value)
                {
                    port = value;
                    OnPortChange?.Invoke(port);
                }
            }
        }

        private int port;

        /// <summary>
        /// Send a packet
        /// </summary>
        /// <param name="packet"></param>
        public virtual void SendPacket(Packet packet)
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
                    Logger.Log("Warning: Packet being sent is larger than its predefined size of (" + packet.PacketSize + "  bytes) and is being resized to (" + i + " bytes)", LoggingTarget.Network, LogLevel.Error);
                    goto retry;
                }

                SendBytes(data);
            }
        }

        /// <summary>
        /// Receive a packet
        /// </summary>
        /// <returns></returns>
        public virtual Packet GetPacket()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] data = GetBytes();
                stream.Write(data, 0, data.Length);

                stream.Position = 0;

                BinaryFormatter formatter = new BinaryFormatter();
                if (formatter.Deserialize(stream) is Packet packet)
                {
                    //TODO: not this, each client should send the packet with this information somehow
                    packet.Address = EndPoint.Address.ToString() + EndPoint.Port.ToString();
                    return packet;
                }

                throw new NullReferenceException("Whatever we recieved isnt a packet!");
            }
        }

        /// <summary>
        /// Send some bytes
        /// </summary>
        /// <param name="bytes"></param>
        public abstract void SendBytes(byte[] bytes);

        /// <summary>
        /// Receive some bytes
        /// </summary>
        /// <returns></returns>
        public abstract byte[] GetBytes();

        public abstract void Dispose();
    }
}
