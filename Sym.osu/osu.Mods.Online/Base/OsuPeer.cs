#region usings

using System.Net;
using Sym.Networking.NetworkingHandlers.Server;

#endregion

namespace osu.Mods.Online.Base
{
    public class OsuPeer : Peer
    {
        public OsuUser User;

        public OsuPeer(IPEndPoint end)
            : base(end)
        {
        }
    }
}
