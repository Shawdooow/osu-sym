#region usings

using System.Collections.Generic;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Settings.Options;

#endregion

namespace osu.Mods.Online.Multi.Rulesets
{
    public interface IRulesetMulti
    {
        DrawableRuleset CreateRulesetContainerMulti(WorkingBeatmap beatmap, IReadOnlyList<Mod> mods, OsuNetworkingHandler networking, MatchInfo match);

        Container<MultiplayerOption> RulesetSettings(OsuNetworkingHandler networking);
    }
}
