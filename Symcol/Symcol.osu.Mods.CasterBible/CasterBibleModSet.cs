using osu.Framework.Graphics;
using osu.Game.Screens;
using OpenTK;
using OpenTK.Graphics;
using Symcol.osu.Core.Containers.Shawdooow;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Core.Wiki;
using Symcol.osu.Mods.CasterBible.Wiki;

namespace Symcol.osu.Mods.CasterBible
{
    public class CasterBibleModSet : SymcolModSet
    {
        public override SymcolButton GetMenuButton() => new SymcolButton
        {
            ButtonName = "Caster Bible",
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            ButtonColorTop = Color4.Yellow,
            ButtonColorBottom = Color4.Green,
            ButtonSize = 100,
            ButtonPosition = new Vector2(40, -200),
        };

        public override OsuScreen GetMenuScreen() => new CasterBibleScreen();

        public override WikiSet GetWikiSet() => new CasterWikiSet();
    }
}
