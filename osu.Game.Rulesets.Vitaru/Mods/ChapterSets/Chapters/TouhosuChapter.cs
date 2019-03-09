using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.Characters.VitaruPlayers;

namespace osu.Game.Rulesets.Vitaru.Mods.ChapterSets.Chapters
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
