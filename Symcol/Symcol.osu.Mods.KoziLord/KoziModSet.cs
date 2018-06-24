using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Screens;
using OpenTK;
using OpenTK.Graphics;
using Symcol.osu.Core.Containers.Shawdooow;
using Symcol.osu.Core.SymcolMods;

namespace Symcol.osu.Mods.KoziLord
{
    public class KoziModSet : SymcolModSet
    {
        public override SymcolButton GetMenuButton() => new SymcolButton
        {
            ButtonName = "Jacob's",
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            ButtonColorTop = Color4.Purple,
            ButtonColorBottom = Color4.Magenta,
            ButtonSize = 90,
            ButtonPosition = new Vector2(-180, -20),
        };

        public override OsuScreen GetMenuScreen() => new KoziScreen();
    }
}
