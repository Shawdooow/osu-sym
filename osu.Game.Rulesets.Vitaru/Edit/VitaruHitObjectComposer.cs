using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Vitaru.Edit.Layers.Selection.Overlays;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Edit
{
    public class VitaruHitObjectComposer : HitObjectComposer
    {
        public VitaruHitObjectComposer(Ruleset ruleset) : base(ruleset) { }

        protected override RulesetContainer CreateRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap) => new VitaruEditRulesetContainer(ruleset, beatmap, true);

        protected override IReadOnlyList<ICompositionTool> CompositionTools => new ICompositionTool[]
        {
            new HitObjectCompositionTool<Pattern>(),
        };

        protected override ScalableContainer CreateLayerContainer() => new ScalableContainer(VitaruPlayfield.BaseSize.X) { RelativeSizeAxes = Axes.Both };

        public override HitObjectMask CreateMaskFor(DrawableHitObject hitObject)
        {
            switch (hitObject)
            {
                case DrawablePattern pattern:
                    return new PatternMask(pattern);
            }

            return base.CreateMaskFor(hitObject);
        }
    }
}
