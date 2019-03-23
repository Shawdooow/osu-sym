#region usings

using osu.Game.Graphics;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osuTK.Graphics;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Scarlet.Characters
{
    public class Flandre : TouhosuPlayer
    {
        public override string Name => "Flandre Scarlet";

        public override double MaxHealth => 60;

        public override double MaxEnergy => 36;

        public override double EnergyCost => 18;

        public override double EnergyDrainRate => 4;

        public override Color4 PrimaryColor => Color4.Red;

        public override Color4 SecondaryColor => Color4.WhiteSmoke;

        public override Color4 TrinaryColor => OsuColour.FromHex("#ffe047");

        public override string Ability => "Four of a Kind";

        #region Ability Values

        public override string AbilityStats => "";

        #endregion

        public override Role Role => Role.Offense;

        public override Ruleset.Characters.TouhosuPlayers.Difficulty Difficulty => Ruleset.Characters.TouhosuPlayers.Difficulty.Normal;

        public override string Background => "";

        public override bool Implemented => false;
    }
}
