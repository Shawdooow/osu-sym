using osu.Framework.Graphics;
using OpenTK;
using Symcol.Core.Graphics.Containers;
using Symcol.osu.Core.Screens.Evast;
using Symcol.osu.Mods.Caster.CasterScreens;
using Symcol.osu.Mods.Caster.Pieces;

namespace Symcol.osu.Mods.Caster
{
    public class CasterScreen : BeatmapScreen
    {
        protected override bool HideOverlaysOnEnter => true;

        private readonly CasterToolbar toolbar;
        private readonly CasterControlPanel casterPanel;

        private readonly SymcolContainer screenSpace;

        public CasterScreen()
        {
            Children = new Drawable[]
            {
                toolbar = new CasterToolbar(),
                casterPanel = new CasterControlPanel(),
                screenSpace = new SymcolContainer
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,

                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Position = new Vector2(-10),
                    Size = new Vector2(0.8f, 0.8f),
                }
            };

            toolbar.CurrentBibleScreen.ValueChanged += value =>
            {
                switch (value)
                {
                    case SelectedScreen.Maps:
                        screenSpace.Child = new Maps(casterPanel);
                        break;
                    case SelectedScreen.Results:
                        screenSpace.Child = new Results(casterPanel);
                        break;
                    case SelectedScreen.Teams:
                        screenSpace.Child = new Teams(casterPanel);
                        break;
                }
            };
            toolbar.CurrentBibleScreen.TriggerChange();
        }
    }

    public enum SelectedScreen
    {
        Maps,
        Results,
        Teams,
    }
}
