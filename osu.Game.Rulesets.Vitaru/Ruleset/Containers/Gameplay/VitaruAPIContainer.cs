using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Online.API;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay
{
    public class VitaruAPIContainer : Container, IOnlineComponent
    {
        public static bool Admin => Shawdooow;

        public static bool Shawdooow;

        [BackgroundDependencyLoader]
        private void load(APIAccess api)
        {
            api.Register(this);
        }

        public void APIStateChanged(APIAccess api, APIState state)
        {
            switch (state)
            {
                case APIState.Online:
                    Shawdooow = api.LocalUser.Value.Username == "Shawdooow";
                    break;
            }
        }
    }
}
