#region usings

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Online.API;

#endregion

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

        public void APIStateChanged(IAPIProvider api, APIState state)
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
