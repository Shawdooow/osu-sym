using System;
using System.Collections.Generic;
using osu.Game.Audio;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;

namespace Symcol.Rulesets.Core.HitObjects
{
    public abstract class SymcolHitObject : HitObject, IHasComboInformation
    {
        public SampleInfo GetAdjustedSample(SampleInfo info, SampleControlPoint point = null)
        {
            List<SampleInfo> list = new List<SampleInfo>();

            SampleControlPoint control = SampleControlPoint;

            if (point != null)
                control = point;

            info.Bank = info.Bank ?? control.SampleBank;
            info.Volume = info.Volume > 0 ? info.Volume : control.SampleVolume;
            info.Suffix = Int32.Parse(info.Suffix ?? "0") > 0 ? info.Suffix : control.SampleSuffix.ToString();

            return info;
        }

        public bool NewCombo { get; set; }
        public int ComboOffset { get; set; }
        public int IndexInCurrentCombo { get; set; }
        public int ComboIndex { get; set; }
        public bool LastInCombo { get; set; }
    }
}
