using osu.Framework.Graphics;
using osu.Game.Screens;
using OpenTK;
using OpenTK.Graphics;
using Symcol.osu.Core.Containers.Shawdooow;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Mods.Multi.Screens;

namespace Symcol.osu.Mods.Multi
{
    public sealed class MultiModset : SymcolModSet
    {
        public override SymcolButton GetMenuButton() => new SymcolButton
        {
            ButtonName = "Multi",
            ButtonFontSizeMultiplier = 0.8f,
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            ButtonColorTop = Color4.Blue,
            ButtonColorBottom = Color4.Red,
            ButtonSize = 140,
            ButtonPosition = new Vector2(-20, -180),
        };

        public override OsuScreen GetMenuScreen() => new Lobby();
    }
}
