#region usings

using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Edit
{
    public class VitaruEditPlayfield : VitaruPlayfield
    {
        public override bool Editor => true;

        protected override bool KiaiBoss => VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.EditorBoss) && base.KiaiBoss;

        public VitaruEditPlayfield(VitaruInputManager vitaruInput) : base(vitaruInput)
        {
        }
    }
}
