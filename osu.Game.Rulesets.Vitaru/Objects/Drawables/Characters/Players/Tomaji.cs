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
    public class Tomaji : VitaruPlayer
    {
        #region Fields
        public double SetRate { get; private set; } = 0.8d;

        public const double TomajiHealth = 80;

        public const double TomajiEnergy = 24;

        public const double TomajiEnergyCost = 2;

        public const double TomajiEnergyCostPerSecond = 4;

        public static readonly Color4 TomajiColor = Color4.OrangeRed;

        public override SelectableCharacters PlayableCharacter => SelectableCharacters.TomajiHakurei;

        public override double MaxHealth => TomajiHealth;

        public override double MaxEnergy => TomajiEnergy;

        public override double EnergyCost => TomajiEnergyCost;

        public override double EnergyCostPerSecond => TomajiEnergyCostPerSecond;

        public override Color4 CharacterColor => TomajiColor;

        private double originalRate;

        private double currentRate = 1;

        private readonly Bindable<WorkingBeatmap> workingBeatmap = new Bindable<WorkingBeatmap>();
        #endregion

        public Tomaji(VitaruPlayfield playfield) : base(playfield)
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

        //Currently ripped straight from old Sakuya, needs updating
        protected override bool Pressed(VitaruAction action)
        {
            bool late = true;

            if (false)
            {
                if (action == VitaruAction.Increase && !late)
                    SetRate = Math.Min(Math.Round(SetRate + 0.2d, 1), 0.8d);
                else if (action == VitaruAction.Increase && late)
                    SetRate = Math.Min(Math.Round(SetRate + 0.2d, 1), 1.2d);
                if (action == VitaruAction.Decrease && !late)
                    SetRate = Math.Max(Math.Round(SetRate - 0.2d, 1), 0.4d);
                else if (action == VitaruAction.Decrease && late)
                    SetRate = Math.Max(Math.Round(SetRate - 0.2d, 1), 0.2d);
            }

            return base.Pressed(action);
        }

        #region Touhosu Story Content
        public const string Background = "Tomaji has always been over shadowed by his older sister Ryukoy who is next in line to be the Hakurei Maiden, though he has never minded. " +
            "He had the option to take of to some exotic place far away if he wanted, but he didn't. " +
            "Despite having the entire world to explore he would be happy standing at his sister's side as any kind of help that he could be. " +
            "To him family was the most important and he knew she felt the same way. Even thought she would wear the title they would share the burden.";
        #endregion
    }
}
