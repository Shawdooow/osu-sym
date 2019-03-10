using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Abilities
{
    public interface ITuneable
    {
        AspectLockedPlayfield CurrentPlayfield { get; set; }

        bool Untuned { get; set; }
    }
}
