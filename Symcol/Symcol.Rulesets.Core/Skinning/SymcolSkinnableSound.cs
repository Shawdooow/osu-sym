using osu.Framework.Audio;
using osu.Framework.Graphics.Containers;
using osu.Game.Audio;
using osu.Game.Skinning;

namespace Symcol.Rulesets.Core.Skinning
{
    public class SymcolSkinnableSound : SkinnableSound
    {
        public AudioManager RulesetAudio;

        public override bool HandleMouseInput => false;
        public override bool HandleKeyboardInput => false;

        public SymcolSkinnableSound(params SampleInfo[] samples) : base(samples)
        {
            Name = "SymcolSkinnableSound";
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            LoadAsyncComplete();
        }

        private bool deleted;

        public virtual void Delete()
        {
            if (Parent is Container p)
                p.Remove(this);

            deleted = true;

            Dispose();
        }

        protected override void Dispose(bool isDisposing)
        {
            deleted = true;
            base.Dispose(isDisposing);
        }

        public override bool UpdateSubTree()
        {
            if (!deleted)
                return base.UpdateSubTree();
            return false;
        }
    }
}
