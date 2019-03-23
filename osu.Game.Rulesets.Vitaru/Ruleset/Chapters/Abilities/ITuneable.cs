#region usings

using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Abilities
{
    public interface ITuneable
    {
        AspectLockedPlayfield CurrentPlayfield { get; set; }

        bool Untuned { get; set; }
    }
}
