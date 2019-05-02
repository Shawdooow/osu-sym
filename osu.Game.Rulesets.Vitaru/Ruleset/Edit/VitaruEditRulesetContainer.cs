#region usings

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Edit
{
    public class VitaruEditRulesetContainer : VitaruRulesetContainer
    {
        public VitaruEditRulesetContainer(Rulesets.Ruleset ruleset, WorkingBeatmap beatmap, IReadOnlyList<Mod> mods)
            : base(ruleset, beatmap, mods)
        {
        }

        protected override VitaruPlayfield CreateVitaruPlayfield(VitaruInputManager inputManager, OsuNetworkingHandler networkingHandler, MatchInfo match) => new VitaruEditPlayfield((VitaruInputManager)KeyBindingInputManager);

        public override GameplayCursorContainer Cursor => null;
    }
}
