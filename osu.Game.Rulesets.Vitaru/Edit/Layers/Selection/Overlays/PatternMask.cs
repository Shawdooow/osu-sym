using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Game.Graphics;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Pieces;
using OpenTK;

namespace osu.Game.Rulesets.Vitaru.Edit.Layers.Selection.Overlays
{
    public class PatternMask : HitObjectMask
    {
        public PatternMask(DrawablePattern pattern)
            : base(pattern)
        {
            Origin = Anchor.Centre;

            Position = pattern.Position;
            Size = new Vector2(30);

            CornerRadius = Size.X / 2;

            AddInternal(new StarPiece());

            pattern.HitObject.PositionChanged += _ => Position = pattern.Position;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours)
        {
            Colour = colours.Yellow;
        }
    }
}
