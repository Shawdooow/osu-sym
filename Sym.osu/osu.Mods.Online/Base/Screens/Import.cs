#region usings

using osu.Core.Containers.Shawdooow;
using osu.Core.Screens.Evast;
using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Game.Overlays.Settings;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Mods.Online.Base.Screens
{
    public class Import : BeatmapScreen
    {
        public Import()
        {
            Children = new Drawable[]
            {
                new SettingsButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Import from Host's Stable Install",
                    Action = OnlineModset.OsuNetworkingHandler.Import
                },
                new SymcolButton
                {
                    ButtonText = "Back",
                    Origin = Anchor.BottomCentre,
                    Anchor = Anchor.BottomCentre,
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    Size = 50,
                    Action = this.Exit,
                    Position = new Vector2(0 , -10),
                },
                new SettingsButton
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Toggle TCP",
                    Action = () => { OnlineModset.OsuNetworkingHandler.Tcp = !OnlineModset.OsuNetworkingHandler.Tcp; }
                },
            };
        }
    }
}
