using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using Symcol.Core.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class PlayfieldDust : ParticleContainer
    {
        public override Drawable[] GetParticles() => new Drawable[]
        {
            new DustParticle()
        };

        public PlayfieldDust()
        {
            RelativeSizeAxes = Axes.Both;

            FadeInDuration = 4000;

            MotionType = MotionType.Top;
            Speed = 0.1f;
            Distance = 820;

            FadeOutDuration = 4000;
        }

        private class DustParticle : SymcolCircularContainer
        {
            public override bool HandleMouseInput => false;
            public override bool HandleKeyboardInput => false;

            internal DustParticle()
            {
                Masking = true;
                Size = new Vector2(6);

                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0.2f
                };
            }
        }
    }
}
