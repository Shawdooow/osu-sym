using System;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Input
{
    public class VitaruInputContainer : Container, IKeyBindingHandler<VitaruAction>
    {
        private readonly bool touch = VitaruSettings.VitaruConfigManager.Get<bool>(VitaruSetting.Touch);

        public bool OnPressed(VitaruAction action) => !touch && (Pressed?.Invoke(action) ?? false);

        public Func<VitaruAction, bool> Pressed;

        public bool OnReleased(VitaruAction action) => !touch && (Released?.Invoke(action) ?? false);

        public Func<VitaruAction, bool> Released;
    }
}
