using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.UI;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Settings.Options;

namespace osu.Mods.Online.Multi.Rulesets
{
    public interface IRulesetMulti
    {
        RulesetContainer CreateRulesetContainerMulti(WorkingBeatmap beatmap, OsuNetworkingHandler networking, MatchInfo match);

        Container<MultiplayerOption> RulesetSettings(OsuNetworkingHandler networking);
    }
}
