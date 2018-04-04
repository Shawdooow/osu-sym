// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Osu.Multi;
using osu.Game.Rulesets.UI;
using Symcol.Core.Networking;
using Symcol.Rulesets.Core.Multiplayer.Networking;
using static osu.Game.Rulesets.Osu.UI.Cursor.GameplayCursor;

namespace osu.Game.Rulesets.Osu
{
    public class OsuInputManager : RulesetInputManager<OsuAction>
    {
        public IEnumerable<OsuAction> PressedActions => KeyBindingContainer.PressedActions;

        public static RulesetNetworkingClientHandler RulesetNetworkingClientHandler;

        public static List<RulesetClientInfo> LoadPlayerList = new List<RulesetClientInfo>();

        private Container cursorContainer;

        public OsuInputManager(RulesetInfo ruleset) : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Add(cursorContainer = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Depth = -2,
            });

            if (RulesetNetworkingClientHandler != null)
            {
                foreach (ClientInfo client in LoadPlayerList)
                    if (client is RulesetClientInfo rulesetClientInfo && rulesetClientInfo != RulesetNetworkingClientHandler.RulesetClientInfo)
                        cursorContainer.Add(new OsuCursor(rulesetClientInfo));

                RulesetNetworkingClientHandler.OnPacketReceive += (packet) =>
                {
                    if (packet is OsuMatchPacket matchPacket)
                    {
                        RulesetNetworkingClientHandler.ShareWithOtherPeers(matchPacket);
                        foreach (OsuCursor cursor in cursorContainer)
                            if (matchPacket.ClientInfo.IP == cursor.RulesetClientInfo.IP)
                                cursor.Position = new Vector2(matchPacket.MouseX, matchPacket.MouseY);
                    }
                };
            }
        }

        private double boo = double.MinValue;
        protected override void Update()
        {
            base.Update();

            if (boo <= Time.Current && RulesetNetworkingClientHandler != null)
            {
                //30 packets per second test
                boo = Time.Current + 1000 / 30;

                OsuMatchPacket matchPacket = new OsuMatchPacket(RulesetNetworkingClientHandler.RulesetClientInfo)
                {
                    MouseX = OsuCursor.Thing.ToSpaceOfOtherDrawable(Vector2.Zero, cursorContainer).X,
                    MouseY = OsuCursor.Thing.ToSpaceOfOtherDrawable(Vector2.Zero, cursorContainer).Y
                };

                RulesetNetworkingClientHandler.SendToInGameClients(matchPacket);
                RulesetNetworkingClientHandler.SendToHost(matchPacket);
            }
        }
    }

    public enum OsuAction
    {
        [System.ComponentModel.Description("Left Button")]
        LeftButton,
        [System.ComponentModel.Description("Right Button")]
        RightButton
    }
}
