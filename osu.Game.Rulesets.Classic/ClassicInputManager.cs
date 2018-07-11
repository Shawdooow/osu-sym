// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.ComponentModel;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Classic.UI;
using System.Collections.Generic;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Classic
{
    public class ClassicInputManager : RulesetInputManager<ClassicAction>
    {
        private ClassicUi classicUi;

        public Container<BufferedContainer> Hitobjects;

        public IEnumerable<ClassicAction> PressedActions => KeyBindingContainer.PressedActions;

        public ClassicInputManager(RulesetInfo ruleset) : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            AddRange(new Drawable[] 
            {
                Hitobjects = new Container<BufferedContainer>
                {
                    RelativeSizeAxes = Axes.Both,
                },
                classicUi = new ClassicUi
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Depth = -2,
                }
            });
        }
    }

    public enum ClassicAction
    {
        [Description("Left Button")]
        LeftButton,
        [Description("Right Button")]
        RightButton
    }
}
