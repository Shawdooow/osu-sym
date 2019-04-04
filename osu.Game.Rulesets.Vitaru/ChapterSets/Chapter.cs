using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

namespace osu.Game.Rulesets.Vitaru.ChapterSets
{
    public abstract class Chapter
    {
        public abstract string Title { get; }

        public virtual string Description => null;

        public abstract VitaruPlayer[] GetPlayers();

        public abstract DrawableVitaruPlayer GetDrawablePlayer(VitaruPlayfield playfield, VitaruPlayer player);
    }
}
