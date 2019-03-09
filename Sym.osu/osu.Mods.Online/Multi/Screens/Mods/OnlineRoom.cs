using osu.Framework.Configuration;
using osu.Game.Beatmaps;
using osu.Game.Online.Multiplayer;

namespace osu.Mods.Online.Multi.Screens.Mods
{
    public class OnlineRoom : Room
    {
        public Bindable<BeatmapInfo> Beatmap = new Bindable<BeatmapInfo>();
    }
}
