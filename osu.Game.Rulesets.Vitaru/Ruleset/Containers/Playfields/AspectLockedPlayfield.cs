#region usings

using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Mods.ChapterSets;
using osu.Game.Rulesets.Vitaru.Mods.Gamemodes;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;
using Sym.Base.Graphics.Containers;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields
{
    public class AspectLockedPlayfield : AspectLockedContainer
    {
        private readonly VitaruGamemode gamemode = ChapterStore.GetGamemode(VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Gamemode));

        public override Vector2 Size => gamemode.PlayfieldSize;

        public override Vector2 AspectRatio => gamemode.PlayfieldAspectRatio;

        public AspectLockedPlayfield()
        {
            Margin = gamemode.PlayfieldMargin;
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
