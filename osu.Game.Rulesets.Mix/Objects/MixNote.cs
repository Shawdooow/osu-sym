using OpenTK.Graphics;

namespace osu.Game.Rulesets.Mix.Objects
{
    public class MixNote : MixHitObject
    {
        public Color4 Color { get; set; } = Color4.Red;

        public int Volume { get; set; }

        public bool Whistle { get; set; }
        public bool Finish { get; set; }
        public bool Clap { get; set; }
    }
}
