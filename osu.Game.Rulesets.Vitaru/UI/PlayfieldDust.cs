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
            internal DustParticle()
            {
                Masking = true;
                Alpha = 0.5f;

                Size = new Vector2(8);

                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both
                };
            }
        }
    }
}
