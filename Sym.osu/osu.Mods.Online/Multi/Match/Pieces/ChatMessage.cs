using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Mods.Online.Multi.Match.Packets;
using osuTK;
using Sym.Base.Graphics.Containers;

namespace osu.Mods.Online.Multi.Match.Pieces
{
    public class ChatMessage : SymcolContainer
    {
        public ChatMessage(ChatPacket packet)
        {
            Alpha = 0;

            Anchor = Anchor.TopLeft;
            Origin = Anchor.TopLeft;

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Masking = true;

            Children = new Drawable[]
            {
                new OsuSpriteText
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Colour = OsuColour.FromHex(packet.AuthorColor),
                    TextSize = 24,
                    Text = packet.User.Username + ":"
                },
                new OsuTextFlowContainer(t => { t.TextSize = 24; })
                {
                    Position = new Vector2(140, 0),
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Text = packet.Message
                }
            };

            this.FadeInFromZero(150);
        }
    }
}
