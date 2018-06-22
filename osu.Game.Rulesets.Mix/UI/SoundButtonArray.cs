using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;

namespace osu.Game.Rulesets.Mix.UI
{
    public class SoundButtonArray : Container
    {
        public SoundButtonArray(KeyBindingContainer<MixAction> bindingContainer)
        {
            RelativeSizeAxes = Axes.Both;

            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;

            Height = 0.5f;

            foreach (MixAction action in System.Enum.GetValues(typeof(MixAction)))
                Add(new SoundButton(action, bindingContainer));
        }
    }
}
