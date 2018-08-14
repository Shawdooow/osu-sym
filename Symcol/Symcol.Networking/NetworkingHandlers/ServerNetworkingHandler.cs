using System;
using System.Collections.Generic;
using osu.Framework.Logging;
using Symcol.Networking.NetworkingClients;
using Symcol.Networking.Packets;

namespace Symcol.Networking.NetworkingHandlers
{
    public class ServerNetworkingHandler: NetworkingHandler
    {
        #region Fields

        /// <summary>
        /// All Connecting clients / clients losing connection
        /// </summary>
        public readonly List<ClientInfo> ConnectingClients = new List<ClientInfo>();

        /// <summary>
        /// All Connected clients
        /// </summary>
        public readonly List<ClientInfo> ConnectedClients = new List<ClientInfo>();

        public static List<NetworkingClient> NetworkingClients { get; private set; } = new List<NetworkingClient>();

        #endregion

        public ServerNetworkingHandler()
        {
            OnAddressChange += (ip, port) => { ReceivingClient = new UdpNetworkingClient(port); };
        }

        #region Update Loop

        protected override void Update()
        {
            base.Update();

            CheckClients();
        }

        /// <summary>
        /// Handle any packets we got before sending them to OnPackerReceive
        /// </summary>
        /// <param name="packet"></param>
        protected override void HandlePackets(Packet packet)
        {
            base.HandlePackets(packet);

            switch (packet)
            {
                case ConnectPacket connectPacket:
                    ConnectingClients.Add(GenerateConnectingClientInfo(connectPacket));
                    SendToClient(new ConnectedPacket(), connectPacket);
                    break;
                case DisconnectPacket disconnectPacket:
                    ClientDisconnecting(disconnectPacket);
                    break;
                case TestPacket testPacket:
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

        #endregion

        #region Send Functions

        protected void ShareWithAllClients(Packet packet)
        {
            ShareWithAllConnectingClients(packet);
            ShareWithAllConnectedClients(packet);
        }

        protected void ShareWithAllConnectingClients(Packet packet)
        {
            foreach (ClientInfo info in ConnectingClients)
                GetNetworkingClient(info).SendPacket(SignPacket(packet));
        }

        protected void ShareWithAllConnectedClients(Packet packet)
        {
            foreach (ClientInfo info in ConnectedClients)
                GetNetworkingClient(info).SendPacket(SignPacket(packet));
        }

        protected void SendToClient(Packet packet, Packet recievedPacket)
        {
            GetNetworkingClient(GetClientInfo(recievedPacket)).SendPacket(SignPacket(packet));
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

        #region Network Actions

        public virtual void Close()
        {

        }

        #endregion

        protected override void Dispose(bool isDisposing)
        {
            ShareWithAllClients(new DisconnectPacket());

            //foreach (NetworkingClient c in NetworkingClients)
            //c?.Dispose();

            //NetworkingClients = new List<NetworkingClient>();

            base.Dispose(isDisposing);
        }
    }
}
