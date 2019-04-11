#region usings

using System;
using osu.Framework.Audio.Sample;
using osu.Game.Audio;
using osu.Mods.Rulesets.Core.Skinning;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Audio
{
    public class VitaruSkinableSound : SymcolSkinnableSound
    {
        public VitaruSkinableSound(params SampleInfo[] samples) : base(samples)
        {
            Name = "VitaruSkinableSound";
        }

        protected override SampleChannel LoadChannel(SampleInfo info, Func<string, SampleChannel> getSampleFunction)
        {
            foreach (string lookup in info.LookupNames)
            {
                SampleChannel ch = getSampleFunction($"Gameplay/{lookup}");
                if (ch == null)
                    ch = getSampleFunction($"touhou/{lookup}");
                if (ch == null)
                    continue;

                ch.Volume.Value = info.Volume / 100.0 * 0.66f;
                return ch;
            }

            return null;
        }
    }
}
