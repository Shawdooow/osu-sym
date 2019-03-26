#region usings

using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.ChapterSets;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;
using Sym.Base.Graphics.Containers;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields
{
    public class AspectLockedPlayfield : AspectLockedContainer
    {
        private readonly ChapterSet chapterSet = ChapterStore.GetChapterSet(VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Gamemode));

        public override Vector2 Size => chapterSet.PlayfieldSize;

        public override Vector2 AspectRatio => chapterSet.PlayfieldAspectRatio;

        public AspectLockedPlayfield()
        {
            Margin = chapterSet.PlayfieldMargin;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            Scale = new Vector2(Parent.DrawSize.Y * AspectRatio.X / AspectRatio.Y / Size.X, Parent.DrawSize.Y / Size.Y) * Margin;
        }
    }
}
