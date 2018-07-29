using osu.Game.Configuration;
using osu.Game.Online.API;

namespace Symcol.osu.Mods.SymcolServer
{
    public sealed class SymcolAPIAccess : APIAccess
    {
        public override string FallbackEndpoint => "10.0.0.25:25570";

        public SymcolAPIAccess(OsuConfigManager config)
            : base(config)
        {
        }
    }
}
