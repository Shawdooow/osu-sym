using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Edit
{
    public class VitaruEditRulesetContainer : VitaruRulesetContainer
    {
        public VitaruEditRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap, bool isForCurrentRuleset)
            : base(ruleset, beatmap, isForCurrentRuleset)
        {
        }

        protected override Playfield CreatePlayfield() => VitaruEditPlayfield;

        protected VitaruEditPlayfield VitaruEditPlayfield = new VitaruEditPlayfield();

        protected override DrawableHitObject<VitaruHitObject> GetVisualRepresentation(VitaruHitObject h)
        {
            if (h is Bullet bullet)
                return new DrawableBullet(bullet, VitaruEditPlayfield);
            if (h is Laser laser)
                return new DrawableLaser(laser, VitaruEditPlayfield);
            if (h is Pattern pattern)
                return new DrawablePattern(pattern, VitaruEditPlayfield);
            return null;
        }
    }
}
