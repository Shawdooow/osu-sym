using System;
using System.Collections.Generic;
using osu.Framework.Audio;
using osu.Game.Audio;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Mods.Rulesets.Core.Skinning;

namespace osu.Mods.Rulesets.Core.HitObjects
{
    /// <summary>
    /// Mostly stuff copied from Container
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public abstract class DrawableSymcolHitObject<TObject> : DrawableHitObject<TObject>
        where TObject : SymcolHitObject
    {
        protected AudioManager RulesetAudio { get; set; }

        protected DrawableSymcolHitObject(TObject hitObject)
    : base(hitObject)
        { }

        public List<SymcolSkinnableSound> SymcolSkinnableSounds = new List<SymcolSkinnableSound>();

        protected virtual void PlayBetterSamples()
        {
            foreach (SymcolSkinnableSound sound in SymcolSkinnableSounds)
                sound.Play();            
        }

        protected virtual SymcolSkinnableSound GetSkinnableSound(SampleInfo info, SampleControlPoint point = null)
        {
            SampleControlPoint control = HitObject.SampleControlPoint;

            if (point != null)
                control = point;

            return new SymcolSkinnableSound(HitObject.GetAdjustedSample(info, control)) { RulesetAudio = RulesetAudio };
        }

        public new bool IsDisposed => base.IsDisposed;

        public event Action OnDispose; 

        protected override void Dispose(bool isDisposing)
        {
            OnDispose?.Invoke();

            foreach (SymcolSkinnableSound sound in SymcolSkinnableSounds)
                sound.Dispose();

            SymcolSkinnableSounds = new List<SymcolSkinnableSound>();

            base.Dispose(isDisposing);
        }

        // Not a todo for symsets!

        // Todo: At some point we need to move these to DrawableHitObject after ensuring that all other Rulesets apply
        // transforms in the same way and don't rely on them not being cleared
        public override void ClearTransformsAfter(double time, bool propagateChildren = false, string targetMember = null) { }
        public override void ApplyTransformsAt(double time, bool propagateChildren = false) { }
    }
}
