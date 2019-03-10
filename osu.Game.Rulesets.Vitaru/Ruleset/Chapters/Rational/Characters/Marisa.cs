using osu.Game.Graphics;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Rational.Characters
{
    public class Marisa : TouhosuPlayer
    {
        public override string Name => "Marisa Kirisame";

        public override double MaxHealth => 32;

        public override double MaxEnergy => 48;

        public override double EnergyCost => 2;

        public override double EnergyDrainRate => 4;

        public override Color4 PrimaryColor => Color4.WhiteSmoke;

        public override Color4 SecondaryColor => Color4.Black;

        public override Color4 TrinaryColor => OsuColour.FromHex("#842add");

        public override string Ability => "Mini-Hakkero";

        #region Ability Values

        public override string AbilityStats => "";

        public const double RAMP_UP = 2;

        #endregion

        public override Role Role => Role.Offense;

        public override Ruleset.Characters.TouhosuPlayers.Difficulty Difficulty => Ruleset.Characters.TouhosuPlayers.Difficulty.Hard;

        public override string Background => "";

        public override bool Implemented => false;
    }
}
