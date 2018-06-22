using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace Symcol.Core.Graphics.Sprites
{
    public class SymcolSprite : Sprite
    {
        public override bool HandleMouseInput => false;
        public override bool HandleKeyboardInput => false;

        private bool disposed;

        /// <summary>
        /// Delete this fucking object!
        /// </summary>
        public void Delete()
        {
            if (Parent is Container p)
                p.Remove(this);

            Dispose();
        }

        protected override void Dispose(bool isDisposing)
        {
            disposed = true;
            base.Dispose(isDisposing);
        }

        public override bool UpdateSubTree()
        {
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (disposed) return false;
            return base.UpdateSubTree();
        }
    }
}
