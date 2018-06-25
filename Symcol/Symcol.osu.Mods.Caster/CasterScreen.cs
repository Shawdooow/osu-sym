using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Game.Screens;
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
                    RelativeSizeAxes = Axes.Both

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
