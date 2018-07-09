using osu.Game.Rulesets.Objects.Drawables;
using Symcol.Rulesets.Core.HitObjects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Skinning;
using OpenTK.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Abilities;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableVitaruHitObject : DrawableSymcolHitObject<VitaruHitObject>, ITuneable
    {
        public override bool HandleMouseInput => false;

        protected readonly VitaruPlayfield VitaruPlayfield;

        public Container CurrentPlayfield { get; set; }

        public bool Untuned
        {
            get => untuned;
            set
            {
                if (value == untuned) return;

                untuned = value;

                if (value)
                {
                    VitaruPlayfield.Gamefield.Remove(this);
                    VitaruPlayfield.VitaruInputManager.BlurredPlayfield.Add(this);
                    CurrentPlayfield = VitaruPlayfield.VitaruInputManager.BlurredPlayfield;
                }
                else
                {
                    VitaruPlayfield.VitaruInputManager.BlurredPlayfield.Remove(this);
                    VitaruPlayfield.Gamefield.Add(this);
                    CurrentPlayfield = VitaruPlayfield.Gamefield;
                }
            }

        }

        private bool untuned;

        public bool Editor;

        protected bool Loaded { get; private set; }

        protected bool Started { get; private set; }

        public Action<bool> OnDispose;

        public DrawableVitaruHitObject(VitaruHitObject hitObject, VitaruPlayfield playfield) : base(hitObject)
        {
            VitaruPlayfield = playfield;
            CurrentPlayfield = playfield.Gamefield;

            AlwaysPresent = true;
        }

        protected override void SkinChanged(ISkinSource skin, bool allowFallback)
        {
            base.SkinChanged(skin, allowFallback);

            if (HitObject is IHasComboInformation combo && HitObject.ColorOverride == null)
                AccentColour = skin.GetValue<SkinConfiguration, Color4>(s => s.ComboColours.Count > 0 ? s.ComboColours[combo.ComboIndex % s.ComboColours.Count] : (Color4?)null) ?? Color4.White;
            else
                AccentColour = HitObject.ColorOverride.GetValueOrDefault();
        }

        protected sealed override void UpdateState(ArmedState state)
        {
            double transformTime = HitObject.StartTime - HitObject.TimePreempt;

            ApplyTransformsAt(transformTime, true);
            ClearTransformsAfter(transformTime, true);
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

        protected virtual void Load()
        {
            if (Loaded) return;
            Loaded = true;
        }

        protected virtual void Start()
        {
            if (Started) return;
            Started = true;
        }

        protected virtual void End()
        {
            if (!Started) return;
            Started = false;
        }

        protected virtual void Unload()
        {
            if (!Loaded) return;
            Loaded = false;
        }

        protected override void Dispose(bool isDisposing)
        {
            OnDispose?.Invoke(isDisposing);
            base.Dispose(isDisposing);
        }
    }

    public enum ComboResult
    {
        [System.ComponentModel.Description(@"")]
        None,
        [System.ComponentModel.Description(@"Good")]
        Good,
        [System.ComponentModel.Description(@"Amazing")]
        Perfect
    }
}
