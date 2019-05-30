using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Mix.UI;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Mix
{
    public class MixInputManager : RulesetInputManager<MixAction>
    {
        public MixInputManager(RulesetInfo ruleset) : base(ruleset, 0, SimultaneousBindingMode.Unique)
        {
            Child = new SoundButtonArray(KeyBindingContainer);
        }
    }

    public enum MixAction
    {
        NormalNormalLeft,
        NormalWhistleLeft,
        NormalFinishLeft,
        NormalClapLeft,

        DrumNormalLeft,
        DrumWhistleLeft,
        DrumFinishLeft,
        DrumClapLeft,

        SoftNormalLeft,
        SoftWhistleLeft,
        SoftFinishLeft,
        SoftClapLeft,

        NormalNormalRight,
        NormalWhistleRight,
        NormalFinishRight,
        NormalClapRight,

        DrumNormalRight,
        DrumWhistleRight,
        DrumFinishRight,
        DrumClapRight,

        SoftNormalRight,
        SoftWhistleRight,
        SoftFinishRight,
        SoftClapRight,
    }
}
