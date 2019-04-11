#region usings

using osu.Framework.Input.Events;
using osu.Game.Rulesets.UI;

#endregion

namespace osu.Mods.Online.Multi.Rulesets
{
    public class MultiCursorContainer : GameplayCursorContainer
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
