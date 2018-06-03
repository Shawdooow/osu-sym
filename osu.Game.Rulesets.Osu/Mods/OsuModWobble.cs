using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.MathUtils;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Osu.Objects.Drawables;
using OpenTK;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModWobble : Mod, IApplicableToDrawableHitObjects
    {
        public override string Name => "Wobble";
        public override string ShortenedName => "Wobble";
        public override double ScoreMultiplier => 1.01;
        private const int wobble_speed = 100;
        private const int wooble_intensetiy = 2;
        public override FontAwesome Icon => FontAwesome.fa_balance_scale;

        public void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            drawables.ForEach(drawable => drawable.ApplyCustomUpdateState += apply);
        }

        private void apply(DrawableHitObject drawable, ArmedState state) 
        {
            if (!(drawable is DrawableOsuHitObject d))
                return;

            var h = d.HitObject;

            if (!(drawable is DrawableHitCircle || drawable is DrawableSlider || drawable is DrawableSpinner))
                return;
            if (drawable is DrawableSliderHead)
                return;

            using (drawable.BeginAbsoluteSequence(h.TimeFadein, true))
            {
                Vector2 pos = h.Position;

                for (int i = 0; i < (h.StartTime - h.TimeFadein) / wobble_speed; i++)
                {
                    drawable.Delay(i * wobble_speed).MoveTo(
                        pos + new Vector2(
                            (float)RNG.NextDouble(-wooble_intensetiy, wooble_intensetiy),
                            (float)RNG.NextDouble(-wooble_intensetiy, wooble_intensetiy)),
                        wobble_speed,
                        Easing.In
                    );
                }
            }
        }


    }
}
