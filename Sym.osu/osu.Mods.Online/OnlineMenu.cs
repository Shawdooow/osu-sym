#region usings

using osu.Core.Containers.Shawdooow;
using osu.Core.Screens.Evast;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Mods.Online.Base.Screens;
using osu.Mods.Online.Multi.Lobby;
using osuTK;
using osuTK.Graphics;
using Sym.Networking.NetworkingHandlers;

#endregion

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
                    Position = new Vector2(-80, -20),
                    Action = () =>
                    {
                        if (OnlineModset.OsuNetworkingHandler != null && OnlineModset.OsuNetworkingHandler.Host.Statues >= ConnectionStatues.Connected)
                            Push(new LobbyScreen(OnlineModset.OsuNetworkingHandler));
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
                    Position = new Vector2(80, 20),
                    Action = () => Push(new Options())
                },
                new SymcolButton
                {
                    ButtonText = "Import",
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.Red,
                    ButtonColorBottom = Color4.Orange,
                    Size = 100,
                    Position = new Vector2(80, -180),
                    Action = () =>
                    {
                        if (OnlineModset.OsuNetworkingHandler != null && OnlineModset.OsuNetworkingHandler.Host.Statues >= ConnectionStatues.Connected)
                            Push(new Import());
                        else
                            Logger.Log("Connect to a server first!", LoggingTarget.Network, LogLevel.Error);
                    }
                },
                new SymcolButton
                {
                    ButtonText = "Back",
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    Size = 80,
                    Action = this.Exit,
                    Position = new Vector2(-120 , 140),
                },
            };
        }
    }
}
