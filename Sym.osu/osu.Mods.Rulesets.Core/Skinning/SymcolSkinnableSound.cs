#region usings

using System;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Audio;
using osu.Game.Skinning;

#endregion

namespace osu.Mods.Rulesets.Core.Skinning
{
    public class SymcolSkinnableSound : SkinnableSound
    {
        public override bool HandlePositionalInput => false;
        public override bool HandleNonPositionalInput => false;

        public SampleManager RulesetSamples;

        private readonly SampleInfo[] samples;
        private SampleChannel[] channels;

        private AudioManager audio;

        public SymcolSkinnableSound(params SampleInfo[] samples) : base(new SampleInfo[]{})
        {
            Name = "SymcolSkinnableSound";
            this.samples = samples;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            this.audio = audio;
        }

        public new void Play() => channels?.ForEach(c => c.Play());

        protected override void SkinChanged(ISkinSource skin, bool allowFallback)
        {
            channels = samples.Select(s =>
            {
                SampleChannel ch = LoadChannel(s, skin.GetSample);
                if (ch == null && allowFallback && RulesetSamples != null)
                    ch = LoadChannel(s, RulesetSamples.Get);
                if (ch == null && allowFallback)
                    ch = LoadChannel(s, audio.Sample.Get);
                return ch;
            }).Where(c => c != null).ToArray();
        }

        protected virtual SampleChannel LoadChannel(SampleInfo info, Func<string, SampleChannel> getSampleFunction)
        {
            foreach (string lookup in info.LookupNames)
            {
                SampleChannel ch = getSampleFunction($"Gameplay/{lookup}");
                if (ch == null)
                    continue;

                ch.Volume.Value = info.Volume / 100.0 * 0.66f;
                return ch;
            }

            return null;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            LoadAsyncComplete();
        }

        public virtual void Delete()
        {
            ClearInternal();
            ClearTransforms();
            Expire();
        }
    }
}
