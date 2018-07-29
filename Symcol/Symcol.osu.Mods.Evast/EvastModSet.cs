using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Screens;
using OpenTK;
using Symcol.osu.Core.Containers.Shawdooow;
using Symcol.osu.Core.SymcolMods;

namespace Symcol.osu.Mods.Evast
{
    public class EvastModSet : SymcolModSet
    {
        public override SymcolButton GetMenuButton() => new SymcolButton
        {
            ButtonName = "Evast's",
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            ButtonColorTop = OsuColour.FromHex("#7b30ff"),
            ButtonColorBottom = OsuColour.FromHex("#d230ff"),
            ButtonSize = 90,
            ButtonPosition = new Vector2(20, 200),
        };

        public override OsuScreen GetScreen() => new MoreScreen();
    }
}
