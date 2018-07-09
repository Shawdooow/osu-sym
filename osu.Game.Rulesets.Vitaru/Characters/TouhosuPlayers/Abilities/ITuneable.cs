using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Abilities
{
    public interface ITuneable
    {
        Container CurrentPlayfield { get; set; }

        bool Untuned { get; set; }
    }
}
