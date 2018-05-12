// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Classic.Beatmaps;
using osu.Game.Rulesets.Classic.Objects;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Circles.OsuDifficulty.Preprocessing;
using osu.Game.Rulesets.Circles.OsuDifficulty.Skills;

namespace osu.Game.Rulesets.Classic.ClassicDifficulty
{
    public class ClassicDifficultyCalculator : DifficultyCalculator
    {
        private const int section_length = 400;
        private const double difficulty_multiplier = 0.0675;

        public ClassicDifficultyCalculator(IBeatmap beatmap)
            : base(beatmap)
        {
        }

        public ClassicDifficultyCalculator(IBeatmap beatmap, Mod[] mods)
            : base(beatmap, mods)
        {
        }

        protected override void PreprocessHitObjects()
        {
            foreach (ClassicHitObject h in Beatmap.HitObjects)
                (h as Slider)?.Curve?.Calculate();
        }

        public override double Calculate(Dictionary<string, double> categoryDifficulty = null)
        {
            OsuDifficultyBeatmap beatmap = new OsuDifficultyBeatmap((List<ClassicHitObject>)Beatmap.HitObjects, TimeRate);
            Skill[] skills =
            {
                new Aim(),
                new Speed()
            };

            double sectionEnd = section_length / TimeRate;
            foreach (OsuDifficultyHitObject h in beatmap)
            {
                while (h.BaseObject.StartTime > sectionEnd)
                {
                    foreach (Skill s in skills)
                    {
                        s.SaveCurrentPeak();
                        s.StartNewSectionFrom(sectionEnd);
                    }

                    sectionEnd += section_length;
                }

                foreach (Skill s in skills)
                    s.Process(h);
            }

            double aimRating = Math.Sqrt(skills[0].DifficultyValue()) * difficulty_multiplier;
            double speedRating = Math.Sqrt(skills[1].DifficultyValue()) * difficulty_multiplier;

            double starRating = aimRating + speedRating + Math.Abs(aimRating - speedRating) / 2;

            if (categoryDifficulty != null)
            {
                categoryDifficulty.Add("Aim", aimRating);
                categoryDifficulty.Add("Speed", speedRating);
            }

            return starRating;
        }

        //protected override BeatmapConverter<ClassicHitObject> CreateBeatmapConverter(IBeatmap beatmap) => new ClassicBeatmapConverter();
    }
}
