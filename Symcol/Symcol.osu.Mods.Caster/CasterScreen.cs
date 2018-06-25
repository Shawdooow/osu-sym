using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Game.Screens;
using OpenTK;
using Symcol.Core.Graphics.Containers;

namespace Symcol.osu.Mods.Caster
{
    public class CasterScreen : OsuScreen
    {
        public readonly Bindable<SelectedScreen> SelectedScreen = new Bindable<SelectedScreen>();

        public CasterScreen()
        {
            Children = new Drawable[]
            {
                new CasterSideBar(),
                new SymcolContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.8f, 0.98f)
                }
            };
        }
    }

    public enum SelectedScreen
    {
        Teams,
        Maps,
        Results
    }
}
