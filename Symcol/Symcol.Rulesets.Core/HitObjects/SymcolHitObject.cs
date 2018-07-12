using System.Collections.Generic;
using osu.Game.Audio;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;

namespace Symcol.Rulesets.Core.HitObjects
{
    public abstract class SymcolHitObject : HitObject
    {
        public SampleInfo GetAdjustedSample(SampleInfo info, SampleControlPoint point = null)
        {
            List<SampleInfo> list = new List<SampleInfo>();

            SampleControlPoint control = SampleControlPoint;

            if (point != null)
                control = point;

            return new SampleInfo
            {
                Bank = info.Bank ?? SampleControlPoint.SampleBank,
                Suffix = info.Suffix,
                Name = info.Name,
                Volume = info.Volume > 0 ? info.Volume : SampleControlPoint.SampleVolume,
            };
        }
    }
}
