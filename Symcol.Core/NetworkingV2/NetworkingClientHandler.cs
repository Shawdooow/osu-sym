using System;
using System.Collections.Generic;
using Symcol.Core.Graphics.Containers;
using Symcol.Core.NetworkingV2.NetworkingClients;
using Symcol.Core.NetworkingV2.Packets;

namespace Symcol.Core.NetworkingV2
{
    //TODO: This NEEDS its own clock to avoid fuckery later on with modified clock speeds
    public class NetworkingClientHandler : SymcolContainer
    {
        //30 Seconds by default
        protected virtual double TimeOutTime => 30000;

        protected readonly NetworkingClient ReceiveClient;

        protected readonly NetworkingClient SendClient;

        /// <summary>
        /// Just a client signature basically
        /// </summary>
        public ClientInfo ClientInfo;

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
        /// Gets hit when we get a Packet
        /// </summary>
        public Action<Packet> OnPacketReceive;

        /// <summary>
        /// (Peer) Call this when we connect to a Host (Includes list of connected peers + Host)
        /// </summary>
        public Action<List<ClientInfo>> OnConnectedToHost;

        /// <summary>
        /// (Host) Whenever a new client Connects
        /// </summary>
        public Action<ClientInfo> OnClientConnect;

        /// <summary>
        /// (Host) Whenever a new client Disconnects
        /// </summary>
        public Action<ClientInfo> OnClientDisconnect;

        /// <summary>
        /// (Host/Peer) When a new Client joins the game
        /// </summary>
        public Action<ClientInfo> OnClientJoin;

        /// <summary>
        /// Receive a full player list
        /// </summary>
        public Action<List<ClientInfo>> OnReceivePlayerList;

        /// <summary>
        /// if we are connected and in a match
        /// </summary>
        public bool InMatch;

        /// <summary>
        /// Are we in a game
        /// </summary>
        public bool InGame;

        /// <summary>
        /// Are we loaded and ready to start?
        /// </summary>
        public bool Loaded;

        /// <summary>
        /// Called to leave an in-progress game
        /// </summary>
        public Action OnAbort;

        /// <summary>
        /// Called to load the game (Includes Host)
        /// </summary>
        public Action<List<ClientInfo>> OnLoadGame;

        /// <summary>
        /// Called to start the game once loaded
        /// </summary>
        public Action StartGame;

        public readonly ClientType ClientType;

        public ConnectionStatues ConnectionStatues { get; protected set; }

        public NetworkingClientHandler()
        {
            AlwaysPresent = true;
        }

        protected override void Update()
        {
            base.Update();

            Packet p = ReceiveClient.GetPacket();
        }

        /// <summary>
        /// Poke!
        /// </summary>
        /// <param name="clientInfo"></param>
        protected void TestConnection(ClientInfo clientInfo)
        {

        }

        public void RequestPlayerList()
        {

        }

        /// <summary>
        /// Tell peers to start loading game
        /// </summary>
        public virtual void StartLoadingGame()
        {

        }

        /// <summary>
        /// Call this when the game is Loaded and ready to be started
        /// </summary>
        public virtual void GameLoaded()
        {

        }

        /// <summary>
        /// Connects to the Host
        /// </summary>
        public virtual void ConnectToHost()
        {

        }

        /// <summary>
        /// Tell peers to start and starts ours
        /// </summary>
        public virtual void SendStartGame()
        {

        }

        /// <summary>
        /// Send a Packet to the Host
        /// </summary>
        /// <param name="packet"></param>
        public void SendToHost(Packet packet)
        {
            SendClient?.SendPacket(packet);
        }

        /// <summary>
        /// Send a Packet to all Connecting clients
        /// </summary>
        /// <param name="packet"></param>
        public void SendToConnectingClients(Packet packet)
        {

        }

        /// <summary>
        /// Send a Packet to all clients Connected and waiting
        /// </summary>
        /// <param name="packet"></param>
        public void SendToConnectedClients(Packet packet)
        {

        }

        /// <summary>
        /// Send a Packet to all clients In this Match
        /// </summary>
        /// <param name="packet"></param>
        public void SendToInMatchClients(Packet packet)
        {

        }

        /// <summary>
        /// Send a Packet to all clients Loaded
        /// </summary>
        /// <param name="packet"></param>
        public void SendToLoadedClients(Packet packet)
        {

        }

        /// <summary>
        /// Send a Packet to all clients InGame
        /// </summary>
        /// <param name="packet"></param>
        public void SendToInGameClients(Packet packet)
        {

        }

        /// <summary>
        /// Send a Packet to ALL clients we know
        /// </summary>
        /// <param name="packet"></param>
        public void SendToAllClients(Packet packet)
        {

        }

        /// <summary>
        /// Send to all but the one that sent it
        /// </summary>
        /// <param name="packet"></param>
        public void ShareWithOtherPeers(Packet packet)
        {

        }

        public virtual void AbortGame()
        {

        }

        public virtual void Disconnect()
        {

        }
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
