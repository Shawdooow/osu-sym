using osu.Framework.Graphics.Textures;
using System;
using System.Linq;

namespace Symcol.Core.Graphics.Sprites
{
    /// <summary>
    /// A Sprite that will cycle through multiple Textures after a fixed time (Speed)
    /// </summary>
    public class AnimatedSprite : SymcolSprite
    {
        /// <summary>
        /// The list of Textures we will cycle through
        /// </summary>
        public Texture[] Textures { get; set; }

        /// <summary>
        /// Length of time each sprite is shown for
        /// </summary>
        public double UpdateRate { get; set; } = 250;

        /// <summary>
        /// Play animation in reverse when reaching end rather than cycle back to the first frame
        /// </summary>
        public bool ReverseOnComplete { get; set; }

        /// <summary>
        /// Called when we cycle back to the first texture (technically just before it by one line) if !ReverseOnComplete
        /// </summary>
        public Action OnAnimationRestart;

        private double lastUpdate = double.MaxValue;

        /// <summary>
        /// Reset to first Texture
        /// </summary>
        public void Reset()
        {
            Texture = Textures.First();
            lastUpdate = Time.Current;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Texture = Textures.First();
            lastUpdate = Time.Current;
        }

        private bool reversing;

        protected override void Update()
        {
            base.Update();

            if (lastUpdate + UpdateRate <= Time.Current)
            {
                for (int i = 0; i < Textures.Length; i++)
                {
                    if (Texture == Textures[i] && Textures[i] == Textures.Last())
                    {
                        if (!ReverseOnComplete)
                        {
                            Texture = Textures.First();
                            OnAnimationRestart?.Invoke();
                        }
                        else
                        {
                            Texture = Textures[i - 1];
                            reversing = true;
                        }
                        lastUpdate = Time.Current;
                        break;
                    }

                    if (Texture == Textures[i])
                    {
                        if (ReverseOnComplete && Texture != Textures.First() && reversing)
                            Texture = Textures[i - 1];
                        else
                        {
                            Texture = Textures[i + 1];
                            reversing = true;
                        }

                        lastUpdate = Time.Current;
                        break;
                    }
                }
            }
                
        }
    }
}
