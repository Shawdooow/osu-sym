using System.Diagnostics;
using osu.Framework.Graphics;
using osu.Framework.MathUtils;
using OpenTK;

namespace Symcol.Core.Graphics.Containers
{
    public abstract class ParticleContainer : SymcolContainer
    {
        public MotionType MotionType;

        public abstract Drawable[] GetParticles();

        /// <summary>
        /// How many particles we expect to spawn per second on average
        /// </summary>
        public double Spawnrate = 10;

        #region FadeIn

        public Easing FadeInEasing;

        public double FadeInDuration = 100;

        #endregion

        /// <summary>
        /// Distance we will move before death
        /// </summary>
        public double Distance;

        /// <summary>
        /// Particle Movement Easing
        /// </summary>
        public Easing Easing;

        /// <summary>
        /// Multiplier to cross Distance where default (1) is one second
        /// </summary>
        public double Speed = 1;

        #region FadeOut

        public Easing FadeOutEasing;

        public double FadeOutDuration = 100;

        #endregion

        /// <summary>
        /// How much the particle's scale can randomly vary
        /// </summary>
        public Vector2 ScaleVariance = Vector2.Zero;

        /// <summary>
        /// How much the particle's rotation can randomly vary
        /// </summary>
        public double RotationVariance = 0;

        /// <summary>
        /// Whether particles will actively rotate one spawned
        /// </summary>
        public bool ActivelyRotate;

        /// <summary>
        /// How much the particle's active rotation can randomly vary
        /// </summary>
        public double ActiveRotationVariance = 0;

        /// <summary>
        /// How much the particle's movement can randomly vary
        /// </summary>
        public Vector2 MovementVariance = Vector2.Zero;

        protected override void Update()
        {
            base.Update();

            if (RNG.NextDouble(0, (float)Clock.ElapsedFrameTime / 1000) >= Spawnrate)
                Spawn();
        }

        /// <summary>
        /// Spawns a "Particle"
        /// </summary>
        protected virtual void Spawn()
        {
            Drawable particle = GetParticles()[RNG.Next(0, GetParticles().Length)];

            switch (MotionType)
            {
                case MotionType.Radial:
                    AddParticle(particle, Anchor.Centre, Vector2.Zero);
                    break;
                case MotionType.Top:
                    AddParticle(particle, Anchor.TopLeft, new Vector2(0, DrawSize.X));
                    particle.FadeInFromZero(FadeInDuration, FadeInEasing)
                            .MoveTo(new Vector2(0, (float)Distance), 1000 * Speed, Easing)
                            .Delay(1000 * Speed - FadeOutDuration)
                            .FadeOut(FadeOutDuration, FadeOutEasing)
                            .Delay(FadeOutDuration)
                            .Expire();
                    break;
                case MotionType.Bottom:
                    AddParticle(particle, Anchor.BottomLeft, new Vector2(0, DrawSize.X));
                    break;
                case MotionType.Left:
                    AddParticle(particle, Anchor.TopLeft, new Vector2(0, DrawSize.Y));
                    break;
                case MotionType.Right:
                    AddParticle(particle, Anchor.TopRight, new Vector2(0, DrawSize.Y));
                    break;
            }
        }

        protected virtual void AddParticle(Drawable particle, Anchor anchor, Vector2 spawns)
        {
            particle.Anchor = anchor;

            Add(particle);
        }
    }

    public enum MotionType
    {
        Radial,
        Top,
        Bottom,
        Left,
        Right,
    }
}
