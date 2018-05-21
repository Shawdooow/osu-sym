using osu.Framework.Graphics.Containers;

namespace Symcol.Core.Graphics.Containers
{
    public class SymcolContainer : Container
    {
        public void Delete()
        {
            if (Parent is Container p)
                p.Remove(this);

            Dispose();
        }
    }
}
