using System;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Screens;
using osu.Game.Screens.Select;
using Symcol.Rulesets.Core.LegacyMultiplayer.Networking;

namespace Symcol.Rulesets.Core.LegacyMultiplayer.Screens
{
    public class MatchSongSelect : SongSelect
    {
        private bool exiting;

        protected override BackgroundScreen CreateBackground() => null;

        public Action<WorkingBeatmap> SelectionFinalised;

        public readonly RulesetNetworkingClientHandler RulesetNetworkingClientHandler;

        public MatchSongSelect(RulesetNetworkingClientHandler rulesetNetworkingClientHandler)
        {
            RulesetNetworkingClientHandler = rulesetNetworkingClientHandler;
        }

        protected override void OnEntering(Screen last)
        {
            Add(RulesetNetworkingClientHandler);
            base.OnEntering(last);
        }

        protected override bool OnStart()
        {
            if (!exiting)
            {
                SelectionFinalised(Beatmap.Value);
                exiting = true;
                Remove(RulesetNetworkingClientHandler);
                Exit();
            }
            return true;
        }
    }
}
