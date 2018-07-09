using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Vitaru.Debug;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using OpenTK;
using Symcol.Rulesets.Core.Rulesets;

namespace osu.Game.Rulesets.Vitaru
{
    public class VitaruInputManager : SymcolInputManager<VitaruAction>
    {
        private readonly bool debug = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.DebugMode);
        private readonly bool comboFire = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.ComboFire);
        private readonly bool shade = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.PitchShade);

        protected override bool VectorVideo => VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.VectorVideos);

        public readonly BufferedContainer BlurContainer;

        public readonly AspectLockedPlayfield BlurredPlayfield;

        public readonly Box Shade;

        public readonly DebugToolkit DebugToolkit;

        public VitaruInputManager(RulesetInfo ruleset, int variant) : base(ruleset, variant, SimultaneousBindingMode.Unique)
        {
            Add(BlurContainer = new BufferedContainer
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                Name = "BlurContainer",
                BlurSigma = Vector2.One,

                Child = BlurredPlayfield = new AspectLockedPlayfield()
            });

            if (shade)
                Add(Shade = new Box { RelativeSizeAxes = Axes.Both, Alpha = 0, Colour = Color4.Orange });

            if (debug)
                Add(DebugToolkit = new DebugToolkit());

            if (comboFire)
                Add(new ComboFire());
        }
    }

    public enum VitaruAction
    {
        None = -1,

        //Movement
        Left = 0,
        Right,
        Up,
        Down,

        //Self-explanitory
        Shoot,
        Spell,

        //Slows the player + reveals hitbox
        Slow,

        //Sakuya
        Increase,
        Decrease,

        //Ryukoy
        Pull,
    }
}
