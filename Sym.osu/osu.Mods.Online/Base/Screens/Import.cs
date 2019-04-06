#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using osu.Core.Containers.Shawdooow;
using osu.Core.Screens.Evast;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Overlays;
using osu.Game.Overlays.Notifications;
using osu.Game.Overlays.Settings;
using osu.Mods.Online.Base.Packets;
using osuTK;
using osuTK.Graphics;
using Sym.Networking.NetworkingClients;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Base.Screens
{
    public class Import : BeatmapScreen
    {
        private BeatmapManager manager;

        private Storage storage;

        private Storage temp;

        private NotificationOverlay notifications;

        private ProgressNotification progress;

        public Import()
        {
            Children = new Drawable[]
            {
                new SettingsButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Import from Host's Stable Install",
                    Action = import
                },
                new SymcolButton
                {
                    ButtonText = "Back",
                    Origin = Anchor.BottomCentre,
                    Anchor = Anchor.BottomCentre,
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    Size = 50,
                    Action = this.Exit,
                    Position = new Vector2(0 , -10),
                },
                new SettingsButton
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Toggle TCP",
                    Action = () => { OnlineModset.OsuNetworkingHandler.Tcp = !OnlineModset.OsuNetworkingHandler.Tcp; }
                },
            };

            OnlineModset.OsuNetworkingHandler.OnPacketReceive += packetReceived;
        }

        [BackgroundDependencyLoader]
        private void load(BeatmapManager manager, Storage storage, NotificationOverlay notifications)
        {
            this.manager = manager;
            this.storage = storage;
            this.notifications = notifications;
            temp = this.storage.GetStorageForDirectory("online\\temp");

            //Basically just create the temp folder then delete the writer, bit of a hack but works for now
            StreamWriter writer = new StreamWriter(temp.GetStream($"mem.zip", FileAccess.ReadWrite, FileMode.Create));
            writer.Close();
            temp.Delete($"mem.zip");
        }

        private SendingMapPacket receivingMap;
        private SentMapPacket quedSetPacket;

        private int piecesSize;
        private Dictionary<byte[], int> pieces = new Dictionary<byte[], int>();

        protected override void Update()
        {
            base.Update();

            //this could be for looped but lets not threadlock for now instead
            if (OnlineModset.OsuNetworkingHandler.Tcp && OnlineModset.OsuNetworkingHandler.TcpNetworkingClient.Available > 0 && receivingMap != null)
                fetch();

            //We got the whole map yet?
            if (quedSetPacket != null && piecesSize == quedSetPacket.Size && receivingMap != null)
            {
                receivingMap = null;
                //lets do this!
                import(quedSetPacket);
            }
        }

        /// <summary>
        /// Fetch TCP data if we are importing
        /// </summary>
        private void fetch()
        {
            byte[] data = new byte[TcpNetworkingClient.BUFFER_SIZE / 8];
            int count = OnlineModset.OsuNetworkingHandler.TcpNetworkStream.Read(data, 0, data.Length);

            piecesSize += count;
            pieces.Add(data, count);

            progress.Progress = piecesSize / (float)receivingMap.Size;
            Logger.Log($"Data fetched for importing from stable ({piecesSize}/{receivingMap.Size})", LoggingTarget.Network);
        }

        /// <summary>
        /// Request to import from stable
        /// </summary>
        private void import()
        {
            if (progress == null)
                notifications.Post(progress = new ProgressNotification());

            progress.Text = "Getting Ready...";
            progress.Progress = 0;
            progress.State = ProgressNotificationState.Active;

            OnlineModset.OsuNetworkingHandler.SendToServer(new ImportPacket());
        }

        private void packetReceived(PacketInfo info)
        {
            switch (info.Packet)
            {
                case SendingMapPacket sendingMapPacket:
                    receivingMap = sendingMapPacket;
                    progress.Text = $"Receiving {sendingMapPacket.MapName}...";
                    break;
                case SentMapPacket sentMapPacket:
                    //que it since we might not have the whole mapset.zip yet
                    quedSetPacket = sentMapPacket;
                    break;
            }
        }

        private void import(SentMapPacket set)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    progress.Text = $"Importing {set.MapName}";
                    progress.Progress = 1;

                    Logger.Log($"Beginning Import of ({set.MapName})...", LoggingTarget.Network);

                    using (FileStream fs = new FileStream(temp.GetFullPath($"{set.MapName}.zip"), FileMode.Create, FileAccess.Write))
                    {
                        //Save all the pieces until we have all of them
                        foreach (KeyValuePair<byte[], int> pair in pieces)
                            fs.Write(pair.Key, 0, pair.Value);

                        fs.Close();
                        fs.Dispose();
                    }

                    //Extract it to be imported via the vanilla importer
                    ZipFile.ExtractToDirectory(temp.GetFullPath($"{set.MapName}.zip"), temp.GetFullPath($"{set.MapName}"), Encoding.UTF8);
                    Logger.Log($"Zip extraction while receiving ({set.MapName}) sucessful!", LoggingTarget.Network);

                    //Actually import the map now from the extracted folder
                    manager.Import(new[]
                    {
                        temp.GetFullPath($"{set.MapName}")
                    });
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
                temp.Delete($"{set.MapName}.zip");
                pieces = new Dictionary<byte[], int>();
                piecesSize = 0;

                //reset for next map
                quedSetPacket = null;

                OnlineModset.OsuNetworkingHandler.SendToServer(new ImportCompletePacket());
            });
        }

        public override bool OnExiting(IScreen next)
        {
            //bit of cleanup when we exit
            OnlineModset.OsuNetworkingHandler.OnPacketReceive -= packetReceived;
            OnlineModset.OsuNetworkingHandler.Tcp = false;

            if (storage.ExistsDirectory("online/temp")) storage.DeleteDirectory("online/temp");

            if (progress != null)
                progress.State = ProgressNotificationState.Cancelled;

            return base.OnExiting(next);
        }
    }
}
