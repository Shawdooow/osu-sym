#region usings

using osu.Mods.Online.Base;
using Sym.Networking.Containers;

#endregion

namespace osu.Mods.Online.Multi
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
