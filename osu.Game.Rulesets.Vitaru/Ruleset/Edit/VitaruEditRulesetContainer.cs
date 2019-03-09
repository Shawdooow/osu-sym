using osu.Framework.Graphics.Cursor;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Mods.Online.Base;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Edit
{
    public class VitaruEditRulesetContainer : VitaruRulesetContainer
    {
        public VitaruEditRulesetContainer(Rulesets.Ruleset ruleset, WorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        protected override VitaruPlayfield CreateVitaruPlayfield(VitaruInputManager inputManager, OsuNetworkingHandler networkingHandler) => new VitaruEditPlayfield((VitaruInputManager)KeyBindingInputManager);

        protected override CursorContainer CreateCursor() => null;
    }
}
