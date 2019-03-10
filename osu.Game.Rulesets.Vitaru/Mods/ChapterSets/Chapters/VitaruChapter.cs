using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

namespace osu.Game.Rulesets.Vitaru.Mods.ChapterSets.Chapters
{
    public class VitaruChapter
    {
        public virtual string Title { get; } = "Vitaru";

        public virtual VitaruPlayer[] GetPlayers() => new VitaruPlayer[]
        {
            new Alex(),
        };

        public virtual DrawableVitaruPlayer GetDrawablePlayer(VitaruPlayfield playfield, VitaruPlayer player)
        {
            switch (player.Name)
            {
                default:
                    return null;
                case "Alex":
                    return new DrawableVitaruPlayer(playfield, player);
            }
        }

        public virtual string Description => "";
    }
}
