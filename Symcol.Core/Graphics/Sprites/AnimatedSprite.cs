using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using System.Linq;

namespace Symcol.Core.Graphics.Sprites
{
    /// <summary>
    /// A Sprite that will cycle through multiple Textures after a fixed time (Speed)
    /// </summary>
    public class AnimatedSprite : Sprite
    {
        /// <summary>
        /// The list of Textures we will cycle through
        /// </summary>
        public Texture[] Textures { get; set; }

        /// <summary>
        /// Length of time each sprite is shown for
        /// </summary>
        public double UpdateRate { get; set; } = 250;

        private double lastUpdate = double.MaxValue;

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Texture = Textures.First();
            lastUpdate = Time.Current;
        }

        protected override void Update()
        {
            base.Update();

            if (lastUpdate + UpdateRate <= Time.Current)
            {
                bool next = false;
                foreach (Texture texture in Textures)
                {
                    if (Texture == texture)
                        next = true;
                    else if (next)
                    {
                        Texture = texture;
                        lastUpdate = Time.Current;
                    }

                    //If this texture is last better cycle back to the first!
                    if (Texture == texture && texture == Textures.Last())
                    {
                        Texture = Textures.First();
                        lastUpdate = Time.Current;
                    }
                }
            }
                
        }
    }
}
