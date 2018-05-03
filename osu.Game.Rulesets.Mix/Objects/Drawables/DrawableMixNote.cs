using OpenTK.Graphics;
using osu.Game.Rulesets.Mix.Objects.Drawables.Pieces;

namespace osu.Game.Rulesets.Mix.Objects.Drawables
{
    public class DrawableMixNote : DrawableMixHitObject<MixNote>
    {
        public DrawableMixNote(MixHitObject hitObject) : base(hitObject)
        {
            Size = new OpenTK.Vector2(64);

            Child = new Note(Color4.Red);
        }

        public override bool OnPressed(MixAction action)
        {
            throw new System.NotImplementedException();
        }
    }
}
