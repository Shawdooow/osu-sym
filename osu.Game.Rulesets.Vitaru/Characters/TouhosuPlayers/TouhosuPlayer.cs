using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers
{
    public class TouhosuPlayer : VitaruPlayer
    {
        public virtual double MaxEnergy { get; } = 24;

        public virtual double EnergyCost { get; } = 2;

        public virtual double EnergyCostPerSecond { get; } = 4;

        public virtual string Spell { get; } = "None";

        public virtual Role Role { get; } = Role.Offense;

        public virtual Difficulty Difficulty { get; } = Difficulty.Easy;

        public virtual bool Implemented { get; }
    }

    public enum Role
    {
        Offense,
        Defense,
        Support
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        Insane,
        Another,
        Extra,
    }
}
