using OpenTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Characters
{
    public abstract class Character
    {
        public virtual string Name { get; } = "Alex";

        public virtual double MaxHealth => 100;

        public virtual Color4 PrimaryColor { get; } = Color4.Green;

        public virtual Color4 SecondaryColor { get; } = Color4.LightBlue;

        public virtual Color4 TrinaryColor { get; } = Color4.LightGreen;

        public virtual string Background { get; } = "Alex always had a thing for music.";
    }
}
