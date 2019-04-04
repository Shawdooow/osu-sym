#region usings

using System;
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
using osu.Game.Overlays.Settings;
using osu.Mods.Online.Base.Packets;
using osuTK;
using osuTK.Graphics;
using Sym.Networking.Packets;

#endregion

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

        private void import()
        {
            OnlineModset.OsuNetworkingHandler.SendToServer(new ImportPacket());
        }

        private void packetReceived(PacketInfo info)
        {
            switch (info.Packet)
            {
                case MapSetPacket mapSetPacket:
                    import(mapSetPacket);
                    break;
            }
        }

        private void import(MapSetPacket set)
        {
            StreamWriter writer = new StreamWriter(temp.GetStream($"{set.MapName}.zip", FileAccess.ReadWrite, FileMode.Create));

            Task.Factory.StartNew(() =>
            {
                try
                {
                    //writer.Write(Encoding.UTF8.GetBytes(reader.ReadToEnd()));
                    writer.Close();

                    string serial = String.Empty;

                    foreach (byte[] ba in OnlineModset.OsuNetworkingHandler.Pieces)
                        serial = serial + Convert.ToBase64String(ba);

                    File.WriteAllBytes(temp.GetFullPath($"{set.MapName}.zip"), Convert.FromBase64String(serial));

                    ZipFile.ExtractToDirectory(temp.GetFullPath($"{set.MapName}.zip"), temp.GetFullPath($"{set.MapName}"), Encoding.UTF8);
                    //temp.Delete($"{set.MapName}.zip");
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
