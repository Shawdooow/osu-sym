using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
using osu.Game.Overlays.Settings;
using osu.Mods.Online.Base.Packets;
using osuTK;
using osuTK.Graphics;
using Sym.Networking.NetworkingClients;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Base.Screens
{
    public class Import : BeatmapScreen
    {
        private BeatmapManager manager;

        private Storage storage;

        private Storage temp;

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
        private void load(BeatmapManager manager, Storage storage)
        {
            this.manager = manager;
            this.storage = storage;
            temp = this.storage.GetStorageForDirectory("online\\temp");
        }

        private MapSetPacket quedSetPacket;

        private int piecesSize = 0;
        private Dictionary<byte[], int> pieces = new Dictionary<byte[], int>();

        protected override void Update()
        {
            base.Update();

            if (OnlineModset.OsuNetworkingHandler.Tcp && OnlineModset.OsuNetworkingHandler.TcpNetworkingClient.Available > 0)
                fetch();

            if (quedSetPacket != null && piecesSize == quedSetPacket.Size)
            {
                import(quedSetPacket);
                quedSetPacket = null;
            }
        }

        private void fetch()
        {
            byte[] data = new byte[TcpNetworkingClient.BUFFER_SIZE / 8];
            int count = OnlineModset.OsuNetworkingHandler.TcpNetworkStream.Read(data, 0, data.Length);
            piecesSize += count;
            pieces.Add(data, count);
        }

        private void import()
        {
            OnlineModset.OsuNetworkingHandler.SendToServer(new ImportPacket());
        }

        private void packetReceived(PacketInfo info)
        {
            switch (info.Packet)
            {
                case MapSetPacket mapSetPacket:
                    quedSetPacket = mapSetPacket;
                    break;
            }
        }

        private void import(MapSetPacket set)
        {
            StreamWriter writer = new StreamWriter(temp.GetStream($"{set.MapName}.zip", FileAccess.ReadWrite, FileMode.Create));
            StreamReader reader = new StreamReader(OnlineModset.OsuNetworkingHandler.TcpNetworkStream);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    writer.Close();

                    using (FileStream fs = new FileStream(temp.GetFullPath($"{set.MapName}.zip"), FileMode.Create, FileAccess.Write))
                    {
                        foreach (KeyValuePair<byte[], int> pair in pieces)
                            fs.Write(pair.Key, 0, pair.Value);
                    }

                    reader.Close();

                    ZipFile.ExtractToDirectory(temp.GetFullPath($"{set.MapName}.zip"), temp.GetFullPath($"{set.MapName}"), Encoding.UTF8);

                    temp.Delete($"{set.MapName}.zip");
                    pieces = new Dictionary<byte[], int>();
                    piecesSize = 0;
                }
                catch (Exception e) { Logger.Error(e, "Failed to receive map!", LoggingTarget.Network); }
            }, TaskCreationOptions.LongRunning).ContinueWith(result =>
            {
                manager.Import(new[]
                {
                    temp.GetFullPath($"{set.MapName}")
                });
            });
        }

        public override bool OnExiting(IScreen next)
        {
            OnlineModset.OsuNetworkingHandler.OnPacketReceive -= packetReceived;
            OnlineModset.OsuNetworkingHandler.Tcp = false;
            if (storage.ExistsDirectory("online/temp")) storage.DeleteDirectory("online/temp");
            return base.OnExiting(next);
        }
    }
}
