using osu.Framework.Graphics.Cursor;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Edit
{
    public class VitaruEditPlayfield : VitaruPlayfield
    {
        public override bool LoadPlayer => false;

        //public override bool ProvidingUserCursor => false;

        protected override CursorContainer CreateCursor() => null;

        public VitaruEditPlayfield(VitaruInputManager vitaruInput) : base (vitaruInput)
        {
        }
    }
}
