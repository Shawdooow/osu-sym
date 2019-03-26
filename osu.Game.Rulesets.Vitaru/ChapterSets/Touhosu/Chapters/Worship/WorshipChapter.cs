#region usings

using osu.Game.Rulesets.Vitaru.ChapterSets.Chapters;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Worship.Characters;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Worship.Characters.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Worship
{
    public class WorshipChapter : TouhosuChapter
    {
        public override string Title => "The Chapter of Worship";

        public override TouhosuPlayer[] GetTouhosuPlayers() => new TouhosuPlayer[]
        {
            new Reimu(),
            new Ryukoy(),
            new Tomaji(),
        };

        public override DrawableTouhosuPlayer GetDrawableTouhosuPlayer(VitaruPlayfield playfield, TouhosuPlayer player)
        {
            switch (player.Name)
            {
                default:
                    return null;

                case "Reimu Hakurei":
                    return new DrawableReimu(playfield);
                case "Ryukoy Hakurei":
                    return new DrawableRyukoy(playfield);
                case "Tomaji Hakurei":
                    return new DrawableTomaji(playfield);
            }
        }
    }
}
