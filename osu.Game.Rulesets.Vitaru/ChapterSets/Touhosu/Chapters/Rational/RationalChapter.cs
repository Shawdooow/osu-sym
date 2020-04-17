#region usings

using osu.Game.Rulesets.Vitaru.ChapterSets.Chapters;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Media;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Media.Drawables;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Rational.Characters;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Rational.Characters.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Rational
{
    public class RationalChapter : TouhosuChapter
    {
        public override string Title => "The Rational Chapter";

        public override TouhosuPlayer[] GetTouhosuPlayers() => new TouhosuPlayer[]
        {
            new Marisa(),
            new Aya(), 
        };

        public override DrawableTouhosuPlayer GetDrawableTouhosuPlayer(VitaruPlayfield playfield, TouhosuPlayer player)
        {
            switch (player.Name)
            {
                default:
                    return null;

                case "Marisa Kirisame":
                    return new DrawableMarisa(playfield);
                case "Aya Shameimaru":
                    return new DrawableAya(playfield);
            }
        }
    }
}
