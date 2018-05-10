using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers
{
    public class TouhosuPlayer : VitaruPlayer
    {
        public virtual double MaxEnergy { get; } = 24;

        public virtual double EnergyCost { get; } = 2;

        public virtual double EnergyCostPerSecond { get; } = 4;

        public virtual string Background { get; } = "";
    }
}
