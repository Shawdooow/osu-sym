#region usings

using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Chapters
{
    public class TouhosuChapter : VitaruChapter
    {
        public override string Title => "Touhosu";

        public sealed override VitaruPlayer[] GetPlayers() => GetTouhosuPlayers();

        public sealed override DrawableVitaruPlayer GetDrawablePlayer(VitaruPlayfield playfield, VitaruPlayer player) => GetDrawableTouhosuPlayer(playfield, (TouhosuPlayer)player);

        public virtual TouhosuPlayer[] GetTouhosuPlayers() => null;

        public virtual DrawableTouhosuPlayer GetDrawableTouhosuPlayer(VitaruPlayfield playfield, TouhosuPlayer player) => null;
    }
}
