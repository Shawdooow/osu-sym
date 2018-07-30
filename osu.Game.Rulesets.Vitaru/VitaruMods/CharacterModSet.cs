using osu.Game.Rulesets.Vitaru.Characters;
using osu.Game.Rulesets.Vitaru.Characters.Bosses;
using osu.Game.Rulesets.Vitaru.Characters.Bosses.DrawableBosses;
using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers.DrawableVitaruPlayers;
using osu.Game.Rulesets.Vitaru.OldMulti;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.VitaruMods
{
    public abstract class CharacterModSet
    {
        public abstract VitaruPlayer[] GetPlayers();

        public abstract DrawableVitaruPlayer[] GetDrawablePlayers(VitaruPlayfield playfield, VitaruNetworkingClientHandler vitaruNetworkingClientHandler);

        public abstract Boss[] GetBosses();

        public abstract DrawableBoss[] GetDrawableBosses(VitaruPlayfield vitaruPlayfield);
    }
}
