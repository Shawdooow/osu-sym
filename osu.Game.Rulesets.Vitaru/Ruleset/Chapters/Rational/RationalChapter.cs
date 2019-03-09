using osu.Game.Rulesets.Vitaru.Mods.ChapterSets.Chapters;
using osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Rational.Characters;
using osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Rational.Characters.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.Characters.TouhosuPlayers;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Rational
{
    public class RationalChapter : TouhosuChapter
    {
        public override string Title => "The Rational Chapter";

        public override TouhosuPlayer[] GetTouhosuPlayers() => new TouhosuPlayer[]
        {
            new Marisa(),
        };

        public override DrawableTouhosuPlayer GetDrawableTouhosuPlayer(VitaruPlayfield playfield, TouhosuPlayer player)
        {
            switch (player.Name)
            {
                default:
                    return null;

                case "Marisa Kirisame":
                    return new DrawableMarisa(playfield);
            }
        }
    }
}
