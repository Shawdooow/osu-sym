﻿#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using osu.Framework.Allocation;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Game;
using osu.Game.Beatmaps;
using osu.Mods.Online.Base.Packets;
using osu.Mods.Online.Multi;
using osu.Mods.Online.Multi.Lobby.Packets;
using osu.Mods.Online.Multi.Match.Packets;
using osu.Mods.Online.Multi.Player.Packets;
using osu.Mods.Online.Multi.Settings.Options;
using osu.Mods.Online.Score;
using osu.Mods.Online.Score.Packets;
using Sym.Networking.NetworkingClients;
using Sym.Networking.NetworkingHandlers;
using Sym.Networking.NetworkingHandlers.Server;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Base
{
    public class OsuServerNetworkingHandler : ServerNetworkingHandler
    {
        protected override string Gamekey => "osu";

        protected readonly List<OsuMatch> OsuMatches = new List<OsuMatch>();

        protected OsuGame OsuGame { get; private set; }

        protected ScoreStore OnlineScoreStore { get; private set; }

        protected uint MatchID;

        protected override Client CreateConnectingClient(ConnectPacket connectPacket)
        {
            OsuConnectPacket osuConnectPacket = (OsuConnectPacket)connectPacket;

            OsuClient c = new OsuClient
            {
                EndPoint = new IPEndPoint(IPAddress.Parse(UdpNetworkingClient.EndPoint.Address.ToString()), UdpNetworkingClient.EndPoint.Port),
                LastConnectionTime = Time.Current,
                Statues = ConnectionStatues.Connecting,
                User = osuConnectPacket.User,
            };
            c.OnDisconnected += () => Clients.Remove(c);

            return c;
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage, OsuGame osu)
        {
            OnlineScoreStore = new ScoreStore(storage);
            OsuGame = osu;
        }

        protected override void HandlePackets(PacketInfo info)
        {
            Logger.Log($"Recieved a Packet from {UdpNetworkingClient.EndPoint}", LoggingTarget.Network, LogLevel.Debug);

            if (!HandlePacket(info.Packet))
                return;

            OsuMatch match;

            switch (info.Packet)
            {
                default:
                    base.HandlePackets(info);
                    break;

                #region Score
                case ScoreSubmissionPacket scoreSubmission:
                    OnlineScoreStore.SetScore(scoreSubmission);
                    break;
                #endregion

                #region Import
                case ImportPacket import:
                    Storage stable = OsuGame.GetStorageForStableInstall();
                    Storage storage = stable.GetStorageForDirectory($"Songs");

                    string name = "Alstroemeria Records - Flight of the Bamboo Cutter";

                    string full = storage.GetFullPath(name);

                    if (!storage.Exists($"temp\\{name}.zip"))
                        ZipFile.CreateFromDirectory(full, $"{storage.GetFullPath("temp")}\\{name}.zip");

                    StreamReader reader = new StreamReader(storage.GetStream($"temp\\{name}.zip"));
                    string s = reader.ReadToEnd();
                    int pieces = 0;

                    const int chunk = 8192;

                    for(int i = 0; i < s.Length; i += chunk)
                    {
                        pieces++;
                        List<char> chars = new List<char>();
                        for (int w = 0; w < chunk; w++)
                        {
                            try
                            {
                                chars.Add(s[w * i / chunk]);
                            }
                            catch (Exception e)
                            {
                                if (e is IndexOutOfRangeException) break;
                            }
                        }
                        ReturnToClient(new MapSetPiecePacket(chars.ToArray().ToString(), name, i / chunk));
                    }

                    ReturnToClient(new MapSetPacket(name, pieces));

                    /*
                    foreach (string map in stable.GetDirectories("Songs"))
                    {
                        //Alstroemeria Records - Flight of the Bamboo Cutter
                        //last.SendPacket(new MapSetPacket(stable.GetStorageForDirectory($"Songs/{map}")));
                        last.SendPacket(new MapSetPacket(stable.GetStorageForDirectory($"Songs/Alstroemeria Records - Flight of the Bamboo Cutter")));
                        break;
                    }
                    */

                    break;
                #endregion

                #region Multi

                #region Lobby

                case GetMatchListPacket getMatch:
                        //Send them a list of matches
                        MatchListPacket matchList = new MatchListPacket
                        {
                            MatchInfoList = GetMatches()
                        };
                        ReturnToClient(matchList);
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
                        ReturnToClient(new MatchCreatedPacket
                        {
                            MatchInfo = createMatch.MatchInfo,
                            //Make them join this match since they made it!
                            Join = true,
                        });
                        break;
                    case JoinMatchPacket joinPacket:
                        foreach (OsuMatch m in OsuMatches)
                            if (m.MatchInfo.MatchID == joinPacket.Match.MatchID)
                            {
                                match = m;

                                //Add them
                                OsuClient osu = FindClient(joinPacket.User);
                                osu.User = joinPacket.User;
                                if (!match.Add(osu)) break;

                                osu.OnDisconnected += () => match.Remove(osu);

                                //Tell everyone already there someone joined
                                ShareWithMatchClients(match.MatchInfo, new PlayerJoinedPacket
                                {
                                    User = joinPacket.User
                                });

                                //Tell them they have joined
                                ReturnToClient(new JoinedMatchPacket { MatchInfo = match.MatchInfo });
                                break;
                            }

                        Logger.Log("Couldn't find an OsuMatch matching one in packet!", LoggingTarget.Network, LogLevel.Error);
                        break;

                    #endregion

                    #region Match

                    case GetMapPacket getMap:
                        match = FindMatch(getMap.User);

                        //Tell them what map the Match is set to
                        UdpNetworkingClient.SendPacket(SignPacket(new SetMapPacket(match.MatchInfo.Map)), GetLastClient().EndPoint);
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
                            OsuUserInfo user = FindClient(setting.User).User;
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

                            UdpNetworkingClient.SendPacket(list, GetLastClient().EndPoint);
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
                        OsuClient p = FindClient(loaded.User);
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
                        foreach (OsuClient r in match.LoadedClients)
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
            foreach (OsuUserInfo user in match.Users)
                UdpNetworkingClient.SendPacket(packet, FindClient(user).EndPoint);
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

        protected OsuMatch FindMatch(OsuUserInfo player)
        {
            foreach (OsuMatch m in OsuMatches)
                foreach (OsuUserInfo p in m.MatchInfo.Users)
                    if (p.ID == player.ID)
                        return m;
            return null;
        }

        /// <summary>
        /// Exists since OsuClient isn't serializable
        /// </summary>
        /// <returns></returns>
        protected OsuClient FindClient(OsuUserInfo user) => FindClient(user.ID);

        protected OsuClient FindClient(long id)
        {
            foreach (Client c in Clients)
            {
                OsuClient osu = (OsuClient)c;
                if (osu.User.ID == id)
                    return osu;
            }
            return null;
        }

        protected class OsuMatch
        {
            public MatchInfo MatchInfo;

            public List<OsuClient> Clients = new List<OsuClient>();

            public List<OsuClient> LoadedClients = new List<OsuClient>();

            public double MatchLastUpdateTime;

            public bool Add(OsuClient client)
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

            public bool Remove(OsuClient client)
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

            public bool Loaded(OsuClient client)
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

            public bool UnLoaded(OsuClient client)
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
