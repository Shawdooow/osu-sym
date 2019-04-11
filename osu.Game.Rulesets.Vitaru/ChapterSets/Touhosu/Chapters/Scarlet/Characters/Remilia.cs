#region usings

using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osuTK.Graphics;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Scarlet.Characters
{
    public class Remilia : TouhosuPlayer
    {
        public override string Name => "Remilia Scarlet";

        public override double MaxHealth => 60;

        public override double MaxEnergy => 12;

        public override double EnergyCost => 2;

        public override Color4 PrimaryColor => Color4.LightPink;

        public override Color4 SecondaryColor => Color4.White;

        public override Color4 TrinaryColor => Color4.Red;

        public override string Ability => "Vampuric";

        public override Role Role => Role.Offense;

        public override Ruleset.Characters.TouhosuPlayers.Difficulty Difficulty => Ruleset.Characters.TouhosuPlayers.Difficulty.Normal;

        public override string Background => "";

        public override bool Implemented => false;
    }
}
