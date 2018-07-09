using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers;
using System.ComponentModel;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Hakurei;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Inlaws;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Rational;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Scarlet;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers
{
    public class TouhosuPlayer : VitaruPlayer
    {
        public virtual double MaxEnergy { get; } = 0;

        public virtual double EnergyCost { get; } = 0;

        public virtual double EnergyDrainRate { get; } = 0;

        public virtual string Ability => "None";

        public virtual string AbilityStats => null;

        //public virtual string[] Abilities { get; } = null;

        public virtual Role Role { get; } = Role.Offense;

        public virtual Difficulty Difficulty { get; } = Difficulty.Easy;

        public virtual bool Implemented { get; }

        public static TouhosuPlayer GetTouhosuPlayer(string name)
        {
            switch (name)
            {
                default:
                    return new TouhosuPlayer();

                case "ReimuHakurei":
                    return new Reimu();
                case "RyukoyHakurei":
                    return new Ryukoy();
                case "TomajiHakurei":
                    return new Tomaji();

                case "SakuyaIzayoi":
                    return new Sakuya();
                case "RemiliaScarlet":
                    return new Remilia();
                case "FlandreScarlet":
                    return new Flandre();

                case "AliceLetrunce":
                    return new Alice();
                case "VasterLetrunce":
                    return new Vaster();

                case "MarisaKirisame":
                    return new Marisa();
            }
        }
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

        //Crazy Town
        [Description("Time Freeze")]
        TimeFreeze,
        [Description("Arcanum Barrier")]
        ArcanumBarrier,

        //No
        [Description("Centipede")]
        Centipede,
        [Description("Serious")]
        SeriousShit
    }
}
