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
using Sym.Networking.NetworkingHandlers;
using Sym.Networking.NetworkingHandlers.Peer;
using Sym.Networking.Packets;
using Sym.Networking.Packets.FileTransfer;

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
                case StartFileTransferPacket startFileTransfer:
                    receivingFile = startFileTransfer;
                    progress.Text = $"Receiving {startFileTransfer.Name}...";
                    file = new FileStream(temp.GetFullPath($"{startFileTransfer.Name}.zip"), FileMode.Create, FileAccess.Write);
                    break;
                case FileTransferPacket fileTransfer:
                    fileSize += fileTransfer.Data.Length;
                    file.Write(fileTransfer.Data, 0, fileTransfer.Data.Length);

                    progress.Progress = fileSize / (float)receivingFile.Size;
                    Logger.Log($"Data fetched for importing from stable ({fileSize}/{receivingFile.Size})", LoggingTarget.Network);

                    if (fileSize >= receivingFile.Size)
                    {
                        //lets do this!
                        import(receivingFile);

                        receivingFile = null;
                        fileSize = 0;

                        //We can start downloading the next one now
                        SendToServer(new SendMapPacket());
                    }
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

        private StartFileTransferPacket receivingFile;

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

        private void import(StartFileTransferPacket set)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    progress.Text = $"Importing {set.Name}";
                    progress.Progress = 1;

                    Logger.Log($"Beginning Import of ({set.Name})...", LoggingTarget.Network);

                    file.Close();
                    file.Dispose();

                    //Extract it to be imported via the vanilla importer
                    ZipFile.ExtractToDirectory(temp.GetFullPath($"{set.Name}.zip"), temp.GetFullPath($"{set.Name}"), Encoding.UTF8);
                    temp.Delete($"{set.Name}.zip");

                    Logger.Log($"Zip extraction while receiving ({set.Name}) sucessful!", LoggingTarget.Network);

                    //Actually import the map now from the extracted folder
                    manager.Import(temp.GetFullPath($"{set.Name}"));
                    progress.CompletionText = $"{set.Name} Imported!";
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Failed to receive map!", LoggingTarget.Network);
                    progress.Text = "Import Failed!";
                }
            }, TaskCreationOptions.LongRunning).ContinueWith(result =>
            {
                //Cleanup our mess for mobile device's sake!
                if (temp.ExistsDirectory($"{set.Name}")) temp.DeleteDirectory($"{set.Name}");
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
