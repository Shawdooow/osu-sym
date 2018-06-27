// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Input;
using OpenTK;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Drawables;
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
        public ClassicRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        protected override CursorContainer CreateCursor() => new GameplayCursor();

        public override ScoreProcessor CreateScoreProcessor() => new ClassicScoreProcessor(this);

        protected override Playfield CreatePlayfield() => new ClassicPlayfield(ClassicInputManager);

        public override PassThroughInputManager CreateInputManager() => ClassicInputManager;

        private bool loaded;

        protected ClassicInputManager ClassicInputManager
        {
            get
            {
                if (!loaded)
                {
                    loaded = true;
                    return classicInputManager = new ClassicInputManager(Ruleset.RulesetInfo);
                }
                else
                    return classicInputManager;
            }
        }

        private ClassicInputManager classicInputManager;

        protected override DrawableHitObject<ClassicHitObject> GetVisualRepresentation(ClassicHitObject h)
        {
            switch (h)
            {
                case HitCircle circle:
                    return new DrawableHitCircle(circle);
                case Slider slider:
                    return new DrawableSlider(slider);
                case Hold hold:
                    return new DrawableHold(hold);
                case Spinner spinner:
                    return new DrawableSpinner(spinner);
            }

            return null;
        }

        protected override ReplayInputHandler CreateReplayInputHandler(Replay replay) => new ClassicReplayInputHandler(replay);

        protected override Vector2 GetAspectAdjustedSize()
        {
            var aspectSize = DrawSize.X * 0.9f < DrawSize.Y ? new Vector2(DrawSize.X, DrawSize.X * 0.9f) : new Vector2(DrawSize.Y * 4f / 3f, DrawSize.Y);
            return new Vector2(aspectSize.X / DrawSize.X, aspectSize.Y / DrawSize.Y);
        }
    }
}
