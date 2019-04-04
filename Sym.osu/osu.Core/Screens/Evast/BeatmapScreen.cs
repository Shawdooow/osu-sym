// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Screens;
using osu.Game.Screens.Backgrounds;
using osuTK;

namespace osu.Core.Screens.Evast
{
    public class BeatmapScreen : SymOsuScreen
    {
        protected override BackgroundScreen CreateBackground() => new BackgroundScreenBeatmap(Beatmap);

        protected virtual float BackgroundBlur => 20;

        private Vector2 backgroundBlur => new Vector2(BackgroundBlur);

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            Beatmap.ValueChanged += OnBeatmapChange;
            Beatmap.TriggerChange();
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

            Beatmap.ValueChanged += OnBeatmapChange;
            Beatmap.TriggerChange();
        }

        public override void OnSuspending(IScreen next)
        {
            base.OnSuspending(next);

            Beatmap.ValueChanged -= OnBeatmapChange;
        }

        public override bool OnExiting(IScreen next)
        {
            Beatmap.ValueChanged -= OnBeatmapChange;

            return base.OnExiting(next);
        }

        protected virtual void OnBeatmapChange(WorkingBeatmap beatmap)
        {
            var backgroundModeBeatmap = Background as BackgroundScreenBeatmap;
            if (backgroundModeBeatmap != null)
            {
                backgroundModeBeatmap.Beatmap = beatmap;
                backgroundModeBeatmap.BlurTo(backgroundBlur, 1000);
                backgroundModeBeatmap.FadeTo(1, 250);
            }
        }
    }
}
