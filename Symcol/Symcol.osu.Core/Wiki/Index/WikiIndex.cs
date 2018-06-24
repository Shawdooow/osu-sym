using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using OpenTK;
using Symcol.osu.Core.Wiki.OverlayPieces;

namespace Symcol.osu.Core.Wiki.Index
{
    public class WikiIndex : OsuScrollContainer
    {
        public readonly Bindable<WikiSet> CurrentWikiSet = new Bindable<WikiSet>();

        private FillFlowContainer<WikiClickableOsuSpriteText> selectableWikis;

        public WikiIndex()
        {
            Anchor = Anchor.BottomLeft;
            Origin = Anchor.BottomLeft;

            ScrollbarAnchor = Anchor.TopLeft;
            RelativeSizeAxes = Axes.Y;
            Size = new Vector2(180, 0.5f);

            CurrentWikiSet.ValueChanged += value =>
            {
                WikiSetStore.ReloadWikiSets();
                ReloadOptions();
            };
            CurrentWikiSet.TriggerChange();
        }

        public void ReloadOptions()
        {
            if (selectableWikis != null)
            {
                Remove(selectableWikis);
                selectableWikis.Dispose();
            }

            Add(selectableWikis = new FillFlowContainer<WikiClickableOsuSpriteText>
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y
            });

            foreach (WikiSet set in WikiSetStore.LoadedWikiSets)
            {
                WikiClickableOsuSpriteText button = new WikiClickableOsuSpriteText
                {
                    Text = set.Name,
                    TextSize = 16,
                };
                selectableWikis.Add(button);
                button.Action = () => { CurrentWikiSet.Value = set; };
            }
        }
    }
}
