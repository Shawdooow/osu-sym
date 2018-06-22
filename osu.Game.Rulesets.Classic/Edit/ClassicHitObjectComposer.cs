// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Classic.Objects;

namespace osu.Game.Rulesets.Classic.Edit
{
    public class ClassicHitObjectComposer : HitObjectComposer
    {
        public ClassicHitObjectComposer(Ruleset ruleset)
            : base(ruleset)
        {
        }

        protected override RulesetContainer CreateRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap) => new ClassicEditRulesetContainer(ruleset, beatmap);

        protected override IReadOnlyList<ICompositionTool> CompositionTools => new ICompositionTool[]
        {
            new HitObjectCompositionTool<HitCircle>(),
            new HitObjectCompositionTool<Slider>(),
            new HitObjectCompositionTool<Hold>(),
            new HitObjectCompositionTool<Spinner>()
            
        };
    }
}
