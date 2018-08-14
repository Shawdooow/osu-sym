using System;
using osu.Framework.Logging;
using Symcol.Core.Networking.Packets;

namespace Symcol.Core.Networking.NetworkingHandlers
{
    public class PeerNetworkingHandler : NetworkingHandler
    {
        #region Fields

        /// <summary>
        /// Call this when we connect to a Host (Includes list of connected peers + Host)
        /// </summary>
        public Action<ClientInfo> OnConnectedToHost;

        #endregion

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
        protected override void HandlePackets(Packet packet)
        {
            switch (packet)
            {
                case ConnectedPacket connectedPacket:
                    ConnectionStatues = ConnectionStatues.Connected;
                    Logger.Log("Connected to server!", LoggingTarget.Network);
                    OnConnectedToHost?.Invoke(ClientInfo);
                    break;
                case DisconnectPacket disconnectPacket:
                    Logger.Log("Server shutting down!", LoggingTarget.Network);
                    break;
                case TestPacket testPacket:
                    SendToServer(new TestPacket());
                    break;
            }

            OnPacketReceive?.Invoke(packet);
        }

        #endregion

        #region Packet and Client Helper Functions

        #endregion

        #region Send Functions

        public virtual void SendToServer(Packet packet)
        {
            GetNetworkingClient(ClientInfo).SendPacket(packet);
        }

        #endregion

        #region Network Actions

        /// <summary>
        /// Starts the connection proccess to Host / Server
        /// </summary>
        public virtual void Connect()
        {
            if (true)//ConnectionStatues <= ConnectionStatues.Disconnected)
            {
                Logger.Log($"Attempting to connect to {ClientInfo.Address}", LoggingTarget.Network);
                SendToServer(new ConnectPacket());
            }
            else
                Logger.Log("We are already connecting, please wait for us to fail before retrying!", LoggingTarget.Network);
        }

        public virtual void Disconnect()
        {
            if (ConnectionStatues >= ConnectionStatues.Connecting)
                SendToServer(new DisconnectPacket());
            else
                Logger.Log("We are not connected!", LoggingTarget.Network);
        }

        #endregion

        protected override void Dispose(bool isDisposing)
        {
            SendToServer(new DisconnectPacket());

            //foreach (NetworkingClient c in NetworkingClients)
            //c?.Dispose();

            //NetworkingClients = new List<NetworkingClient>();

            base.Dispose(isDisposing);
        }
    }
}
