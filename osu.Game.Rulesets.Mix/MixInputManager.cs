using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.UI;
using System.ComponentModel;

namespace osu.Game.Rulesets.Mix
{
    class MixInputManager : RulesetInputManager<MixAction>
    {
        public MixInputManager(RulesetInfo ruleset) : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
        }
    }

    public enum MixAction
    {
        //Movement
        [Description("West Left")]
        WestLeftButton,
        [Description("West Right")]
        WestRightButton,
        [Description("East Left")]
        EastLeftButton,
        [Description("East Right")]
        EastRightButton,
        [Description("North Left")]
         NorthLeftButton,
        [Description("North Right")]
        NorthRightButton,
        [Description("South Left")]
        SouthLeftButton,
        [Description("South Right")]
        SouthRightButton,
    }
}
