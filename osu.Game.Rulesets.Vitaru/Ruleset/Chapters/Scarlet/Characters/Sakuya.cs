using osu.Game.Graphics;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.Characters.TouhosuPlayers;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Scarlet.Characters
{
    public class Sakuya : TouhosuPlayer
    {
        public override string Name => "Sakuya Izayoi";

        public override double MaxHealth => 60;

        public override double MaxEnergy => 24;

        public override double EnergyCost => 4;

        public override double EnergyDrainRate => 4;

        public override Color4 PrimaryColor => Color4.Navy;

        public override Color4 SecondaryColor => OsuColour.FromHex("#92a0dd");

        public override Color4 TrinaryColor => OsuColour.FromHex("#d6d6d6");

        public override string Ability => "Time Waster";

        public override Role Role => Role.Defense;

        public override Objects.Characters.TouhosuPlayers.Difficulty Difficulty => Objects.Characters.TouhosuPlayers.Difficulty.Normal;

        public override string Background => "      Sakuya is no stranger to the oddities in the world, but never could they stop her from besting her opponents. " +
            "Her perfect record has only been tainted by one person, but The Hakureis are close friends of hers now.\n\n" +
            "       They have put there differences aside once to fight off something bigger then all of them combined, " +
            "but as the phrase goes: \"Greater than the sum of its parts\" they were able to hold the fort long enough to succeed.";

        public override bool Implemented => false;
    }
}
