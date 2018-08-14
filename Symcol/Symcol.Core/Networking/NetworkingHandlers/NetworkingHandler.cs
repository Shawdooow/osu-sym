﻿using System;
using System.Collections.Generic;
using osu.Framework.Logging;
using osu.Framework.Timing;
using Symcol.Core.Graphics.Containers;
using Symcol.Core.Networking.NetworkingClients;
using Symcol.Core.Networking.Packets;
// ReSharper disable InconsistentNaming

namespace Symcol.Core.Networking.NetworkingHandlers
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

        public ClientInfo ServerInfo;

        public static List<NetworkingClient> NetworkingClients { get; private set; } = new List<NetworkingClient>();

        /// <summary>
        /// All Connecting clients / clients losing connection
        /// </summary>
        public readonly List<ClientInfo> ConnectingClients = new List<ClientInfo>();

        /// <summary>
        /// All Connected clients
        /// </summary>
        public readonly List<ClientInfo> ConnectedClients = new List<ClientInfo>();

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

        public event Action<ClientType> OnClientTypeChange;

        public ClientType ClientType
        {
            get => clientType;
            set
            {
                if (value != clientType)
                {
                    clientType = value;

                    OnClientTypeChange?.Invoke(value);
                }
            }
        }

        private ClientType clientType;

        public ConnectionStatues ConnectionStatues { get; protected set; }

        #endregion

        public NetworkingHandler()
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

            CheckClients();
        }

        /// <summary>
        /// Handle any packets we got before sending them to OnPackerReceive
        /// </summary>
        /// <param name="packet"></param>
        protected virtual void HandlePackets(Packet packet)
        {
            switch (packet)
            {
                case ConnectPacket connectPacket:
                    ConnectingClients.Add(GenerateConnectingClientInfo(connectPacket));
                    SendToClient(new ConnectedPacket(), connectPacket);
                    break;
                case ConnectedPacket connectedPacket:
                    ConnectionStatues = ConnectionStatues.Connected;
                    OnConnectedToHost?.Invoke(null);
                    break;
                case DisconnectPacket disconnectPacket:
                    ClientDisconnecting(disconnectPacket);
                    break;
                case TestPacket testPacket:
                    if (clientType == ClientType.Peer)
                        SendPacket(new TestPacket());
                    else
                        foreach (ClientInfo info in ConnectingClients)
                            if (info.Address == testPacket.Address)
                            {
                                ConnectingClients.Remove(info);
                                info.LastConnectionTime = Time.Current;
                                info.ConnectionTryCount = 0;
                                ConnectedClients.Add(info);
                                break;
                            }
                    foreach (ClientInfo info in ConnectedClients)
                        if (info.Address == testPacket.Address)
                        {
                            info.LastConnectionTime = Time.Current;
                            info.ConnectionTryCount = 0;
                            break;
                        }
                    break;
            }

            OnPacketReceive?.Invoke(packet);
        }

        protected virtual void CheckClients()
        {
            foreach (ClientInfo client in ConnectingClients)
            {
                if (client.LastConnectionTime + TimeOutTime / 10 <= Time.Current && client.ConnectionTryCount == 0)
                    TestConnection(client);

                if (client.LastConnectionTime + TimeOutTime / 6 <= Time.Current && client.ConnectionTryCount == 1)
                    TestConnection(client);

                if (client.LastConnectionTime + TimeOutTime / 3 <= Time.Current && client.ConnectionTryCount == 2)
                    TestConnection(client);

                if (client.LastConnectionTime + TimeOutTime <= Time.Current)
                {
                    ConnectingClients.Remove(client);
                    Logger.Log("Connection to a connecting client lost! - " + client.Address, LoggingTarget.Network, LogLevel.Error);
                    break;
                }
            }

            foreach (ClientInfo client in ConnectedClients)
            {
                if (client.LastConnectionTime + TimeOutTime / 6 <= Time.Current && client.ConnectionTryCount == 0)
                    TestConnection(client);

                if (client.LastConnectionTime + TimeOutTime / 3 <= Time.Current && client.ConnectionTryCount == 1)
                    TestConnection(client);

                if (client.LastConnectionTime + TimeOutTime / 2 <= Time.Current && client.ConnectionTryCount == 2)
                    TestConnection(client);

                if (client.LastConnectionTime + TimeOutTime <= Time.Current)
                {
                    ConnectedClients.Remove(client);
                    Logger.Log("Connection to a connected client lost! - " + client.Address, LoggingTarget.Network, LogLevel.Error);
                    break;
                }
            }
        }

        #endregion

        #region Packet and Client Helper Functions

        /// <summary>
        /// Signs a packet then trys to send it wherever it must go.
        /// Also calls OnPacketReceive with it
        /// </summary>
        /// <param name="packet"></param>
        public void SendPacket(Packet packet)
        {
            packet = SignPacket(packet);
            switch (ClientType)
            {
                case ClientType.Peer:
                    SendToServer(packet);
                    break;
                case ClientType.Host:
                case ClientType.Server:
                    ShareWithAllClients(packet);
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

        /// <summary>
        /// Get a matching client info from currently connecting/connected clients
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        protected ClientInfo GetClientInfo(Packet packet)
        {
            foreach (ClientInfo info in ConnectingClients)
                if (info.Address == packet.Address)
                    return info;

            foreach (ClientInfo info in ConnectedClients)
                if (info.Address == packet.Address)
                    return info;

            return null;
        }

        /// <summary>
        /// Get a matching client info from currently connecting clients
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        protected ClientInfo GetConnectingClientInfo(Packet packet)
        {
            foreach (ClientInfo info in ConnectingClients)
                if (info.Address == packet.Address)
                    return info;
            return null;
        }

        /// <summary>
        /// Get a matching client info from currently connected clients
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        protected ClientInfo GetConnectedClientInfo(Packet packet)
        {
            foreach (ClientInfo info in ConnectedClients)
                if (info.Address == packet.Address)
                    return info;
            return null;
        }

        /// <summary>
        /// Takes a ConnectPacket and creates a ClientInfo for the connecting client
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public ClientInfo GenerateConnectingClientInfo(ConnectPacket packet)
        {
            string[] split = packet.Address.Split(':');

            string i = split[0];
            int p = int.Parse(split[1]);

            return new ClientInfo
            {
                Address = packet.Address,
                IP = i,
                Port = p,
                LastConnectionTime = Time.Current,
                Gamekey = packet.Gamekey
            };
        }

        /// <summary>
        /// Called to remove a client that is disconnecting
        /// </summary>
        /// <param name="packet"></param>
        protected void ClientDisconnecting(DisconnectPacket packet)
        {
            foreach (ClientInfo client in ConnectedClients)
                if (client.Address == packet.Address)
                {
                    ConnectedClients.Remove(client);
                    return;
                }
            foreach (ClientInfo client in ConnectingClients)
                if (client.Address == packet.Address)
                {
                    ConnectingClients.Remove(client);
                    return;
                }
        }

        /// <summary>
        /// Test a clients connection
        /// </summary>
        /// <param name="info"></param>
        protected virtual void TestConnection(ClientInfo info)
        {
            info.ConnectionTryCount++;
            NetworkingClient client = GetNetworkingClient(info);
            client.SendPacket(SignPacket(new TestPacket()));
        }

        #endregion

        #region Send Functions

        protected void SendToServer(Packet packet)
        {
            GetNetworkingClient(ServerInfo).SendPacket(packet);
        }

        protected void ShareWithAllClients(Packet packet)
        {
            ShareWithAllConnectingClients(packet);
            ShareWithAllConnectedClients(packet);
        }

        protected void ShareWithAllConnectingClients(Packet packet)
        {
            foreach (ClientInfo info in ConnectingClients)
                GetNetworkingClient(info).SendPacket(packet);
        }

        protected void ShareWithAllConnectedClients(Packet packet)
        {
            foreach (ClientInfo info in ConnectedClients)
                GetNetworkingClient(info).SendPacket(packet);
        }

        protected void SendToClient(Packet packet, Packet recievedPacket)
        {
            SignPacket(packet);
            GetNetworkingClient(GetClientInfo(recievedPacket)).SendPacket(packet);
        }

        #endregion

        #region Network Actions

        /// <summary>
        /// Starts the connection proccess to Host / Server
        /// </summary>
        public virtual void Connect()
        {
            if (ClientType != ClientType.Peer)
            {
                Logger.Log("There is nothing for us to connect to!", LoggingTarget.Network);
                return;
            }

            if (true)//ConnectionStatues <= ConnectionStatues.Disconnected)
            {
                Logger.Log($"Attempting to connect to {ServerInfo.Address}", LoggingTarget.Network);
                SendPacket(new ConnectPacket());
            }
            else
                Logger.Log("We are already connecting, please wait for us to fail before retrying!", LoggingTarget.Network);
        }

        public virtual void Disconnect()
        {
            if (ClientType != ClientType.Peer)
            {
                Logger.Log("We are not a peer!", LoggingTarget.Network);
                return;
            }

            if (ConnectionStatues >= ConnectionStatues.Connecting)
                SendPacket(new DisconnectPacket());
            else
                Logger.Log("We are not connected!", LoggingTarget.Network);
        }

        #endregion

        protected override void Dispose(bool isDisposing)
        {
            SendPacket(new DisconnectPacket());

            //foreach (NetworkingClient c in NetworkingClients)
                //c?.Dispose();

            //NetworkingClients = new List<NetworkingClient>();

            base.Dispose(isDisposing);
        }
    }

    public enum ClientType
    {
        Peer,
        Host,
        Server
    }

    public enum ConnectionStatues
    {
        Disconnected,
        Connecting,
        Connected
    }
}