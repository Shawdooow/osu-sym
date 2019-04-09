#region usings

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using osu.Core;
using osu.Core.Config;
using osu.Framework.Allocation;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Game.Beatmaps;
using osu.Game.Online.API;
using osu.Game.Overlays;
using osu.Game.Overlays.Notifications;
using osu.Mods.Online.Base.Packets;
using osu.Mods.Online.Multi.Lobby.Packets;
using Sym.Networking.NetworkingClients;
using Sym.Networking.NetworkingHandlers;
using Sym.Networking.NetworkingHandlers.Peer;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Base
{
    public class OsuNetworkingHandler : PeerNetworkingHandler, IOnlineComponent
    {
        protected override string Gamekey => "osu";

        public OsuUser OsuUser = new OsuUser();

        private APIState apiState = APIState.Offline;

        private bool connectReady;
        private double forceTime = double.MinValue;

        private BeatmapManager manager;
        private Storage storage;
        private Storage temp;
        private NotificationOverlay notifications;
        private ProgressNotification progress;

        public override void Connect()
        {
            if (apiState != APIState.Online)
            {
                connectReady = true;
                forceTime = Time.Current + 2000;
            }
            else
                connect();
        }

        private void connect()
        {
            Logger.Log($"Attempting to connect to {TcpNetworkingClient.Address}", LoggingTarget.Network);
            SendToServer(new OsuConnectPacket());
            Host.Statues = ConnectionStatues.Connecting;
        }

        protected override void Update()
        {
            base.Update();

            if (forceTime <= Time.Current && Host.Statues <= ConnectionStatues.Disconnected)
            {
                connect();
                Logger.Log($"Joining server (without logging in) as ({OsuUser.Username} + {OsuUser.ID})", LoggingTarget.Network, LogLevel.Error);
            }

            //We got the whole map yet?
            if (receivingMap != null && fileSize == receivingMap.Size)
            {
                //lets do this!
                import(receivingMap);

                receivingMap = null;
                fileSize = 0;

                //We can start downloading the next one now
                SendToServer(new SendMapPacket());
            }
        }

        [BackgroundDependencyLoader]
        private void load(APIAccess api, BeatmapManager manager, Storage storage, NotificationOverlay notifications)
        {
            api.Register(this);

            OsuUser.Colour = SymcolOsuModSet.SymConfigManager.Get<string>(SymSetting.PlayerColor);
            OsuUser.Username = SymcolOsuModSet.SymConfigManager.Get<string>(SymSetting.SavedName);
            OsuUser.ID = SymcolOsuModSet.SymConfigManager.Get<long>(SymSetting.SavedUserID);

            this.manager = manager;
            this.storage = storage;
            this.notifications = notifications;
            temp = this.storage.GetStorageForDirectory("online\\temp");

            //Basically just create the temp folder then delete the writer, bit of a hack but works for now
            StreamWriter writer = new StreamWriter(temp.GetStream($"mem.zip", FileAccess.ReadWrite, FileMode.Create));
            writer.Close();
            temp.Delete($"mem.zip");
        }

        protected override void PacketReceived(PacketInfo<Host> info)
        {
            switch (info.Packet)
            {
                default:
                    base.PacketReceived(info);
                    break;
                case SendingMapPacket sendingMapPacket:
                    if (receivingMap != null)
                    {
                        if (receivingMap.MapName == sendingMapPacket.MapName) break;

                        byte[] data = new byte[TcpNetworkingClient.BUFFER_SIZE / 8];
                        //while (TcpNetworkingClient.Available > 0)
                            //TcpNetworkStream.Read(data, 0, data.Length);
                    }

                    receivingMap = sendingMapPacket;
                    progress.Text = $"Receiving {sendingMapPacket.MapName}...";
                    file = new FileStream(temp.GetFullPath($"{sendingMapPacket.MapName}.zip"), FileMode.Create, FileAccess.Write);
                    break;
            }
        }

        protected override Packet SignPacket(Packet packet)
        {
            switch (packet)
            {
                case OsuConnectPacket connectPacket:
                    connectPacket.User = OsuUser;
                    break;
                case OnlinePacket onlinePacket:
                    onlinePacket.User = OsuUser;
                    break;
            }
            return base.SignPacket(packet);
        }

        public void APIStateChanged(APIAccess api, APIState state)
        {
            apiState = state;
            OsuUser.Colour = SymcolOsuModSet.SymConfigManager.GetBindable<string>(SymSetting.PlayerColor);

            switch (state)
            {
                default:
                    OsuUser.Username = SymcolOsuModSet.SymConfigManager.Get<string>(SymSetting.SavedName);
                    OsuUser.ID = SymcolOsuModSet.SymConfigManager.Get<long>(SymSetting.SavedUserID);
                    break;
                case APIState.Online:
                    SymcolOsuModSet.SymConfigManager.Set(SymSetting.SavedName, api.LocalUser.Value.Username);
                    SymcolOsuModSet.SymConfigManager.Set(SymSetting.SavedUserID, api.LocalUser.Value.Id);

                    OsuUser.Username = api.LocalUser.Value.Username;
                    OsuUser.ID = api.LocalUser.Value.Id;
                    OsuUser.Country = api.LocalUser.Value.Country.FullName;
                    OsuUser.CountryFlagName = api.LocalUser.Value.Country.FlagName;
                    OsuUser.Pic = api.LocalUser.Value.AvatarUrl;
                    OsuUser.Background = api.LocalUser.Value.CoverUrl;

                    if (connectReady)
                        connect();
                    else
                        Logger.Log("We are now online, you should reconnect to the Sym Server at your earliest convenience!", LoggingTarget.Network, LogLevel.Important);
                    break;
            }
        }

        #region Import

        private SendingMapPacket receivingMap;

        private int fileSize;
        private FileStream file;

        /// <summary>
        /// Request to import from stable
        /// </summary>
        public void Import()
        {
            if (progress == null)
                notifications.Post(progress = new ProgressNotification
                {
                    Text = "Getting Ready...",
                    Progress = 0,
                    State = ProgressNotificationState.Active,
                });
            else
                progress.State = ProgressNotificationState.Cancelled;

            SendToServer(new ImportPacket());
        }

        private void import(SendingMapPacket set)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    progress.Text = $"Importing {set.MapName}";
                    progress.Progress = 1;

                    Logger.Log($"Beginning Import of ({set.MapName})...", LoggingTarget.Network);

                    file.Close();
                    file.Dispose();

                    //Extract it to be imported via the vanilla importer
                    ZipFile.ExtractToDirectory(temp.GetFullPath($"{set.MapName}.zip"), temp.GetFullPath($"{set.MapName}"), Encoding.UTF8);
                    temp.Delete($"{set.MapName}.zip");

                    Logger.Log($"Zip extraction while receiving ({set.MapName}) sucessful!", LoggingTarget.Network);

                    //Actually import the map now from the extracted folder
                    manager.Import(temp.GetFullPath($"{set.MapName}"));
                    progress.CompletionText = $"{set.MapName} Imported!";
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Failed to receive map!", LoggingTarget.Network);
                    progress.Text = "Import Failed!";
                }
            }, TaskCreationOptions.LongRunning).ContinueWith(result =>
            {
                //Cleanup our mess for mobile device's sake!
                if (temp.ExistsDirectory($"{set.MapName}")) temp.DeleteDirectory($"{set.MapName}");
            });
        }

        protected override void Dispose(bool isDisposing)
        {
            if (storage.ExistsDirectory("online/temp")) storage.DeleteDirectory("online/temp");

            if (progress != null)
                progress.State = ProgressNotificationState.Cancelled;
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
