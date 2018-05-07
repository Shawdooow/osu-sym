using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Configuration;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Vitaru.Characters.Players
{
    public class Sakuya : TouhosuPlayer
    {
        #region Fields
        public double SetRate { get; private set; } = 0.75d;

        public const double SakuyaHealth = 100;

        public const double SakuyaEnergy = 36;

        public const double SakuyaEnergyCost = 2;

        public const double SakuyaEnergyCostPerSecond = 4;

        public static readonly Color4 SakuyaPrimaryColor = Color4.Navy;

        public static readonly Color4 SakuyaSecondaryColor = OsuColour.FromHex("#92a0dd");

        public static readonly Color4 SakuyaComplementaryColor = OsuColour.FromHex("#d6d6d6");

        public override TouhosuCharacters PlayableCharacter => TouhosuCharacters.SakuyaIzayoi;

        public override double MaxHealth => SakuyaHealth;

        public override double MaxEnergy => SakuyaEnergy;

        public override double EnergyCost => SakuyaEnergyCost;

        public override double EnergyCostPerSecond => SakuyaEnergyCostPerSecond;

        public override Color4 PrimaryColor => SakuyaPrimaryColor;

        public override Color4 SecondaryColor => SakuyaSecondaryColor;

        public override Color4 ComplementaryColor => SakuyaComplementaryColor;

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

                    Energy -= (Clock.ElapsedFrameTime / 1000) * (1 / currentRate) * energyDrainMultiplier * EnergyCostPerSecond;

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
            if (VitaruPlayfield.VitaruInputManager.Shade != null)
            {
                if (speed > 1)
                {
                    VitaruPlayfield.VitaruInputManager.Shade.Colour = Color4.Cyan;
                    VitaruPlayfield.VitaruInputManager.Shade.Alpha = (float)(speed - 1) * 0.05f;
                }
                else if (speed == 1)
                    VitaruPlayfield.VitaruInputManager.Shade.Alpha = 0;
                else if (speed < 1 && speed > 0)
                {
                    VitaruPlayfield.VitaruInputManager.Shade.Colour = Color4.Orange;
                    VitaruPlayfield.VitaruInputManager.Shade.Alpha = (float)(1 - speed) * 0.05f;
                }
                else if (speed < 0)
                {
                    VitaruPlayfield.VitaruInputManager.Shade.Colour = Color4.Purple;
                    VitaruPlayfield.VitaruInputManager.Shade.Alpha = (float)-speed * 0.1f;
                }
            }

            if (clock is IHasPitchAdjust pitchAdjust)
                pitchAdjust.PitchAdjust = speed;

            SpeedMultiplier = 1 / speed;
        }

        protected override bool Pressed(VitaruAction action)
        {
            if (action == VitaruAction.Increase)
            {
                if (Actions[VitaruAction.Slow])
                    SetRate = Math.Min(Math.Round(SetRate + 0.05d, 2), 2d);
                else
                    SetRate = Math.Min(Math.Round(SetRate + 0.25d, 2), 2d);
            }
            if (action == VitaruAction.Decrease)
            {
                if (Actions[VitaruAction.Slow])
                    SetRate = Math.Max(Math.Round(SetRate - 0.05d, 2), 0.25d);
                else
                    SetRate = Math.Max(Math.Round(SetRate - 0.25d, 2), 0.25d);
            }

            return base.Pressed(action);
        }

        #region Touhosu Story Content
        public const string Background = "";
        #endregion
    }
}
