#region usings

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Lobby.Packets;
using osuTK.Graphics;

#endregion

namespace osu.Mods.Online.Multi.Lobby.Pieces
{
    public class MatchTile : ClickableContainer
    {
        public MatchTile(OsuNetworkingHandler osuNetworkingHandler, MatchInfo match)
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
