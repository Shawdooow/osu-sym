using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using OpenTK;
using OpenTK.Graphics;
using Symcol.Core.LegacyNetworking;
using Symcol.Rulesets.Core.LegacyMultiplayer.Networking;
using Symcol.Rulesets.Core.Rulesets;
using Symcol.osu.Core;
using Symcol.osu.Core.Config;

namespace Symcol.Rulesets.Core.LegacyMultiplayer.Pieces
{
    public class Chat : Container
    {
        private readonly RulesetNetworkingClientHandler rulesetNetworkingClientHandler;

        private string playerColorHex = SymcolOsuModSet.SymcolConfigManager.GetBindable<string>(SymcolSetting.PlayerColor);

        private readonly FillFlowContainer<ChatMessage> messageContainer;
        private readonly OsuTextBox textBox;

        public Chat(RulesetNetworkingClientHandler rulesetNetworkingClientHandler)
        {
            this.rulesetNetworkingClientHandler = rulesetNetworkingClientHandler;

            rulesetNetworkingClientHandler.OnPacketReceive += (packet) =>
            {
                if (packet is ChatPacket chatPacket)
                    Add(chatPacket);
                if (rulesetNetworkingClientHandler.ClientType == ClientType.Host)
                    rulesetNetworkingClientHandler.ShareWithOtherPeers(packet);
            };

            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;
            RelativeSizeAxes = Axes.Both;
            Height = 0.46f;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.8f
                },
                new OsuScrollContainer
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.Both,
                    Height = 0.9f,

                    Children = new Drawable[]
                    {
                        messageContainer = new FillFlowContainer<ChatMessage>
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y
                        }
                    }
                },
                textBox = new OsuTextBox
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.98f,
                    Height = 36,
                    Position = new Vector2(0, -12),
                    Colour = Color4.White,
                    Text = "Type here!"
                }
            };

            textBox.OnCommit += (s, r) =>
            {
                AddMessage(textBox.Text);
                textBox.Text = "";
            };
        }

        public void Add(ChatPacket packet)
        {
            ChatMessage message = new ChatMessage(packet);
            messageContainer.Add(message);
        }

        public void AddMessage(string message)
        {
            if (message == "" | message == " ")
                return;

            try
            {
                OsuColour.FromHex(playerColorHex);
            }
            catch
            {
                playerColorHex = "#ffffff";
            }

            ChatPacket packet = new ChatPacket(rulesetNetworkingClientHandler.ClientInfo)
            {
                Author = SymcolOsuModSet.SymcolConfigManager.Get<string>(SymcolSetting.SavedName),
                AuthorColor = playerColorHex,
                Message = message,
            };

            rulesetNetworkingClientHandler.SendToHost(packet);
            rulesetNetworkingClientHandler.SendToInMatchClients(packet);
            Add(packet);
        }
    }
}
