using osu.Framework.Graphics;
using osu.Game.Screens;
using OpenTK;
using OpenTK.Graphics;
using Symcol.osu.Core.Containers.Shawdooow;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Mods.Multi.Screens;

namespace Symcol.osu.Mods.Multi
{
    public class MultiModset : SymcolModSet
    {
        public override SymcolButton GetMenuButton() => new SymcolButton
        {
            ButtonName = "Multi",
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            ButtonColorTop = Color4.Blue,
            ButtonColorBottom = Color4.Red,
            ButtonSize = 100,
            ButtonPosition = new Vector2(10, -220),
        };

        public override OsuScreen GetScreen() => new ConnectToServer();
    }
}
