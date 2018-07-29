using osu.Framework.Screens;
using osu.Game.Screens;
using Symcol.osu.Mods.Multi.Networking;

namespace Symcol.osu.Mods.Multi.Screens
{
    public class MultiScreen : OsuScreen
    {
        protected static OsuNetworkingClientHandler OsuNetworkingClientHandler;

        protected override void OnEntering(Screen last)
        {
            if (OsuNetworkingClientHandler != null)
                Add(OsuNetworkingClientHandler);
            base.OnEntering(last);
        }

        protected override void OnSuspending(Screen next)
        {
            if (OsuNetworkingClientHandler != null)
                Remove(OsuNetworkingClientHandler);
            base.OnSuspending(next);
        }

        protected override void OnResuming(Screen last)
        {
            if (OsuNetworkingClientHandler != null)
                Add(OsuNetworkingClientHandler);
            base.OnResuming(last);
        }

        protected override bool OnExiting(Screen next)
        {
            if (OsuNetworkingClientHandler != null)
                Remove(OsuNetworkingClientHandler);
            return base.OnExiting(next);
        }
    }
}
