using osu.Framework.Screens;
using Symcol.osu.Core.Screens.Evast;
using Symcol.osu.Mods.Multi.Networking;

namespace Symcol.osu.Mods.Multi.Screens
{
    public class MultiScreen : BeatmapScreen
    {
        public readonly OsuNetworkingClientHandler OsuNetworkingClientHandler;

        public MultiScreen(OsuNetworkingClientHandler osuNetworkingClientHandler)
        {
            OsuNetworkingClientHandler = osuNetworkingClientHandler;
        }

        protected override void OnEntering(Screen last)
        {
            Add(OsuNetworkingClientHandler);
            base.OnEntering(last);
        }

        protected override void OnSuspending(Screen next)
        {
            Remove(OsuNetworkingClientHandler);
            base.OnSuspending(next);
        }

        protected override void OnResuming(Screen last)
        {
            Add(OsuNetworkingClientHandler);
            base.OnResuming(last);
        }

        protected override bool OnExiting(Screen next)
        {
            Remove(OsuNetworkingClientHandler);
            return base.OnExiting(next);
        }
    }
}
