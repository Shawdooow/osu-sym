using osu.Core.Screens.Evast;
using osu.Framework.Screens;
using osu.Mods.Online.Base;
using Sym.Networking.Packets;

// ReSharper disable DelegateSubtraction

namespace osu.Mods.Online.Multi
{
    public class MultiScreen : BeatmapScreen
    {
        public readonly OsuNetworkingHandler OsuNetworkingHandler;

        public MultiScreen(OsuNetworkingHandler osuNetworkingHandler)
        {
            OsuNetworkingHandler = osuNetworkingHandler;
        }

        protected virtual void SendPacket(Packet packet) => OsuNetworkingHandler.SendToServer(packet);

        protected virtual void OnPacketRecieve(PacketInfo info)
        {

        }

        public override void OnEntering(IScreen last)
        {
            OsuNetworkingHandler.OnPacketReceive += OnPacketRecieve;
            base.OnEntering(last);
        }

        public override void OnSuspending(IScreen next)
        {
            OsuNetworkingHandler.OnPacketReceive -= OnPacketRecieve;
            base.OnSuspending(next);
        }

        public override void OnResuming(IScreen last)
        {
            OsuNetworkingHandler.OnPacketReceive += OnPacketRecieve;
            base.OnResuming(last);
        }

        public override bool OnExiting(IScreen next)
        {
            OsuNetworkingHandler.OnPacketReceive -= OnPacketRecieve;
            return base.OnExiting(next);
        }
    }
}
