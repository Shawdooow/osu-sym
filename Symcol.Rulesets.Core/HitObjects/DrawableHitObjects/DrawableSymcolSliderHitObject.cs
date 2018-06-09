using System.Linq;
using osu.Framework.Logging;
using osu.Game.Audio;
using Symcol.Rulesets.Core.Skinning;

namespace Symcol.Rulesets.Core.HitObjects
{
    public abstract class DrawableSymcolSliderHitObject<TObject> : DrawableSymcolHitObject<TObject>
        where TObject : SymcolSliderHitObject
    {
        protected DrawableSymcolSliderHitObject(TObject hitObject)
            : base(hitObject)
        {
        }

        protected void PlayBetterRepeatSamples()
        {
            PlayBetterSamples();

            if (HitObject.BetterRepeatSamples.Count > 0)
            {
                foreach (SampleInfo info in HitObject.BetterRepeatSamples.First())
                {
                    SymcolSkinnableSound sound;
                    SymcolSkinnableSounds.Add(sound = GetSkinnableSound(info, HitObject.SampleControlPoints.Count > 0 ? HitObject.SampleControlPoints.First() : null));
                    Add(sound);
                }
                HitObject.BetterRepeatSamples.Remove(HitObject.BetterRepeatSamples.First());

                if (HitObject.SampleControlPoints.Count > 0)
                    HitObject.SampleControlPoints.Remove(HitObject.SampleControlPoints.First());
                else
                    Logger.Log("Expected a SampleControlPoint in DrawableSlider!", LoggingTarget.Runtime, LogLevel.Debug);
            }
        }
    }
}
