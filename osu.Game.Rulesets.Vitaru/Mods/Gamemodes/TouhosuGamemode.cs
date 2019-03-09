using osu.Game.Rulesets.Vitaru.Mods.ChapterSets.Chapters;
using osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Rational;
using osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Scarlet;
using osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Worship;
using osuTK;

namespace osu.Game.Rulesets.Vitaru.Mods.Gamemodes
{
    public class TouhosuGamemode : VitaruGamemode
    {
        public override string Name => "Touhosu";

        public sealed override VitaruChapter[] GetChapters() => GetTouhosuChapters();

        public virtual TouhosuChapter[] GetTouhosuChapters() => new TouhosuChapter[]
        {
            new WorshipChapter(),
            new ScarletChapter(),
            new RationalChapter(),
            //new AlkiChapter(),
        };

        public override Vector2 PlayfieldAspectRatio => new Vector2(5, 4);

        public override Vector2 PlayfieldSize => new Vector2(1024, 820);

        public override Vector2 ClusterOffset => new Vector2(256, 0);

        public override string Description => "The \"amplified\" gamemode. Touhosu mode is everything Vitaru is and so much more. " +
                                                      "Selecting different characters no longer just changes your skin but also your stats and your spell!";
    }
}
