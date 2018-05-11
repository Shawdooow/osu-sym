namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers
{
    public class Tomaji : TouhosuPlayer
    {
        public override string Name => "Tomaji Hakurei";

        public override double MaxHealth => 60;

        public override double MaxEnergy => base.MaxEnergy;

        public override double EnergyCost => base.EnergyCost;

        public override double EnergyDrainRate => base.EnergyDrainRate;

        public override string Background => "Tomaji has always been over shadowed by his older sister Ryukoy who is next in line to be the Hakurei Maiden, though he has never minded. " +
            "He had the option to take of to some exotic place far away if he wanted, but he didn't. " +
            "Despite having the entire world to explore he would be happy standing at his sister's side as any kind of help that he could be. " +
            "To him family was the most important and he knew she felt the same way. Even thought she would wear the title they would share the burden.";
    }
}
