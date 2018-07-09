﻿using OpenTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Hakurei
{
    public class Tomaji : TouhosuPlayer
    {
        public override string Name => "Tomaji Hakurei";

        public override string FileName => "TomajiHakurei";

        public override double MaxHealth => 40;

        public override double MaxEnergy => 12;

        public override double EnergyCost => 0;

        public override double EnergyDrainRate => 4;

        public override Color4 PrimaryColor => Color4.Orange;

        public override Color4 SecondaryColor => base.SecondaryColor;

        public override Color4 TrinaryColor => base.TrinaryColor;

        public override string Ability => "Blink";

        #region Ability Values

        public override string AbilityStats => "-Max Distance: " + BLINK_DISTANCE +
                                               "\n-Charge Time: " + CHARGE_TIME;

        public const double CHARGE_TIME = 1000;

        public const double BLINK_DISTANCE = 320;
        #endregion

        public override Role Role => Role.Offense;

        public override Difficulty Difficulty => Difficulty.Another;

        public override string Background => "Tomaji has always been over shadowed by his older sister Ryukoy who is next in line to be the Hakurei Maiden, though he has never minded. " +
            "He had the option to take off to some exotic place far away if he wanted, but he didn't. " +
            "Despite having the entire world to explore he would be happy standing at his sister's side as any kind of help that he could be. " +
            "To him family was the most important and he knew she felt the same way. Even thought she would wear the title they would share the burden.";

        public override bool Implemented => false;
    }
}
