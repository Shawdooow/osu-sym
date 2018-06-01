using osu.Framework.Graphics.Containers;
using osu.Game.Audio;
using osu.Game.Skinning;

namespace Symcol.Rulesets.Core.Skinning
{
    public class SymcolSkinnableSound : SkinnableSound
    {
        public SymcolSkinnableSound(params SampleInfo[] samples) : base(samples)
        {
        }

        private bool deleted;

        public virtual void Delete()
        {
            if (Parent is Container p)
                p.Remove(this);

            deleted = true;

            Dispose();
        }
    }
}
