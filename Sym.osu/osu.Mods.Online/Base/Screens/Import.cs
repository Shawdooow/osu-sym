using System.Collections.Generic;
using System.IO;
using System.Linq;
using osu.Core.Screens.Evast;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Overlays.Settings;
using osu.Mods.Online.Base.Packets;
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
            Child = new SettingsButton
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.X,
                Width = 0.3f,
                Text = "Import from Host's Stable Install",
                Action = import
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

        private readonly List<MapSetPiecePacket> pieces = new List<MapSetPiecePacket>();

        private void packetReceived(PacketInfo info)
        {
            switch (info.Packet)
            {
                case MapSetPacket mapSetPacket:
                    import(mapSetPacket);
                    break;
                case MapSetPiecePacket mapSetPiecePacket:
                    pieces.Add(mapSetPiecePacket);
                    break;
            }
        }

        private void import(MapSetPacket set)
        {
            StreamWriter writer = new StreamWriter(temp.GetStream($"{set.MapName}.zip", FileAccess.ReadWrite, FileMode.Create));

            List<MapSetPiecePacket> ps = new List<MapSetPiecePacket>();

            for (int i = 0; i < pieces.Count; i++)
                if (pieces[i].MapName == set.MapName && pieces[i].Piece > ps.Last().Piece)
                {
                    ps.Add(pieces[i]);
                    pieces.Remove(pieces[i]);
                    i = -1;
                }

            if (ps.Count != set.Pieces)
            {
                Logger.Log($"Pieces of {set.MapName} missing!", LoggingTarget.Network, LogLevel.Error);
                return;
            }

            foreach (MapSetPiecePacket p in ps)
                writer.Write(p.ZipSerial);

            writer.Close();

            manager.Import(new[]
            {
                temp.GetFullPath($"{set.MapName}.zip")
            });
        }

        public override bool OnExiting(IScreen next)
        {
            OnlineModset.OsuNetworkingHandler.OnPacketReceive -= packetReceived;
            if (storage.ExistsDirectory("online/temp")) storage.DeleteDirectory("online/temp");
            return base.OnExiting(next);
        }
    }
}
