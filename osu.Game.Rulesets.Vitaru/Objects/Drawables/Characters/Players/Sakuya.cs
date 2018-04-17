using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Configuration;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters.Pieces;
using osu.Game.Rulesets.Vitaru.UI;
using System;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters.Players
{
    public class Sakuya : Player
    {
        #region Fields
        public double SetRate { get; private set; } = 0.8d;

        public const double SakuyaHealth = 80;

        public const double SakuyaEnergy = 24;

        public const double SakuyaEnergyCost = 2;

        public const double SakuyaEnergyCostPerSecond = 4;

        public static readonly Color4 SakuyaColor = Color4.Navy;

        public override SelectableCharacters PlayableCharacter => SelectableCharacters.SakuyaIzayoi;

        public override double MaxHealth => SakuyaHealth;

        public override double MaxEnergy => SakuyaEnergy;

        public override double EnergyCost => SakuyaEnergyCost;

        public override double EnergyCostPerSecond => SakuyaEnergyCostPerSecond;

        public override Color4 CharacterColor => SakuyaColor;

        private double originalRate;

        private double currentRate = 1;

        private readonly Bindable<WorkingBeatmap> workingBeatmap = new Bindable<WorkingBeatmap>();
        #endregion

        public Sakuya(VitaruPlayfield playfield) : base(playfield)
        {
            Spell += (action) =>
            {
                if (originalRate == 0)
                    originalRate = (float)workingBeatmap.Value.Track.Rate;

                currentRate = originalRate * SetRate;
                applyToClock(workingBeatmap.Value.Track, currentRate);

                SpellEndTime = Time.Current + 1000;
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuGameBase game)
        {
            workingBeatmap.BindTo(game.Beatmap);
        }

        //TODO: use new seal system in Player, this should not be neccesary
        protected override void LoadComplete()
        {
            base.LoadComplete();

            Add(new Seal(this));
            Remove(Sign);
        }

        protected override void SpellUpdate()
        {
            base.SpellUpdate();

            if (SpellEndTime >= Time.Current)
                if (!SpellActive)
                {
                    currentRate += (float)Clock.ElapsedFrameTime / 100;
                    if (currentRate > originalRate)
                        currentRate = originalRate;
                    applyToClock(workingBeatmap.Value.Track, currentRate);
                    if (currentRate > 0 && SpellEndTime - 500 <= Time.Current)
                    {
                        currentRate = originalRate;
                        applyToClock(workingBeatmap.Value.Track, currentRate);
                    }
                    else if (currentRate < 0 && SpellEndTime + 500 >= Time.Current)
                    {
                        currentRate = originalRate;
                        applyToClock(workingBeatmap.Value.Track, currentRate);
                    }
                }
                else
                {
                    double energyDrainMultiplier = 0;
                    if (currentRate < 1)
                        energyDrainMultiplier = 1 - currentRate;
                    else if (currentRate >= 1)
                        energyDrainMultiplier = currentRate - 1;

                    Energy -= Clock.ElapsedFrameTime / 1000 * (1 / currentRate) * EnergyCostPerSecond * energyDrainMultiplier;

                    if (currentRate > 0)
                        SpellEndTime = Time.Current + 2000;
                    else
                        SpellEndTime = Time.Current - 2000;

                    currentRate = originalRate * SetRate;
                    applyToClock(workingBeatmap.Value.Track, currentRate);
                }
        }

        private void applyToClock(IAdjustableClock clock, double speed)
        {
            if (VitaruInputManager.Shade != null)
            {
                if (speed > 1)
                {
                    VitaruInputManager.Shade.Colour = Color4.Cyan;
                    VitaruInputManager.Shade.Alpha = (float)(speed - 1) * 0.05f;
                }
                else if (speed == 1)
                    VitaruInputManager.Shade.Alpha = 0;
                else if (speed < 1 && speed > 0)
                {
                    VitaruInputManager.Shade.Colour = Color4.Orange;
                    VitaruInputManager.Shade.Alpha = (float)(1 - speed) * 0.05f;
                }
                else if (speed < 0)
                {
                    VitaruInputManager.Shade.Colour = Color4.Purple;
                    VitaruInputManager.Shade.Alpha = (float)-speed * 0.1f;
                }
            }

            if (clock is IHasPitchAdjust pitchAdjust)
                pitchAdjust.PitchAdjust = speed;
            SpeedMultiplier = 1 / speed;
        }

        protected override bool Pressed(VitaruAction action)
        {
            if (action == VitaruAction.Increase)
                SetRate = Math.Min(Math.Round(SetRate + 0.2d, 1), 1.2d);
            if (action == VitaruAction.Decrease)
                SetRate = Math.Max(Math.Round(SetRate - 0.2d, 1), 0.2d);

            return base.Pressed(action);
        }

        #region Touhosu Story Content
        public const string Background = "";
        #endregion
    }
}
