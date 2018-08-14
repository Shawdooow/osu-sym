using System;
using System.Collections.Generic;
using osu.Framework.Timing;
using Symcol.Core.Graphics.Containers;
using Symcol.Networking.NetworkingClients;
using Symcol.Networking.Packets;

// ReSharper disable InconsistentNaming

namespace Symcol.Networking.NetworkingHandlers
{
    public abstract class NetworkingHandler : SymcolContainer
    {
        #region Fields

        //30 Seconds by default
        protected virtual double TimeOutTime => 30000;

        protected virtual string Gamekey => null;

        /// <summary>
        /// Just a client signature basically
        /// </summary>
        public ClientInfo ClientInfo = new ClientInfo();

        public static List<NetworkingClient> NetworkingClients { get; private set; } = new List<NetworkingClient>();

        /// <summary>
        /// Gets hit when we get + send a Packet
        /// </summary>
        public Action<Packet> OnPacketReceive;

        /// <summary>
        /// TODO: Implement TCP connections
        /// </summary>
        public bool Tcp
        {
            get => tcp;
            set
            {
                // ReSharper disable once RedundantCheckBeforeAssignment
                if (value != tcp)
                {
                    tcp = value;
                }
            }
        }

        private bool tcp;

        /// <summary>
        /// Called when the address is changed
        /// </summary>
        public event Action<string, int> OnAddressChange;

        /// <summary>
        /// Our IP:Port
        /// </summary>
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

        /// <summary>
        /// Our IP
        /// </summary>
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

        /// <summary>
        /// Our Port
        /// </summary>
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

        public ConnectionStatues ConnectionStatues { get; protected set; }

        #endregion

        protected NetworkingHandler()
        {
            AlwaysPresent = true;

            DecoupleableInterpolatingFramedClock clock = new DecoupleableInterpolatingFramedClock { IsCoupled = false };
            Clock = clock;
            clock.Start();
            
            OnAddressChange += (ip, port) =>
            {
                string address = ip + ":" + port;

                ClientInfo.Address = address;
                ClientInfo.IP = ip;
                ClientInfo.Port = port;
                ClientInfo.Gamekey = Gamekey;
            };
        }

        #region Update Loop

        protected override void Update()
        {
            base.Update();

            foreach (Packet p in ReceivePackets())
                HandlePackets(p);
        }

        /// <summary>
        /// Handle any packets we got before sending them to OnPackerReceive
        /// </summary>
        /// <param name="packet"></param>
        protected virtual void HandlePackets(Packet packet)
        {
            OnPacketReceive?.Invoke(packet);
        }

        #endregion

        #region Packet and Client Helper Functions

        /// <summary>
        /// returns a list of all avalable packets
        /// </summary>
        /// <returns></returns>
        protected List<Packet> ReceivePackets()
        {
            List<Packet> packets = new List<Packet>();
            for (int i = 0; i < GetNetworkingClient(ClientInfo)?.Available; i++)
                packets.Add(GetNetworkingClient(ClientInfo).GetPacket());
            return packets;
        }

        /// <summary>
        /// Returns a send only networking client for the inputed ClientInfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected NetworkingClient GetNetworkingClient(ClientInfo info)
        {
            foreach (NetworkingClient c in NetworkingClients)
                if (c.Address == info.Address)
                    return c;

            if (Tcp)
                throw new NotImplementedException("TCP client is not implemented!");

            UdpNetworkingClient client = new UdpNetworkingClient(info.Address);

            if (client.UdpClient != null)
                NetworkingClients.Add(client);

            return client;
        }

        /// <summary>
        /// Signs this packet so everyone knows where it came from
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        protected virtual Packet SignPacket(Packet packet)
        {
            if (packet is ConnectPacket c)
                c.Gamekey = ClientInfo.Gamekey;
            packet.Address = ClientInfo.Address;
            return packet;
        }

        #endregion

        #region Send Functions

        #endregion

        #region Network Actions

        #endregion
    }

    public enum ConnectionStatues
    {
        Disconnected,
        Connecting,
        Connected
    }
}
