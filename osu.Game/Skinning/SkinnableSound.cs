// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Audio;

namespace osu.Game.Skinning
{
    public class SkinnableSound : SkinReloadableDrawable
    {
        public AudioManager RulesetAudio;

        private readonly SampleInfo[] samples;
        private SampleChannel[] channels;

        private AudioManager audio;

        public SkinnableSound(params SampleInfo[] samples)
        {
            this.samples = samples;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            this.audio = audio;
        }

        public void Play() => channels?.ForEach(c => c.Play());

        protected override void SkinChanged(ISkinSource skin, bool allowFallback)
        {
            channels = samples.Select(s =>
            {
                var ch = loadChannel(s, skin.GetSample);
                if (ch == null && allowFallback)
                    ch = loadChannel(s, audio.Sample.Get);
                return ch;
            }).Where(c => c != null).ToArray();
        }

        private SampleChannel loadChannel(SampleInfo info, Func<string, SampleChannel> getSampleFunction)
        {
            SampleChannel ch = null;

            if (info.Namespace != null)
                ch = getSampleFunction($"Gameplay/{info.Namespace}/{info.Bank}-{info.Name + info.BankNumber}");
            
            // try without number as a fallback.	
            if (info.Namespace != null && ch == null)
                ch = getSampleFunction($"Gameplay/{info.Namespace}/{info.Bank}-{info.Name}");

            // try without namespace as a fallback.	
            if (ch == null)
                ch = getSampleFunction($"Gameplay/{info.Bank}-{info.Name + info.BankNumber}");

            // try without number as a fallback.
            if (ch == null)
                ch = getSampleFunction($"Gameplay/{info.Bank}-{info.Name}");

            // try with ruleset as a fallback.
            if (RulesetAudio != null && ch == null)
                ch = RulesetAudio.GetSampleManager().Get($"{info.Bank}-{info.Name}");

            if (ch != null)
                ch.Volume.Value = RulesetAudio != null ? info.Volume / 100.0 * 0.8f : info.Volume / 100.0;

            return ch;
        }
    }
}
