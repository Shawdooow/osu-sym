// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Beatmaps;
using osu.Game.Rulesets.Classic.UI;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Classic.Edit
{
    public class ClassicEditRulesetContainer : ClassicRulesetContainer
    {
        public ClassicEditRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap, bool isForCurrentRuleset)
            : base(ruleset, beatmap, isForCurrentRuleset)
        {
        }

        protected override Playfield CreatePlayfield() => new ClassicEditPlayfield();
    }
}
