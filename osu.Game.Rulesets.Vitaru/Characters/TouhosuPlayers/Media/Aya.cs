﻿namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Media
{
    public class Aya : TouhosuPlayer
    {
        public override string Name => "Aya Shameimaru";

        public override string FileName => "AyaShameimaru";

        public override double MaxHealth => 40;

        public override double MaxEnergy => 16;

        public override double EnergyCost => 4;

        //public override Color4 PrimaryColor => Color4.Red;

        //public override Color4 SecondaryColor => Color4.White;

        //public override Color4 TrinaryColor => Color4.Yellow;

        public override string Ability => "Snapshot";

        public override Role Role => Role.Offense;

        public override Difficulty Difficulty => Difficulty.Normal;

        public override string Background => "";

        public override bool Implemented => false;
    }
}
