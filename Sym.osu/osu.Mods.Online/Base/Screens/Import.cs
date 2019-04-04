using osu.Core.Screens.Evast;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
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
            temp = this.storage.GetStorageForDirectory("online/temp");
        }

        private void import()
        {

        }

        private void packetReceived(PacketInfo info)
        {
            if (info.Packet is MapSetPacket set)
            {
                
            }
        }

        public override bool OnExiting(IScreen next)
        {
            OnlineModset.OsuNetworkingHandler.OnPacketReceive -= packetReceived;
            if (storage.ExistsDirectory("online/temp")) storage.DeleteDirectory("online/temp");
            return base.OnExiting(next);
        }
    }
}
