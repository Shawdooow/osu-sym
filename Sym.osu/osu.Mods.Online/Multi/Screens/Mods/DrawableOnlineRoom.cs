using osu.Game.Screens.Multi.Lounge.Components;

namespace osu.Mods.Online.Multi.Screens.Mods
{
    public class DrawableOnlineRoom : DrawableRoom
    {
        public new OnlineRoom Room;

        public DrawableOnlineRoom(OnlineRoom online)
            : base(online)
        {
            Room = online;
        }
    }
}
