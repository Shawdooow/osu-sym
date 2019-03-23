#region usings

using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

#endregion

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
