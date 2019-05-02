#region usings

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Vitaru.Ruleset.Edit.Blueprints.ClusterBlueprints;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Edit
{
    public class VitaruHitObjectComposer : HitObjectComposer<VitaruHitObject>
    {
        public VitaruHitObjectComposer(VitaruRuleset ruleset)
            : base(ruleset)
        {
        }

        protected override DrawableRuleset<VitaruHitObject> CreateDrawableRuleset(Rulesets.Ruleset ruleset, WorkingBeatmap beatmap, IReadOnlyList<Mod> mods) => new VitaruEditRulesetContainer(ruleset, beatmap, mods);

        protected override IReadOnlyList<HitObjectCompositionTool> CompositionTools => new HitObjectCompositionTool[]
        {
            new ClusterCompositionTool()
        };

        //protected override Container CreateLayerContainer() => new PlayfieldAdjustmentContainer { RelativeSizeAxes = Axes.Both };

        public override SelectionBlueprint CreateBlueprintFor(DrawableHitObject hitObject)
        {
            switch (hitObject)
            {
                case DrawableCluster circle:
                    return new ClusterSelectionBlueprint(circle);
            }

            return base.CreateBlueprintFor(hitObject);
        }
    }
}
