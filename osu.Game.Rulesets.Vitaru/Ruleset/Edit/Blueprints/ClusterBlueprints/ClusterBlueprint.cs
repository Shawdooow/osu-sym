using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables.Pieces;
using osuTK;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Edit.Blueprints.ClusterBlueprints
{
    public class ClusterSelectionBlueprint : SelectionBlueprint
    {
        public ClusterSelectionBlueprint(DrawableCluster cluster)
            : base(cluster)
        {
            Origin = Anchor.Centre;

            Position = cluster.Position;
            Size = new Vector2(30);

            CornerRadius = Size.X / 2;

            AddInternal(new StarPiece());

            cluster.HitObject.PositionChanged += _ => Position = cluster.Position;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.Yellow;
        }
    }
}
