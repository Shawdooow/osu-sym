// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Input;
using OpenTK;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Classic.Beatmaps;
using osu.Game.Rulesets.Classic.Objects;
using osu.Game.Rulesets.Classic.Objects.Drawables;
using osu.Game.Rulesets.Classic.Replays;
using osu.Game.Rulesets.Classic.Scoring;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Replays;
using osu.Framework.Graphics.Cursor;
using osu.Game.Rulesets.Classic.UI.Cursor;
using osu.Game.Input.Handlers;

namespace osu.Game.Rulesets.Classic.UI
{
    public class ClassicRulesetContainer : RulesetContainer<ClassicHitObject>
    {
        public ClassicRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap, bool isForCurrentRuleset)
            : base(ruleset, beatmap, isForCurrentRuleset)
        {
        }

        protected override CursorContainer CreateCursor() => new GameplayCursor();

        public override ScoreProcessor CreateScoreProcessor() => new ClassicScoreProcessor(this);

        protected override BeatmapConverter<ClassicHitObject> CreateBeatmapConverter() => new ClassicBeatmapConverter();

        protected override BeatmapProcessor<ClassicHitObject> CreateBeatmapProcessor() => new ClassicBeatmapProcessor();

        protected override Playfield CreatePlayfield() => new ClassicPlayfield();

        public override PassThroughInputManager CreateInputManager() => new ClassicInputManager(Ruleset.RulesetInfo);

        protected override DrawableHitObject<ClassicHitObject> GetVisualRepresentation(ClassicHitObject h)
        {
            var circle = h as HitCircle;
            if (circle != null)
                return new DrawableHitCircle(circle);

            var slider = h as Slider;
            if (slider != null)
                return new DrawableSlider(slider);

            var hold = h as Hold;
            if (hold != null)
                return new DrawableHold(hold);

            var spinner = h as Spinner;
            if (spinner != null)
                return new DrawableSpinner(spinner);
            return null;
        }

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new ClassicReplayInputHandler(replay);

        protected override Vector2 GetAspectAdjustedSize()
        {
            var aspectSize = DrawSize.X * 0.75f < DrawSize.Y ? new Vector2(DrawSize.X, DrawSize.X * 0.75f) : new Vector2(DrawSize.Y * 4f / 3f, DrawSize.Y);
            return new Vector2(aspectSize.X / DrawSize.X, aspectSize.Y / DrawSize.Y);
        }
    }
}
