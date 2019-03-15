using osu.Mods.Online.Base;
using Sym.Networking.Containers;

namespace osu.Mods.Online.Multi.Screens.Pieces
{
    public class MultiplayerContainer : PeerNetworkingContainer
    {
        public readonly OsuNetworkingHandler OsuNetworkingHandler;

        public MultiplayerContainer(OsuNetworkingHandler osuNetworkingHandler) : base(osuNetworkingHandler)
        {
            OsuNetworkingHandler = osuNetworkingHandler;
        }
    }
}
