using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.UI;
using osu.Mods.Online.Base;

namespace osu.Mods.Online.Multi.Rulesets
{
    public interface IRulesetMulti
    {
        RulesetContainer CreateRulesetContainerMulti(WorkingBeatmap beatmap, OsuNetworkingHandler networking);

        Container RulesetSettings(OsuNetworkingHandler networking);
    }
}
