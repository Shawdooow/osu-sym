using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using Symcol.Core.Graphics.Sprites;
using System;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.DrawableTouhosuPlayers
{
    public class DrawableSakuya : DrawableTouhosuPlayer
    {
        #region Fields
        private readonly GraphicsPresets graphics = VitaruSettings.VitaruConfigManager.GetBindable<GraphicsPresets>(VitaruSetting.GraphicsPresets);

        //protected override string CharacterName => graphics == GraphicsPresets.StandardV2 ? Player.Name : Player.FileName;

        public double SetRate { get; private set; } = 0.75d;

        private double originalRate;

        private double currentRate = 1;

        private readonly Bindable<WorkingBeatmap> workingBeatmap = new Bindable<WorkingBeatmap>();
        #endregion

        public DrawableSakuya(VitaruPlayfield playfield, VitaruNetworkingClientHandler vitaruNetworkingClientHandler) : base(playfield, new Sakuya(), vitaruNetworkingClientHandler)
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

        protected override void LoadAnimationSprites(TextureStore textures, Storage storage)
        {
            if (graphics == GraphicsPresets.StandardV2)
            {

                SoulContainer.Alpha = 0;
                KiaiContainer.Alpha = 1;

                KiaiLeftSprite.Alpha = 0;
                KiaiRightSprite.Alpha = 0;
                KiaiStillSprite.Alpha = 0;

                KiaiContainer.Add(new AnimatedSprite()
                {
                    RelativeSizeAxes = Axes.Both,
                    UpdateRate = 100,
                    Textures = new Texture[]
                    {
                        VitaruSkinElement.LoadSkinElement(Player.Name + " Kiai 0", storage),
                        VitaruSkinElement.LoadSkinElement(Player.Name + " Kiai 1", storage),
                        VitaruSkinElement.LoadSkinElement(Player.Name + " Kiai 2", storage),
                        VitaruSkinElement.LoadSkinElement(Player.Name + " Kiai 3", storage),
                        VitaruSkinElement.LoadSkinElement(Player.Name + " Kiai 4", storage),
                        VitaruSkinElement.LoadSkinElement(Player.Name + " Kiai 5", storage),
                        VitaruSkinElement.LoadSkinElement(Player.Name + " Kiai 6", storage),
                        VitaruSkinElement.LoadSkinElement(Player.Name + " Kiai 7", storage),
                    }
                });
            }
            else
                base.LoadAnimationSprites(textures, storage);

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

                    Energy -= (Clock.ElapsedFrameTime / 1000) * (1 / currentRate) * energyDrainMultiplier * TouhosuPlayer.EnergyDrainRate;

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
    }
}
