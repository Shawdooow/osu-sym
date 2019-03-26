#region usings

using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.Chapters
{
    public class RejectChapter : Chapter
    {
        public override string Title => "The Rejected Chapter";

        public override string Description => null;

        public override VitaruPlayer[] GetPlayers() => new VitaruPlayer[]
        {
            new Alex(),
        };

        public override DrawableVitaruPlayer GetDrawablePlayer(VitaruPlayfield playfield, VitaruPlayer player)
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
