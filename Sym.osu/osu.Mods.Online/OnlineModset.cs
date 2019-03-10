﻿using System;
using osu.Core;
using osu.Core.Config;
using osu.Core.Containers.Shawdooow;
using osu.Core.OsuMods;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Game;
using osu.Game.Screens;
using osu.Mods.Online.Base;
using osuTK;
using osuTK.Graphics;

namespace osu.Mods.Online
{
    public class OnlineModset : OsuModSet
    {
        public static OsuNetworkingHandler OsuNetworkingHandler;

        private readonly Bindable<int> auto = SymcolOsuModSet.SymcolConfigManager.GetBindable<int>(SymcolSetting.Auto);
        private readonly Bindable<string> ipBindable = SymcolOsuModSet.SymcolConfigManager.GetBindable<string>(SymcolSetting.SavedIP);
        private readonly Bindable<int> portBindable = SymcolOsuModSet.SymcolConfigManager.GetBindable<int>(SymcolSetting.SavedPort);

        public override SymcolButton GetMenuButton() => new SymcolButton
        {
            ButtonText = "Online",
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            ButtonColorTop = Color4.Black,
            ButtonColorBottom = Color4.LightGray,
            Size = 90,
            Position = new Vector2(220, 20),
        };

        public override OsuScreen GetScreen() => new OnlineMenu();

        public override void LoadComplete(OsuGame game)
        {
            base.LoadComplete(game);

            switch (auto.Value)
            {
                case 1:
                    OsuNetworkingHandler = new OsuNetworkingHandler
                    {
                        Address = ipBindable.Value + ":" + portBindable.Value
                    };

                    game.Add(OsuNetworkingHandler);
                    OsuNetworkingHandler.OnConnectedToHost += host => Logger.Log("Connected to server", LoggingTarget.Network, LogLevel.Debug);
                    OsuNetworkingHandler.Connect();
                    break;
                case 2:
                    OsuNetworkingHandler = new OsuNetworkingHandler
                    {
                        Address = ipBindable.Value + ":" + portBindable.Value
                    };

                    OsuNetworkingHandler.Add(new OsuServerNetworkingHandler
                    {
                        Address = ipBindable.Value + ":" + portBindable.Value
                    });

                    game.Add(OsuNetworkingHandler);
                    OsuNetworkingHandler.OnConnectedToHost += host => Logger.Log("Connected to local server", LoggingTarget.Network, LogLevel.Debug);
                    OsuNetworkingHandler.Connect();
                    break;
            }
        }
    }
}
