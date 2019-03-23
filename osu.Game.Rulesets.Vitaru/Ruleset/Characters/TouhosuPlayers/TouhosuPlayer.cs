#region usings

using System.ComponentModel;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers
{
    public class TouhosuPlayer : VitaruPlayer
    {
        public virtual double MaxEnergy { get; } = 24;

        public virtual double EnergyCost { get; } = 4;

        public virtual double EnergyDrainRate { get; } = 0;

        public virtual string Ability => "None";

        public virtual string AbilityStats => null;

        public virtual Role Role { get; } = Role.Offense;

        public virtual Difficulty Difficulty { get; } = Difficulty.Easy;

        public override string Background => "";

        public virtual bool Implemented { get; }
    }

    public enum Role
    {
        Offense,
        Defense,
        Support,
        Specialized,
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
        Insane,
        Another,
        Extra,

        //Crazy Town
        [Description("Time Freeze")]
        TimeFreeze,
        [Description("Arcanum Barrier")]
        ArcanumBarrier,

        //No
        [Description("Centipede")]
        Centipede,
        [Description("Serious")]
        SeriousShit,
    }
}
