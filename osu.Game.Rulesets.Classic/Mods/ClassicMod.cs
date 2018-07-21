// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Classic.Replays;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Classic.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Classic.Objects.Drawables;
using osu.Game.Rulesets.Classic.UI;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using OpenTK;

namespace osu.Game.Rulesets.Classic.Mods
{
    public class ClassicModNoFail : ModNoFail
    {
        public override Type[] IncompatibleMods => base.IncompatibleMods.Concat(new[] { typeof(ClassicModAutopilot) }).ToArray();
    }

    public class ClassicModEasy : ModEasy
    {

    }

    public class ClassicModHidden : ModHidden, IApplicableToDrawableHitObjects
    {
        public override string Description => @"Play with no approach circles and fading notes for a slight score advantage.";
        public override double ScoreMultiplier => 1.06;

        private const float fade_in_duration_multiplier = 0.4f;
        private const double fade_out_duration_multiplier = 0.3;

        public void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            foreach (var d in drawables.OfType<DrawableClassicHitObject>())
                d.ApplyCustomUpdateState += ApplyHiddenState;
        }

        protected void ApplyHiddenState(DrawableHitObject drawable, ArmedState state)
        {
            if (!(drawable is DrawableClassicHitObject d))
                return;

            var fadeOutStartTime = d.HitObject.StartTime - (d.HitObject.TimePreempt / 3) * 2;
            var fadeOutDuration = d.HitObject.TimePreempt * fade_out_duration_multiplier;

            // new duration from completed fade in to end (before fading out)
            var longFadeDuration = ((d.HitObject as IHasEndTime)?.EndTime ?? d.HitObject.StartTime) - fadeOutStartTime;

            switch (drawable)
            {
                case DrawableHitCircle circle:
                    // we don't want to see the approach circle
                    circle.ApproachCircle.Hide();

                    // fade out immediately after fade in.
                    using (drawable.BeginAbsoluteSequence(fadeOutStartTime, true))
                        circle.FadeOut(fadeOutDuration);
                    break;
                case DrawableSlider slider:
                    using (slider.BeginAbsoluteSequence(fadeOutStartTime, true))
                    {
                        slider.Body.FadeOut(longFadeDuration, Easing.Out);

                        // delay a bit less to let the sliderball fade out peacefully instead of having a hard cut
                        using (slider.BeginDelayedSequence(longFadeDuration - fadeOutDuration, true))
                            slider.Ball.FadeOut(fadeOutDuration);
                    }

                    break;
                case DrawableSpinner spinner:
                    // hide elements we don't care about.
                    spinner.Disc.Hide();
                    spinner.Ticks.Hide();
                    spinner.Background.Hide();

                    using (spinner.BeginAbsoluteSequence(fadeOutStartTime + longFadeDuration, true))
                    {
                        spinner.FadeOut(fadeOutDuration);

                        // speed up the end sequence accordingly
                        switch (state)
                        {
                            case ArmedState.Hit:
                                spinner.ScaleTo(spinner.Scale * 1.2f, fadeOutDuration * 2, Easing.Out);
                                break;
                            case ArmedState.Miss:
                                spinner.ScaleTo(spinner.Scale * 0.8f, fadeOutDuration * 2, Easing.In);
                                break;
                        }

                        spinner.Expire();
                    }

                    break;
            }
        }
    }

    public class ClassicModSuddenDeath : ModSuddenDeath
    {
        public override Type[] IncompatibleMods => base.IncompatibleMods.Concat(new[] { typeof(ClassicModAutopilot) }).ToArray();
    }

    public class ClassicModDaycore : ModDaycore
    {
        public override double ScoreMultiplier => 0.5;
    }

    public class ClassicModDoubleTime : ModDoubleTime
    {
        public override double ScoreMultiplier => 1.12;
    }

    public class ClassicModRelax : ModRelax
    {
        public override string Description => "You don't need to click.\nGive your clicking/tapping finger a break from the heat of things.";
        public override Type[] IncompatibleMods => base.IncompatibleMods.Concat(new[] { typeof(ClassicModAutopilot) }).ToArray();
    }

    public class ClassicModHalfTime : ModHalfTime
    {
        public override double ScoreMultiplier => 0.5;
    }

    public class ClassicModNightcore : ModNightcore
    {
        public override double ScoreMultiplier => 1.12;
    }

    public class ClassicModFlashlight : ModFlashlight
    {
        public override double ScoreMultiplier => 1.12;
    }

    public class ClassicModPerfect : ModPerfect
    {

    }

    public class ClassicModSpunOut : Mod
    {
        public override string Name => "Spun Out";
        public override string ShortenedName => "SO";
        public override FontAwesome Icon => FontAwesome.fa_osu_mod_spunout;
        public override string Description => @"Spinners will be automatically completed";
        public override double ScoreMultiplier => 0.9;
        public override bool Ranked => true;
        public override Type[] IncompatibleMods => new[] { typeof(ModAutoplay), typeof(ClassicModAutopilot) };
    }

    public class ClassicModAutopilot : Mod
    {
        public override string Name => "Autopilot";
        public override string ShortenedName => "AP";
        public override FontAwesome Icon => FontAwesome.fa_osu_mod_autopilot;
        public override string Description => @"Automatic cursor movement - just follow the rhythm.";
        public override double ScoreMultiplier => 0;
        public override bool Ranked => false;
        public override Type[] IncompatibleMods => new[] { typeof(ClassicModSpunOut), typeof(ModRelax), typeof(ModSuddenDeath), typeof(ModNoFail), typeof(ModAutoplay) };
    }

    public class ClassicModAutoplay : ModAutoplay<ClassicHitObject>
    {
        public override Type[] IncompatibleMods => base.IncompatibleMods.Concat(new[] { typeof(ClassicModAutopilot), typeof(ClassicModSpunOut) }).ToArray();

        protected override Score CreateReplayScore(Beatmap<ClassicHitObject> beatmap) => new Score
        {
            Replay = new ClassicAutoGenerator(beatmap).Generate()
        };
    }

    public class ClassicModTarget : Mod
    {
        public override string Name => "Target";
        public override string ShortenedName => "TP";
        public override FontAwesome Icon => FontAwesome.fa_osu_mod_target;
        public override string Description => @"";
        public override double ScoreMultiplier => 1;
    }
}
