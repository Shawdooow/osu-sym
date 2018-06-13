using System;
using System.Collections.Generic;
using osu.Framework.Logging;
using osu.Framework.Timing;
using Symcol.Core.Graphics.Containers;
using Symcol.Core.NetworkingV2.NetworkingClients;
using Symcol.Core.NetworkingV2.Packets;

namespace Symcol.Core.NetworkingV2
{
    public class NetworkingClientHandler : SymcolContainer
    {
        #region Fields

        //30 Seconds by default
        protected virtual double TimeOutTime => 30000;

        protected NetworkingClient ReceiveClient;

        /// <summary>
        /// Just a client signature basically
        /// </summary>
        public ClientInfo ClientInfo;

        /// <summary>
        /// If we are connected to a host / server this will be it
        /// </summary>
        public ClientInfo ServerInfo;

        /// <summary>
        /// All Connecting clients / clients losing connection
        /// </summary>
        public readonly List<ClientInfo> ConnectingClients = new List<ClientInfo>();

        /// <summary>
        /// All Connected clients
        /// </summary>
        public readonly List<ClientInfo> ConncetedClients = new List<ClientInfo>();

        /// <summary>
        /// Clients waiting in our lobby
        /// </summary>
        public readonly List<ClientInfo> InMatchClients = new List<ClientInfo>();

        /// <summary>
        /// Clients loaded and ready to start
        /// </summary>
        public readonly List<ClientInfo> LoadedClients = new List<ClientInfo>();

        /// <summary>
        /// Clients ingame playing
        /// </summary>
        public readonly List<ClientInfo> InGameClients = new List<ClientInfo>();

        /// <summary>
        /// Gets hit when we get + send a Packet
        /// </summary>
        public Action<Packet> OnPacketReceive;

        /// <summary>
        /// Call this when we connect to a Host (Includes list of connected peers + Host)
        /// </summary>
        public Action<List<ClientInfo>> OnConnectedToHost;

        /// <summary>
        /// TODO: Implement TCP connections
        /// </summary>
        public bool TCP
        {
            get => tcp;
            set
            {
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

        public event Action<ClientType> OnClientTypeChange;

        public ClientType ClientType
        {
            get => clientType;
            set
            {
                if (value != clientType)
                {
                    clientType = value;

                    switch (value)
                    {
                        case ClientType.Peer:
                            break;
                        case ClientType.Host:
                        case ClientType.Server:
                            break;
                    }

                    OnClientTypeChange?.Invoke(value);
                }
            }
        }

        private ClientType clientType;

        public ConnectionStatues ConnectionStatues { get; protected set; }

        #endregion

        public NetworkingClientHandler()
        {
            AlwaysPresent = true;

            //TODO: make sure this works as intended
            Clock = new DecoupleableInterpolatingFramedClock { IsCoupled = false };

            OnAddressChange += (ip, port) =>
            {
                string address = ip + ":" + port;
                ClientInfo = new ClientInfo
                {
                    Address = address,
                    IP = ip,
                    Port = port
                };

                if (ReceiveClient != null)
                    ReceiveClient.Address = address;
            };

            OnClientTypeChange += (type) =>
            {
                switch (type)
                {
                    case ClientType.Peer:
                        ReceiveClient = new UDPNetworkingClient(false)
                        {
                            Address = Address
                        };
                        break;
                    case ClientType.Host:
                    case ClientType.Server:
                        ReceiveClient = new UDPNetworkingClient(false)
                        {
                            Address = Address
                        };
                        break;
                }
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
            if (ClientType != ClientType.Server)
            {
                switch (packet)
                {
                    case ConnectPacket connectPacket:
                        break;
                    case DisconnectPacket disconnectPacket:
                        break;
                }
            }
            else
            {

            }

            OnPacketReceive?.Invoke(packet);
        }

        #endregion

        #region Packet and Client Helper Functions

        /// <summary>
        /// Trys to send packets wherever they must go.
        /// Also calls OnPacketReceive with it
        /// </summary>
        /// <param name="packet"></param>
        public void SendPacket(Packet packet)
        {
            switch (ClientType)
            {
                case ClientType.Peer:
                    SendToServer(packet);
                    break;
                case ClientType.Host:
                case ClientType.Server:
                    SendToAllConnectedClients(packet);
                    break;

            }
            OnPacketReceive?.Invoke(packet);
        }

        /// <summary>
        /// returns a list of all avalable packets
        /// </summary>
        /// <returns></returns>
        protected List<Packet> ReceivePackets()
        {
            List<Packet> packets = new List<Packet>();
            for (int i = 0; i < ReceiveClient.Avalable; i++)
                packets.Add(ReceiveClient.GetPacket());
            return packets;
        }

        /// <summary>
        /// Returns a send only networking client for the inputed ClientInfo
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        protected virtual NetworkingClient GetNetworkingClient(ClientInfo info)
        {
            if (TCP)
                throw new NotImplementedException("TCP client is not implemented!");
            return new UDPNetworkingClient(true)
            {
                Address = info.IP + ":" + info.Port
            };
        }

        /// <summary>
        /// Signs this packet so everyone knows where it came from
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        protected virtual Packet SignPacket(Packet packet)
        {
            packet.Address = ReceiveClient.Address;
            return packet;
        }

        #endregion

        #region Send Functions

        protected void SendToServer(Packet packet)
        {
            packet = SignPacket(packet);
            GetNetworkingClient(ServerInfo).SendPacket(packet);
        }

        protected void SendToAllConnectedClients(Packet packet)
        {
            packet = SignPacket(packet);
            foreach (ClientInfo info in ConnectingClients)
                GetNetworkingClient(info).SendPacket(packet);
        }

        #endregion

        #region Network Actions

        /// <summary>
        /// Starts the connection proccess to Host / Server
        /// </summary>
        public virtual void Connect()
        {
            if (ConnectionStatues <= ConnectionStatues.Disconnected)
            {

            }
            else
            {
                Logger.Log("We are already connecting, please wait for us to fail before retrying!", LoggingTarget.Network);
            }
        }

        #endregion

    }

    public enum ClientType
    {
        Host,
        Peer,
        Server
    }

    public enum ConnectionStatues
    {
        Disconnected,
        Connecting,
        Connected
    }
}
