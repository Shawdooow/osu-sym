using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers;
using System.ComponentModel;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers
{
    public class TouhosuPlayer : VitaruPlayer
    {
        public new virtual TouhosuCharacters Character => TouhosuCharacters.SakuyaIzayoi;

        public virtual double MaxEnergy { get; } = 24;

        public virtual double EnergyCost { get; } = 2;

        public virtual double EnergyCostPerSecond { get; } = 4;

        public virtual string Background { get; } = "";
    }

    public enum TouhosuCharacters
    {
        //The Hakurei Family, or whats left of them
        [Description("Reimu Hakurei")]
        ReimuHakurei,
        [Description("Ryukoy Hakurei")]
        RyukoyHakurei,
        [Description("Tomaji Hakurei")]
        TomajiHakurei,

        //Hakurei Family Friends, 
        [Description("Sakuya Izayoi")]
        SakuyaIzayoi,
        //[System.ComponentModel.Description("Flandre Scarlet")]
        //FlandreScarlet,
        //[System.ComponentModel.Description("Remilia Scarlet")]
        //RemiliaScarlet,

        //Uncle Vaster and Aunty Alice
        //[System.ComponentModel.Description("Alice Letrunce")]
        //AliceLetrunce,
        //[System.ComponentModel.Description("Vaster Letrunce")]
        //VasterLetrunce,

        //[System.ComponentModel.Description("Marisa Kirisame")]
        //MarisaKirisame,
    }
}
