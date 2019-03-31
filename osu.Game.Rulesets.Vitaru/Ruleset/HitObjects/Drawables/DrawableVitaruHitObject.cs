#region usings

using osu.Framework.Configuration;
using osu.Game.Audio;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Vitaru.ChapterSets;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Abilities;
using osu.Game.Rulesets.Vitaru.Ruleset.Audio;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Scoring.Judgements;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osu.Game.Skinning;
using osu.Mods.MapMixer;
using osu.Mods.Rulesets.Core.HitObjects;
using osu.Mods.Rulesets.Core.Skinning;
using osuTK.Graphics;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables
{
    public abstract class DrawableVitaruHitObject : DrawableSymcolHitObject<VitaruHitObject>, ITuneable
    {
        protected readonly ChapterSet ChapterSet = ChapterStore.GetChapterSet(VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Gamemode));

        protected Bindable<SoundsOptions> Sounds { get; private set; } = VitaruSettings.VitaruConfigManager.GetBindable<SoundsOptions>(VitaruSetting.Sounds);

        protected VitaruPlayfield VitaruPlayfield { get; private set; }

        public AspectLockedPlayfield CurrentPlayfield { get; set; }

        protected virtual double object_size => 0;

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

        public new bool Masking
        {
            get => base.Masking;
            set => base.Masking = value;
        }

        private bool untuned;

        public bool Editor { get; set; }

        public bool Preempted { get; private set; }

        public bool Started { get; private set; }        

        protected override JudgementResult CreateResult(Judgement judgement) => new VitaruJudgementResult(judgement);

        private readonly float unpreemt;
        private readonly float preemt;
        private readonly float start;
        private readonly float end;

        private bool die;

        protected DrawableVitaruHitObject(VitaruHitObject hitObject) : base(hitObject) { }

        protected DrawableVitaruHitObject(VitaruHitObject hitObject, VitaruPlayfield playfield) : base(hitObject)
        {
            VitaruPlayfield = playfield;
            CurrentPlayfield = playfield?.Gamefield;

            Sounds.ValueChanged += swap;
            OnDispose += () => Sounds.ValueChanged -= swap;

            void swap(SoundsOptions value)
            {
                switch(value)
                {
                    default:
                        RulesetAudio = null;
                        break;
                    case SoundsOptions.Classic:
                        RulesetAudio = MapMixerModSet.ClassicAudio;
                        break;
                    case SoundsOptions.Touhou:
                        RulesetAudio = VitaruRuleset.VitaruAudio;
                        break;
                }
            }

            Sounds.TriggerChange();

            AlwaysPresent = true;

            unpreemt = (float)hitObject.TimeUnPreempt;
            preemt = (float)hitObject.TimePreempt;
            start = (float)hitObject.StartTime;
            end = (float)hitObject.EndTime;

            VitaruRuleset.MEMORY_LEAKED.Value += object_size;
            OnDispose += () => VitaruRuleset.MEMORY_LEAKED.Value -= object_size;
        }

        protected override SymcolSkinnableSound GetSkinnableSound(SampleInfo info, SampleControlPoint point = null)
        {
            SampleControlPoint control = HitObject.SampleControlPoint;

            if (point != null)
                control = point;

            return new VitaruSkinableSound(HitObject.GetAdjustedSample(info, control)) { RulesetAudio = RulesetAudio };
        }

        protected override void SkinChanged(ISkinSource skin, bool allowFallback)
        {
            base.SkinChanged(skin, allowFallback);

            if (HitObject is IHasComboInformation combo && HitObject.ColorOverride == null)
                AccentColour = skin.GetValue<SkinConfiguration, Color4?>(s => s.ComboColours.Count > 0 ? s.ComboColours[combo.ComboIndex % s.ComboColours.Count] : (Color4?)null) ?? Color4.White;
            else
                AccentColour = HitObject.ColorOverride.GetValueOrDefault();
        }

        protected sealed override void UpdateState(ArmedState state)
        {
            double transformTime = HitObject.StartTime - HitObject.TimePreempt;

            ApplyTransformsAt(transformTime, true);
            ClearTransformsAfter(transformTime, true);
        }

        public override bool UpdateSubTree()
        {
            bool r = base.UpdateSubTree();

            if (die)
            {
                Dispose();
                die = false;
            }

            return r;
        }

        protected override void Update()
        {
            //base.Update();

            if (VitaruPlayfield.Current + preemt >= start && VitaruPlayfield.Current < end + unpreemt && !Preempted)
                Preempt();
            else if ((VitaruPlayfield.Current + preemt < start || VitaruPlayfield.Current >= end + unpreemt) && Preempted)
                UnPreempt();

            if (VitaruPlayfield.Current >= start && VitaruPlayfield.Current < end && !Started)
                Start();
            else if ((VitaruPlayfield.Current < start || VitaruPlayfield.Current >= end) && Started)
                End();
        }

        protected virtual void Preempt() => Preempted = true;

        protected virtual void Start() => Started = true;

        protected virtual void End() => Started = false;

        protected virtual void UnPreempt() => Preempted = false;

        /// <summary>
        /// Tells this to die ASAP
        /// </summary>
        public void Die() => die = true;

        protected override void Dispose(bool isDisposing)
        {
            Sounds.UnbindAll();
            VitaruPlayfield = null;
            CurrentPlayfield = null;
            ClearInternal();
            base.Dispose(isDisposing);
            Sounds = null;
        }
    }
}
