using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using OpenTK;
using OpenTK.Graphics;
using Symcol.osu.Core;
using Symcol.osu.Core.Config;
using Symcol.osu.Mods.Multi.Networking;
using Symcol.osu.Mods.Multi.Networking.Packets.Match;

namespace Symcol.osu.Mods.Multi.Screens.Pieces
{
    public class Chat : Container
    {
        private readonly OsuNetworkingClientHandler osuNetworkingClientHandler;

        private string playerColorHex = SymcolOsuModSet.SymcolConfigManager.GetBindable<string>(SymcolSetting.PlayerColor);

        private readonly FillFlowContainer<ChatMessage> messageContainer;
        private readonly OsuTextBox textBox;

        public Chat(OsuNetworkingClientHandler osuNetworkingClientHandler)
        {
            this.osuNetworkingClientHandler = osuNetworkingClientHandler;

            osuNetworkingClientHandler.OnPacketReceive += packet =>
            {
                if (packet is ChatPacket chatPacket)
                    Add(chatPacket);
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

            osuNetworkingClientHandler.SendPacket(new ChatPacket
            {
                AuthorColor = playerColorHex,
                Message = message,
            });
        }
    }
}
