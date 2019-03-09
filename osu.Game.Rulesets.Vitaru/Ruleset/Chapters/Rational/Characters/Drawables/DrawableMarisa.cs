using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.Characters.TouhosuPlayers;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Rational.Characters.Drawables
{
    public class DrawableMarisa : DrawableTouhosuPlayer
    {
        public DrawableMarisa(VitaruPlayfield playfield)
            : base(playfield, new Marisa())
        {
        }
    }
}
