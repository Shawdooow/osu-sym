using OpenTK.Graphics;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers
{
    public class Sakuya : TouhosuPlayer
    {
        public override string Name => "Sakuya Izayoi";

        public override string FileName => "SakuyaIzayoi";

        public override double MaxHealth => 100;

        public override double MaxEnergy => 36;

        public override double EnergyCost => 2;

        public override double EnergyDrainRate => 8;

        public override Color4 PrimaryColor => Color4.Navy;

        public override Color4 SecondaryColor => OsuColour.FromHex("#92a0dd");

        public override Color4 ComplementaryColor => OsuColour.FromHex("#d6d6d6");

        public override string Spell => "Time Keeper";

        public override Role Role => Role.Defense;

        public override Difficulty Difficulty => Difficulty.Normal;

        public override string Background => "";

        public override bool Implemented => true;
    }
}
