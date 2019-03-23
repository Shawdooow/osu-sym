#region usings

using System.Collections.Generic;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Debug;
using osu.Game.Rulesets.Vitaru.Ruleset.Input;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Containers
{
    public class VitaruInputManager : RulesetInputManager<VitaruAction>
    {
        protected override RulesetKeyBindingContainer CreateKeyBindingContainer(RulesetInfo ruleset, int variant, SimultaneousBindingMode unique)
            => new VitaruKeyBindingContainer(ruleset, variant, unique);

        private Bindable<bool> debug = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.DebugMode);
        private readonly bool touch = VitaruSettings.VitaruConfigManager.Get<bool>(VitaruSetting.Touch);
        private readonly bool comboFire = VitaruSettings.VitaruConfigManager.Get<bool>(VitaruSetting.ComboFire);
        private readonly bool shade = VitaruSettings.VitaruConfigManager.Get<bool>(VitaruSetting.PitchShade);

        public List<Drawable> LoadCompleteChildren = new List<Drawable>();

        public readonly BufferedContainer BlurContainer;

        public readonly AspectLockedPlayfield BlurredPlayfield;

        public readonly Box Shade;

        public readonly DebugToolkit DebugToolkit;

        public readonly TouchControls TouchControls;

        public VitaruInputManager(RulesetInfo ruleset, int variant) : base(ruleset, variant, SimultaneousBindingMode.Unique)
        {
            LoadCompleteChildren.Add(BlurContainer = new BufferedContainer
            {
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                Name = "BlurContainer",
                BlurSigma = Vector2.One,

                Child = BlurredPlayfield = new AspectLockedPlayfield()
            });

            if (shade)
                LoadCompleteChildren.Add(Shade = new Box { RelativeSizeAxes = Axes.Both, Alpha = 0, Colour = Color4.Orange });

            LoadCompleteChildren.Add(DebugToolkit = new DebugToolkit());

            debug.ValueChanged += value => DebugToolkit.Alpha = value ? 1 : 0;
            debug.TriggerChange();

            if (comboFire)
                LoadCompleteChildren.Add(new ComboFire());

            if (touch)
                LoadCompleteChildren.Add(TouchControls = new TouchControls());
        }

        protected override void Dispose(bool isDisposing)
        {
            debug.UnbindAll();
            debug = null;
            base.Dispose(isDisposing);
        }

        private class VitaruKeyBindingContainer : RulesetKeyBindingContainer
        {
            public VitaruKeyBindingContainer (RulesetInfo ruleset, int variant, SimultaneousBindingMode unique)
                : base(ruleset, variant, unique)
            {
            }
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
