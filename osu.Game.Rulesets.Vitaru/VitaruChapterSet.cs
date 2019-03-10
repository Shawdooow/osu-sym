using osu.Game.Rulesets.Vitaru.Mods.Chaptersets;
using osu.Game.Rulesets.Vitaru.Mods.Gamemodes;

namespace osu.Game.Rulesets.Vitaru
{
    public class VitaruChapterSet : Chapterset
    {
        public override VitaruGamemode[] GetGamemodes() => new VitaruGamemode[]
        {
            new DodgeGamemode(),
            new TouhosuGamemode(),
        };
    }
}
