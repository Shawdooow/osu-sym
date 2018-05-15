using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using System.Linq;

namespace Symcol.Core.Graphics.Sprites
{
    public class AnimatedSprite : Sprite
    {
        public Texture[] Textures { get; set; }

        /// <summary>
        /// Length of time each sprite is shown for
        /// </summary>
        public double Speed { get; set; } = 250;

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

            if (lastUpdate + Speed <= Time.Current)
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
