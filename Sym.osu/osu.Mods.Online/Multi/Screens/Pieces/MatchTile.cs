using osu.Framework.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Mods.Online.Multi.Packets.Lobby;
using Symcol.Base.Graphics.Containers;

namespace osu.Mods.Online.Multi.Screens.Pieces
{
    public class MatchTile : SymcolContainer
    {
        private readonly MatchListPacket.MatchInfo match;

        public MatchTile(MatchListPacket.MatchInfo match)
        {
            this.match = match;

            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;

            RelativeSizeAxes = Axes.X;
            Width = 0.98f;
            Height = 40f;

            Children = new Drawable[]
            {
                new OsuSpriteText
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    TextSize = 20,
                    Text = match.Name,
                }
            };
        }
    }
}
