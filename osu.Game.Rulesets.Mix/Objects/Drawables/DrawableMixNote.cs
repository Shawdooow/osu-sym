using OpenTK.Graphics;
using osu.Game.Rulesets.Mix.Objects.Drawables.Pieces;
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Mix.Objects.Drawables
{
    public class DrawableMixNote : DrawableMixHitObject
    {
        public DrawableMixNote(MixNote note) : base(note)
        {
            Size = new OpenTK.Vector2(64);

            Origin = Anchor.Centre;

            Child = new Note(Color4.Red);
        }

        public override bool OnPressed(MixAction action)
        {
            return false;
        }
    }
}
