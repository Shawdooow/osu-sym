using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.MathUtils;
using osu.Game.Rulesets.Vitaru.Ruleset.Scoring;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay
{
    public class ComboFire : Container
    {
        /// <summary>
        /// Burns with passion!
        /// </summary>
        public virtual int MaxCombo => 1000;

        public virtual Color4 PassionColorDark => Color4.Blue;

        public virtual Color4 PassionColorLight => Color4.Cyan;

        /// <summary>
        /// Starts to burn
        /// </summary>
        public virtual int MinCombo => 100;

        public virtual Color4 ColorDark => Color4.Red;

        public virtual Color4 ColorLight => Color4.Yellow;

        public virtual int Combo => VitaruScoreProcessor.ComboCount;

        public ComboFire()
        {
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;
            Height = 200;

            RelativeSizeAxes = Axes.X;
            Masking = false;
        }

        protected override void Update()
        {
            base.Update();

            float spawn = 0;

            if (0 <= (float)Clock.ElapsedFrameTime / 1000 * Combo)
                spawn = (float)RNG.NextDouble(0, (float)Clock.ElapsedFrameTime / 1000 * Math.Max(Combo - MinCombo, 0));

            if (spawn > 1f)
                addFireParticle();
        }

        private void addFireParticle()
        {
            Vector2 size = new Vector2((float)RNG.NextDouble(40, 120));

            Add(new FireParticle(RelativeToAbsoluteFactor.X)
            {
                Size = size,
                Position = new Vector2((float)RNG.NextDouble(0, RelativeToAbsoluteFactor.X), size.Y),
                Colour = Interpolation.ValueAt(RNG.NextSingle(), Combo < MaxCombo ? ColorDark : PassionColorDark, Combo < MaxCombo ? ColorLight : PassionColorLight, 0, 1),
                Anchor = Anchor.BottomLeft,
                Origin = Anchor.Centre
            });
        }

        private class FireParticle : Triangle
        {
            private readonly float randomMovementYValue;
            private readonly float randomMovementXValue;
            private readonly float width;

            public FireParticle(float width)
            {
                this.width = width;

                AlwaysPresent = true;

                randomMovementYValue = -1 * ((float)RNG.NextDouble(10, 40) * 2);
                randomMovementXValue = (float)RNG.NextDouble(-10, 10);
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                float randomScaleValue = (float)RNG.NextDouble(50, 100) / 250;
                Scale = new Vector2(randomScaleValue);
                this.ScaleTo(Vector2.Zero, 2000, Easing.InQuad);
            }

            protected override void Update()
            {
                base.Update();

                Position = Position + new Vector2(randomMovementXValue * ((float)Clock.ElapsedFrameTime / 1000), randomMovementYValue * ((float)Clock.ElapsedFrameTime / 1000));

                if (Position.X > width + 60 || Scale.X <= 0.01f || Position.X < -60)
                {
                    Container parent = (Container)Parent;
                    parent.Remove(this);
                    Dispose();
                }
            }
        }
    }
}
