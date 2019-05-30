using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using osu.Framework.Logging;
using Symcol.Networking.Packets;

// ReSharper disable InconsistentNaming

namespace Symcol.Networking.NetworkingClients
{
    public abstract class NetworkingClient : IDisposable
    {
        public IPEndPoint EndPoint;

        public abstract int Available { get; }

        public readonly string Address;

        public readonly string IP;

        public readonly int Port;

        protected NetworkingClient(string address)
        {
            Address = address;

            string[] split = address.Split(':');
            string i = split[0];
            int p = int.Parse(split[1]);

            IP = i;
            Port = p;

            EndPoint = new IPEndPoint(IPAddress.Parse(IP), Port);
        }

        protected NetworkingClient(int port)
        {
            Port = port;
            EndPoint = new IPEndPoint(IPAddress.Any, port);
        }

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
                    return packet;

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
