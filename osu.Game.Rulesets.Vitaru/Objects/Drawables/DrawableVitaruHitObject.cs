using osu.Game.Rulesets.Objects.Drawables;
using Symcol.Rulesets.Core.HitObjects;
using System.ComponentModel;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Skinning;
using OpenTK.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using System;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableVitaruHitObject : DrawableSymcolHitObject<VitaruHitObject>
    {
        protected readonly VitaruPlayfield VitaruPlayfield;

        public bool Editor;

        protected bool Loaded { get; private set; }

        protected bool Started { get; private set; }

        public Action OnDispose;

        public DrawableVitaruHitObject(VitaruHitObject hitObject, VitaruPlayfield playfield) : base(hitObject)
        {
            VitaruPlayfield = playfield;

            AlwaysPresent = true;
        }

        protected override void SkinChanged(ISkinSource skin, bool allowFallback)
        {
            base.SkinChanged(skin, allowFallback);

            if (HitObject is IHasComboInformation combo && HitObject.ColorOverride == Color4.White)
                AccentColour = skin.GetValue<SkinConfiguration, Color4>(s => s.ComboColours.Count > 0 ? s.ComboColours[combo.ComboIndex % s.ComboColours.Count] : (Color4?)null) ?? Color4.White;
            else
                AccentColour = HitObject.ColorOverride;
        }

        protected sealed override void UpdateState(ArmedState state)
        {
            double transformTime = HitObject.StartTime - HitObject.TimePreempt;

            base.ApplyTransformsAt(transformTime, true);
            base.ClearTransformsAfter(transformTime, true);
        }

        protected override void Update()
        {
            base.Update();

            if (Time.Current >= HitObject.StartTime - HitObject.TimePreempt && Time.Current < HitObject.EndTime + HitObject.TimePreempt && !Loaded)
                Load();
            else if ((Time.Current < HitObject.StartTime - HitObject.TimePreempt || Time.Current >= HitObject.EndTime + HitObject.TimePreempt) && Loaded)
                Unload();

            if (Time.Current >= HitObject.StartTime && Time.Current < HitObject.EndTime && !Started)
                Start();
            else if ((Time.Current < HitObject.StartTime || Time.Current >= HitObject.EndTime) && Started)
                End();
        }

        protected virtual void Load() { Loaded = true; }

        protected virtual void Start() { Started = true; }

        protected virtual void End() { Started = false; }

        protected virtual void Unload() { Loaded = false; }

        public override void Delete()
        {
            OnDispose?.Invoke();
            base.Delete();
        }
    }

    public enum ComboResult
    {
        [Description(@"")]
        None,
        [Description(@"Good")]
        Good,
        [Description(@"Amazing")]
        Perfect
    }
}
