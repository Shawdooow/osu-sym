#region usings

using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.Chapters
{
    public class VitaruChapter
    {
        public virtual string Title { get; } = "Vitaru";

        public virtual string Description => null;

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
    }
}
