using osuTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.Characters.VitaruPlayers
{
    public class VitaruPlayer : Character
    {
        public override string Name { get; } = "Alex";

        public override double MaxHealth => 80;

        public override Color4 PrimaryColor { get; } = Color4.Green;

        public override Color4 SecondaryColor { get; } = Color4.LightBlue;

        public override Color4 TrinaryColor { get; } = Color4.LightGreen;

        public override string Background { get; } = "Alex always had a thing for music.";
    }
}
