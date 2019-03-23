#region usings

using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Events;

#endregion

namespace osu.Mods.Online.Multi.Rulesets
{
    public class MultiCursorContainer : CursorContainer
    {
        public bool Slave { get; set; }

        public virtual MultiCursorContainer CreateMultiCursor() => null;

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if (Slave) return false;
            return base.OnMouseMove(e);
        }
    }
}
