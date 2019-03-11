using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Packets.Lobby;
using osuTK.Graphics;
using Symcol.Base.Graphics.Containers;

namespace osu.Mods.Online.Multi.Screens.Pieces
{
    public class MatchTile : SymcolClickableContainer
    {
        public MatchTile(OsuNetworkingHandler osuNetworkingHandler, MatchListPacket.MatchInfo match)
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;

            RelativeSizeAxes = Axes.X;
            Width = 0.98f;
            Height = 40f;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.8f,
                },
                new OsuSpriteText
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    TextSize = 20,
                    Text = match.Name,
                }
            };

            Action = () => osuNetworkingHandler.SendToServer(new JoinMatchPacket
            {
                User = osuNetworkingHandler.OsuUserInfo,
                Match = match,
            });
        }
    }
}
