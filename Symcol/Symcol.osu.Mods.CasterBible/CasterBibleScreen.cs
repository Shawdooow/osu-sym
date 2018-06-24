using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Screens;
using Symcol.Core.Graphics.Containers;

namespace Symcol.osu.Mods.CasterBible
{
    public class CasterBibleScreen : OsuScreen
    {
        public readonly Bindable<SelectedScreen> SelectedScreen = new Bindable<SelectedScreen>();

        protected sealed override Container<Drawable> Content => content;

        private readonly SymcolContainer content;

        public CasterBibleScreen()
        {
            content = new SymcolContainer
            {
                RelativeSizeAxes = Axes.Both,
                Width = 3,
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
