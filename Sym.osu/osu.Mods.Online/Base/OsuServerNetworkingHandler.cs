#region usings

using System;
using System.Collections.Generic;
using System.Net;
using osu.Framework.Allocation;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Game;
using osu.Mods.Online.Base.Packets;
using osu.Mods.Online.Multi;
using osu.Mods.Online.Multi.Lobby.Packets;
using osu.Mods.Online.Multi.Match.Packets;
using osu.Mods.Online.Multi.Player.Packets;
using osu.Mods.Online.Multi.Settings.Options;
using osu.Mods.Online.Score;
using osu.Mods.Online.Score.Packets;
using Sym.Networking.NetworkingHandlers;
using Sym.Networking.NetworkingHandlers.Server;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Base
{
    public class OsuServerNetworkingHandler : ServerNetworkingHandler<OsuPeer>
    {
        protected override string Gamekey => "osu";

        protected readonly List<OsuMatch> OsuMatches = new List<OsuMatch>();

        protected OsuGame OsuGame { get; private set; }

        protected Storage Storage { get; private set; }

        protected ScoreStore OnlineScoreStore { get; private set; }

        protected uint MatchID;

        protected override OsuPeer GetClient(IPEndPoint end)
        {
            OsuPeer c = new OsuPeer(end)
            { 
                LastConnectionTime = Time.Current,
                Statues = ConnectionStatues.Connecting,
            };
            c.OnDisconnected += () => Peers.Remove(c);

            return c;
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage, OsuGame osu)
        {
            OnlineScoreStore = new ScoreStore(storage);
            Storage = storage;
            OsuGame = osu;

            //Stable = OsuGame.GetStorageForStableInstall();
            //Songs = Stable.GetStorageForDirectory($"Songs");
        }

        protected override void PacketReceived(PacketInfo<OsuPeer> info)
        {
            OsuMatch match;

            switch (info.Packet)
            {
                default:
                    base.PacketReceived(info);
                    break;

                case OsuConnectPacket connectPacket:
                    base.PacketReceived(info);
                    info.Client.User = connectPacket.User;
                    break;

                #region Score
                case ScoreSubmissionPacket scoreSubmission:
                    OnlineScoreStore.SetScore(scoreSubmission);
                    break;
                #endregion

                #region Import

                /*
                case ImportPacket import:
                    Client c = GetLastClient();

                    if (Maps == null)
                    {
                        List<string> paths = new List<string>();
                        foreach (string path in Stable.GetDirectories($"Songs"))
                        {
                            string modified = "";
                            for (int i = 6; i < path.Length; i++)
                                modified = modified + path[i];
                            paths.Add(modified);
                        }
                        Maps = paths.ToArray();
                    }

                    QueImport(c);
                    break;
                case SendMapPacket send:
                    if (sending) break;
                    c = GetLastClient();
                    if (ImportingClients.ContainsKey(c))
                    {
                        ImportingClients[c]++;
                        SendClientImportMap(Maps[ImportingClients[c]], c);
                    }
                    break;
                    */

                #endregion

                #region Multi

                    #region Lobby

                    case GetMatchListPacket getMatch:
                        //Send them a list of matches
                        MatchListPacket matchList = new MatchListPacket
                        {
                            MatchInfoList = GetMatches()
                        };
                        SendToPeer(matchList, info.Client);
                        break;
                    case CreateMatchPacket createMatch:

                        //A bit hacky, but makes searching easier and more accurate
                        createMatch.MatchInfo.MatchID = MatchID;
                        MatchID++;

                        //Add the new match
                        OsuMatches.Add(new OsuMatch
                        {
                            MatchInfo = createMatch.MatchInfo,
                            MatchLastUpdateTime = Time.Current
                        });
                        SendToPeer(new MatchCreatedPacket
                        {
                            MatchInfo = createMatch.MatchInfo,
                            //Make them join this match since they made it!
                            Join = true,
                        }, info.Client);
                        break;
                    case JoinMatchPacket joinPacket:
                        foreach (OsuMatch m in OsuMatches)
                            if (m.MatchInfo.MatchID == joinPacket.Match.MatchID)
                            {
                                match = m;

                                //Add them
                                OsuPeer osu = FindClient(joinPacket.User);
                                osu.User = joinPacket.User;
                                if (!match.Add(osu)) break;

                                osu.OnDisconnected += () => match.Remove(osu);

                                //Tell everyone already there someone joined
                                ShareWithMatchClients(match.MatchInfo, new PlayerJoinedPacket
                                {
                                    User = joinPacket.User
                                });

                                //Tell them they have joined
                                SendToPeer(new JoinedMatchPacket { MatchInfo = match.MatchInfo }, info.Client);
                                break;
                            }

                        Logger.Log("Couldn't find an OsuMatch matching one in packet!", LoggingTarget.Network, LogLevel.Error);
                        break;

                    #endregion

                    #region Match

                    case GetMapPacket getMap:
                        match = FindMatch(getMap.User);

                        //Tell them what map the Match is set to
                        SendToPeer(new SetMapPacket(match.MatchInfo.Map), info.Client);
                        break;
                    case SetMapPacket map:
                        match = FindMatch(map.User);

                        //Set our map
                        match.MatchInfo.Map = map.Map;

                        //Tell everyone that a new map was set
                        ShareWithMatchClients(match, map);
                        break;
                    case SettingsPacket setting:
                        match = FindMatch(setting.User);

                        //Hacky was of setting the Setting but should work
                        if (setting.Settings[0].Sync == Sync.All)
                        {
                            for (int i = 0; i < match.MatchInfo.Settings.Count; i++)
                                if (match.MatchInfo.Settings[i].Name == setting.Settings[0].Name)
                                {
                                    match.MatchInfo.Settings[i] = setting.Settings[0];
                                    goto finish;
                                }

                            //Hacky way of adding ones we dont have but should work for now
                            match.MatchInfo.Settings.Add(setting.Settings[0]);

                            finish:
                            //Send them ALL the settings just incase, you never know with these things...
                            ShareWithMatchClients(match, new SettingsPacket(match.MatchInfo.Settings.ToArray()));
                        }
                        else if (setting.Settings[0].Sync == Sync.Client)
                        {
                            OsuUser user = FindClient(setting.User).User;
                            for (int i = 0; i < user.UserSettings.Count; i++)
                                if (user.UserSettings[i].Name == setting.Settings[0].Name)
                                {
                                    user.UserSettings[i] = setting.Settings[0];
                                    goto end;
                                }

                            //Hacky way of adding ones we dont have but should work for now
                            user.UserSettings.Add(setting.Settings[0]);
                        }

                        end:
                        break;
                    case StatuesChangePacket statuesChange:
                        match = FindMatch(statuesChange.User);

                        //Set their statues
                        FindClient(statuesChange.User).User.Statues = statuesChange.User.Statues;

                        //Tell everyone they changed their statues
                        ShareWithMatchClients(match, statuesChange);
                        break;
                    case ChatPacket chat:
                        //Nothing we need to do on our end currently, just fire it right back out
                        ShareWithMatchClients(FindMatch(chat.User), chat);
                        break;
                    case LeavePacket leave:
                        match = FindMatch(leave.User);
                        if (match.Remove(FindClient(leave.User)))
                        {
                            //Tell everyone someone rage quit
                            ShareWithMatchClients(match, new PlayerDisconnectedPacket
                            {
                                User = leave.User
                            });

                            //Update their matchlist next
                            MatchListPacket list = new MatchListPacket();
                            list = (MatchListPacket)SignPacket(list);
                            list.MatchInfoList = GetMatches();

                            TcpNetworkingClient.SendPacket(list, TcpNetworkingClient.EndPoint);
                        }

                        break;
                    case LoadPlayerPacket load:
                        match = FindMatch(load.User);
                        ShareWithMatchClients(match, new PlayerLoadingPacket
                        {
                            Match = match.MatchInfo
                        });
                        break;

                    #endregion

                    #region MultiPlayer

                    case PlayerLoadedPacket loaded:
                        OsuPeer p = FindClient(loaded.User);
                        match = FindMatch(loaded.User);

                        match.Clients.Remove(p);
                        match.LoadedClients.Add(p);

                        if (match.Clients.Count == 0)
                            ShareWithMatchClients(match.MatchInfo, new MatchStartingPacket());
                        break;
                    case ScorePacket score:
                        match = FindMatch(FindClient(score.ID).User);
                        ShareWithMatchClients(match.MatchInfo, score);
                        break;
                    case SharePacket share:
                        match = FindMatch(FindClient(share.ID).User);
                        ShareWithMatchClients(match.MatchInfo, share);
                        break;
                    case MatchExitPacket exit:
                        match = FindMatch(exit.User);

                        restart:
                        foreach (OsuPeer r in match.LoadedClients)
                        {
                            match.LoadedClients.Remove(r);
                            match.Clients.Add(r);
                            goto restart;
                        }
                        ShareWithMatchClients(match.MatchInfo, exit);

                        break;

                    #endregion

                #endregion
            }
        }

        protected override void Update()
        {
            base.Update();

            restart:
            foreach (OsuMatch match in OsuMatches)
            {
                if (match.MatchInfo.Users.Count == 0 && match.MatchLastUpdateTime + 60000 <= Time.Current)
                {
                    OsuMatches.Remove(match);
                    Logger.Log("Empty match deleted!");
                    goto restart;
                }

                if (match.MatchInfo.Users.Count > 0)
                {
                    match.MatchLastUpdateTime = Time.Current;
                }
            }
        }

        protected void ShareWithMatchClients(OsuMatch match, Packet packet) => ShareWithMatchClients(match.MatchInfo, packet);

        protected void ShareWithMatchClients(MatchInfo match, Packet packet)
        {
            foreach (OsuUser user in match.Users)
                TcpNetworkingClient.SendPacket(packet, FindClient(user).EndPoint);
        }

        /// <summary>
        /// Exists since OsuMatch isn't serializable
        /// </summary>
        /// <returns></returns>
        protected List<MatchInfo> GetMatches()
        {
            List<MatchInfo> matches = new List<MatchInfo>();

            foreach (OsuMatch match in OsuMatches)
                matches.Add(match.MatchInfo);

            return matches;
        }

        protected OsuMatch FindMatch(OsuUser player)
        {
            foreach (OsuMatch m in OsuMatches)
                foreach (OsuUser p in m.MatchInfo.Users)
                    if (p.ID == player.ID)
                        return m;
            return null;
        }

        /// <summary>
        /// Exists since OsuClient isn't serializable
        /// </summary>
        /// <returns></returns>
        protected OsuPeer FindClient(OsuUser user) => FindClient(user.ID);

        protected OsuPeer FindClient(long id)
        {
            foreach (OsuPeer p in Peers)
                if (p.User.ID == id)
                    return p;
            return null;
        }

        /*

        protected Storage Stable { get; private set; }

        protected Storage Songs { get; private set; }

        protected string[] Maps;

        protected readonly Dictionary<Client, int> ImportingClients = new Dictionary<Client, int>();

        protected virtual void QueImport(Client client)
        {
            if (ImportingClients.ContainsKey(client))
            {
                ImportingClients.Remove(client);
                client.OnDisconnected -= () => remove(client);
            }
            else
            {
                ImportingClients.Add(client, 0);
                client.OnDisconnected += () => remove(client);

                SendClientImportMap(Maps[0], client);
            }

            void remove(Client c)
            {
                ImportingClients.Remove(client);
            }
        }

        private bool sending;

        protected virtual void SendClientImportMap(string name, Client client)
        {
            string full = Songs.GetFullPath(name);

            Task.Factory.StartNew(() =>
            {
                sending = true;
                try
                {
                    Logger.Log($"Client has requested to import from stable (map = {name})", LoggingTarget.Network);

                    //Basically just create the temp folder then delete the writer, bit of a hack but works for now
                    StreamWriter writer = new StreamWriter(Storage.GetStream($"online\\temp\\server\\{name}.zip", FileAccess.ReadWrite, FileMode.Create));
                    writer.Close();

                    if (Storage.Exists($"online\\temp\\server\\{name}.zip")) Storage.Delete($"online\\temp\\server\\{name}.zip");

                    //Zip up the mapset for shipping!
                    ZipFile.CreateFromDirectory(full, $"{Storage.GetFullPath("online\\temp\\server")}\\{name}.zip", CompressionLevel.Optimal, false, Encoding.UTF8);

                    long fileSize;
                    using (FileStream fs = new FileStream($"{Storage.GetFullPath("online\\temp\\server")}\\{name}.zip", FileMode.Open, FileAccess.Read))
                    {
                        fileSize = fs.Length;
                        long sum = 0;
                        byte[] data = new byte[TcpNetworkingClient.BUFFER_SIZE / 16];

                        //Tell them its on its way
                        UdpNetworkingClient.SendPacket(new SendingMapPacket(name, fileSize), client.EndPoint);

                        //Lets send the file piece by piece so we don't destroy mobile devices
                        while (sum < fileSize)
                        {
                            int count = fs.Read(data, 0, data.Length);
                            TcpNetworkStream.Write(data, 0, count);
                            sum += count;
                            Logger.Log($"Progress = {sum}/{fileSize}", LoggingTarget.Network);
                        }

                        fs.Close();
                        fs.Dispose();
                    }

                    //cleanup
                    Storage.Delete($"online\\temp\\server\\{name}.zip");
                    sending = false;
                }
                catch (Exception e)
                {
                    sending = false;
                    Logger.Error(e, "Failed to send map!", LoggingTarget.Network);

                    ImportingClients[client]++;
                    SendClientImportMap(Maps[ImportingClients[client]], client);
                }
            }, TaskCreationOptions.LongRunning);
        }

        */

        protected class OsuMatch
        {
            public MatchInfo MatchInfo;

            public List<OsuPeer> Clients = new List<OsuPeer>();

            public List<OsuPeer> LoadedClients = new List<OsuPeer>();

            public double MatchLastUpdateTime;

            public bool Add(OsuPeer client)
            {
                if (Clients.Contains(client) || LoadedClients.Contains(client) || MatchInfo.Users.Contains(client.User))
                {
                    Logger.Log($"({client.User.Username} - {client.EndPoint}) tried to be added to a match they already in!?", LoggingTarget.Network, LogLevel.Error);
                    return false;
                }

                Clients.Add(client);
                MatchInfo.Users.Add(client.User);

                return true;
            }

            public bool Remove(OsuPeer client)
            {
                if ((Clients.Contains(client) || LoadedClients.Contains(client)) && MatchInfo.Users.Contains(client.User))
                {
                    Clients.Remove(client);
                    MatchInfo.Users.Remove(client.User);

                    return true;
                }

                Logger.Log($"({client.User.Username} - {client.EndPoint}) tried to be removed from a match they aren't in!?", LoggingTarget.Network, LogLevel.Error);
                return false;
            }

            public bool Loaded(OsuPeer client)
            {
                if(Clients.Contains(client) && !LoadedClients.Contains(client))
                {
                    Clients.Remove(client);
                    LoadedClients.Add(client);

                    return true;
                }

                Logger.Log($"({client.User.Username} - {client.EndPoint}) is already loaded?", LoggingTarget.Network, LogLevel.Error);
                return false;
            }

            public bool UnLoaded(OsuPeer client)
            {
                if (!Clients.Contains(client) && LoadedClients.Contains(client))
                {
                    LoadedClients.Remove(client);
                    Clients.Add(client);

                    return true;
                }

                Logger.Log($"({client.User.Username} - {client.EndPoint}) is already unloaded?", LoggingTarget.Network, LogLevel.Error);
                return false;
            }
        }
    }
}
