﻿#region usings

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Game.Rulesets.Scoring;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Player.Packets;
using osuTK;
using osuTK.Input;
using Sym.Networking.NetworkingHandlers.Peer;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Multi.Player.Pieces
{
    public class Scoreboard : MultiplayerContainer
    {
        public readonly Container<ScoreboardItem> ScoreboardItems;

        private readonly ScoreProcessor scoreProcessor;

        private double updateScoreTime = double.MinValue;

        public Scoreboard(OsuNetworkingHandler osuNetworkingHandler, List<OsuUser> users, ScoreProcessor scoreProcessor) : base (osuNetworkingHandler)
        {
            this.scoreProcessor = scoreProcessor;

            AlwaysPresent = true;
            AutoSizeAxes = Axes.Y;
            Width = 120;

            Position = new Vector2(0, -200);
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.TopLeft;

            Child = ScoreboardItems = new Container<ScoreboardItem>
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
            };

            int i = 1;
            foreach (OsuUser user in users)
            {
                ScoreboardItems.Add(new ScoreboardItem(user, i));
                i++;
            }
        }

        protected override void OnPacketRecieve(PacketInfo<Host> info)
        {
            if (info.Packet is ScorePacket scorePacket)
                foreach (ScoreboardItem item in ScoreboardItems)
                    if (scorePacket.ID == item.User.ID && scorePacket.ID != OsuNetworkingHandler.OsuUser.ID)
                        item.Score = scorePacket.Score;
        }

        protected override void Update()
        {
            base.Update();

            if (Time.Current >= updateScoreTime)
            {
                updateScoreTime = Time.Current + 250;

                foreach (ScoreboardItem item in ScoreboardItems)
                    if (OsuNetworkingHandler.OsuUser.ID == item.User.ID)
                        item.Score = (int)scoreProcessor.TotalScore.Value;

                SendPacket(new ScorePacket
                {
                    ID = OsuNetworkingHandler.OsuUser.ID,
                    Score = (int)scoreProcessor.TotalScore.Value
                });
            }
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.Tab)
            {
                if (Alpha > 0)
                    this.FadeOut(100);
                else
                    this.FadeIn(100);
            }

            return base.OnKeyDown(e);
        }
    }
}
