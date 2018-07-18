using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Framework.MathUtils;
using OpenTK;
using Symcol.Core.Graphics.Containers;

namespace Symcol.Core.Graphics.Particles
{
    public class ParticleContainer : SymcolContainer
    {
        public MotionType MotionType;

        public Drawable Particle
        {
            get => Particles.First();
            set => Particles = new List<Drawable> { value };
        }

        public List<Drawable> Particles = new List<Drawable>();

        /// <summary>
        /// How many particles we expect to spawn per second on average
        /// </summary>
        public double Spawnrate = 10;

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

            double r = RNG.NextDouble(0, (float)Clock.ElapsedFrameTime / 1000);

            if (r >= Spawnrate)
                Spawn();
        }

        /// <summary>
        /// Spawns a "Particle"
        /// </summary>
        protected void Spawn()
        {
            Drawable particle = Particle;

            if (Particles.Count > 1)
            {
                int r = RNG.Next(0, Particles.Count);
                particle = Particles[r];
            }
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
