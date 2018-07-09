using OpenTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Hakurei
{
    public class Reimu : TouhosuPlayer
    {
        public override string Name => "Reimu Hakurei";

        public override string FileName => "ReimuHakurei";

        public override double MaxHealth => 80;

        public override double MaxEnergy => 42;

        public override double EnergyCost => 2;

        public override double EnergyDrainRate => 2;

        public override Color4 PrimaryColor => Color4.Red;

        public override Color4 SecondaryColor => Color4.White;

        public override Color4 TrinaryColor => Color4.Yellow;

        public override string Ability => "Leader";

        #region Ability Values

        public override string AbilityStats => "-Leader Max Buff Multiplier: " + LEADER_MAX_BUFF +
                                               "\n-Leader Min Buff Multiplier: " + LEADER_MIN_BUFF +
                                               "\n-Leader Farthest Range: " + LEADER_FARTHEST_RANGE +
                                               "\n-Leader Closest Range: " + LEADER_CLOSEST_RANGE;

        public const double LEADER_MAX_BUFF = 2d;

        public const double LEADER_MIN_BUFF = 1d;

        public const double LEADER_FARTHEST_RANGE = 64;

        public const double LEADER_CLOSEST_RANGE = 128;

        #endregion

        public override Role Role => Role.Support;

        public override Difficulty Difficulty => Difficulty.Easy;

        public override string Background => "When she was young Reimu feared the idea of having kids after how much trouble her mother would tease she was. " +
            "However age has a funny way of changing peoples views, one day she fell head over heals for Vaster's brother. " +
            "Twenty years later despite being a widow she still has her two beautiful children, a girl and a boy. " +
            "The girl reminds her of herself, while the boy of him. " +
            "Ever since he died it has been quiet, Reimu has no idea what happened that day but seemingly all the fairies disappeard. " +
            "The shrine has remained uncontested for nearly sixteen years. " +
            "It made her uneasy that soon her oldest would be taking over. " +
            "Ryukoy was by no means a fighter, if she ever got into trouble it likely wouldn't end well. " +
            "All Reimu can do is pray, age not only changes your prespective but also your body. " +
            "She might have been feared by many back in the day, but she certainly is not capable of what she used to be anymore.";

        public override bool Implemented => false;
    }
}
