using Symcol.Core.Networking;

namespace Symcol.osu.Mods.Multi.Networking
{
    public class OsuNetworkingClientHandler : NetworkingClientHandler
    {
        protected override string Gamekey => "osu";
    }
}
