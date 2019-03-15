using osu.Core.Containers.Shawdooow;
using osu.Core.Screens.Evast;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Mods.Online.Base.Screens;
using osu.Mods.Online.Multi.Screens;
using osuTK;
using osuTK.Graphics;
using Sym.Networking.NetworkingHandlers;

namespace osu.Mods.Online
{
    public class OnlineMenu : BeatmapScreen
    {
        public OnlineMenu()
        {
            Children = new Drawable[]
            {
                new SymcolButton
                {
                    ButtonText = "Multi",
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.Blue,
                    ButtonColorBottom = Color4.Red,
                    Size = 90,
                    Position = new Vector2(-80, 20),
                    Action = () =>
                    {
                        if (OnlineModset.OsuNetworkingHandler != null && OnlineModset.OsuNetworkingHandler.ConnectionStatues >= ConnectionStatues.Connected)
                            Push(new Lobby(OnlineModset.OsuNetworkingHandler));
                        else
                            Logger.Log("Connect to a server first!", LoggingTarget.Network, LogLevel.Error);
                    }
                },
                new SymcolButton
                {
                    ButtonText = "Options",
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.Green,
                    ButtonColorBottom = Color4.Yellow,
                    Size = 110,
                    Position = new Vector2(80, -20),
                    Action = () => Push(new Options())
                },
            };
        }
    }
}
