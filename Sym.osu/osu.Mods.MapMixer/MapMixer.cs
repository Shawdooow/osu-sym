#region usings

using osu.Core.Containers.Shawdooow;
using osu.Core.Screens.Evast;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Track;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Screens;
using osu.Framework.Timing;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Configuration;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osu.Game.Screens.Play;
using osu.Mods.Evast.Visualizers;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

#endregion

namespace osu.Mods.MapMixer
{
    public class MapMixer : BeatmapScreen
    {
        private SettingsSlider<double> clockPitch;
        private SettingsSlider<double> clockSpeed;
        private double pitch = 1;
        private double speed = 1;
        public static BindableDouble ClockPitch;
        public static BindableDouble ClockSpeed;
        private Bindable<double> dimLevel;
        private static readonly Bindable<bool> sync_pitch = new Bindable<bool> { Default = true, Value = true };
        private static readonly Bindable<bool> classic_sounds = new Bindable<bool> { Default = false, Value = false };
        protected override float BackgroundBlur => 10;

        private double play = double.MinValue;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, OsuConfigManager config)
        {
            dimLevel = config.GetBindable<double>(OsuSetting.DimLevel);

            ClockPitch = new BindableDouble { MinValue = 0f, Default = 1, Value = pitch, MaxValue = 2 };
            ClockSpeed = new BindableDouble { MinValue = 0f, Default = 1, Value = speed, MaxValue = 2 };

            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    Position = new Vector2(0 , 25),
                    RelativeSizeAxes = Axes.Both,
                    Origin = Anchor.TopCentre,
                    Anchor = Anchor.TopCentre,

                    Size = new Vector2(0.3f, 0.16f),

                    Children = new Drawable[]
                    {
                        new SettingsCheckbox
                        {
                            Origin = Anchor.TopCentre,
                            Anchor = Anchor.TopCentre,
                            Bindable = sync_pitch,
                            LabelText = "Sync Pitch"
                        },
                        new SettingsCheckbox
                        {
                            Origin = Anchor.TopCentre,
                            Anchor = Anchor.TopCentre,
                            Bindable = classic_sounds,
                            LabelText = "Classic"
                        }
                    }
                },
                new Container
                {
                    Position = new Vector2(0 , -25),
                    Height = 50,
                    RelativeSizeAxes = Axes.X,
                    Origin = Anchor.BottomCentre,
                    Anchor = Anchor.BottomCentre,
                    Children = new Drawable[]
                    {
                        clockPitch = new SettingsSlider<double>
                        {
                            Origin = Anchor.BottomCentre,
                            Anchor = Anchor.Centre,
                            LabelText = "Pitch",
                            Bindable = ClockPitch,
                            KeyboardStep = 0.05f,
                        },
                        clockSpeed = new SettingsSlider<double>
                        {
                            Origin = Anchor.TopCentre,
                            Anchor = Anchor.Centre,
                            LabelText = "Clock Speed",
                            Bindable = ClockSpeed,
                            KeyboardStep = 0.05f
                        }
                    }
                },
                new SymcolButton
                {
                    ButtonText = "Play",
                    Depth = -2,
                    Origin = Anchor.BottomRight,
                    Anchor = Anchor.BottomRight,
                    ButtonColorTop = Color4.DarkBlue,
                    ButtonColorBottom = Color4.Blue,
                    Size = 50,
                    Action = () =>
                    {
                        Push(new Player());
                        play = Time.Current;
                    },
                    Position = new Vector2(-60 , -100),
                },
                new SymcolButton
                {
                    ButtonText = "BG",
                    Depth = -2,
                    Origin = Anchor.BottomRight,
                    Anchor = Anchor.BottomRight,
                    ButtonColorTop = Color4.DarkBlue,
                    ButtonColorBottom = Color4.Blue,
                    Size = 50,
                    Action = () => Push(new Visualizer()),
                    Position = new Vector2(-60 , -175),
                },
                new SymcolButton
                {
                    ButtonText = "Back",
                    Origin = Anchor.BottomLeft,
                    Anchor = Anchor.BottomLeft,
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    Size = 80,
                    Action = this.Exit,
                    Position = new Vector2(60 , -100),
                },

                //Sounds Bar
                new MusicBar
                {
                    RelativeSizeAxes = Axes.X,
                    Origin = Anchor.TopCentre,
                    Anchor = Anchor.TopCentre,
                    Position = new Vector2(0 , 180),
                },
                new HitSoundBoard
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    AlternateBindable = classic_sounds,
                    AlternateAudioManager = MapMixerModSet.ClassicAudio
                },

                //Pitch Settings
                new SymcolButton
                {
                    ButtonText = "1x",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkGoldenrod,
                    ButtonColorBottom = Color4.Goldenrod,
                    Size = 60,
                    Action = () => changeClockSpeeds(1f),
                    Position = new Vector2(0 , 250),
                    Bind = Key.N
                },
                new SymcolButton
                {
                    ButtonText = "1.5x",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkGoldenrod,
                    ButtonColorBottom = Color4.Goldenrod,
                    Size = 60,
                    Action = () => changeClockSpeeds(1.5f),
                    Position = new Vector2(200 , 250),
                    Bind = Key.M
                },
                new SymcolButton
                {
                    ButtonText = "0.75x",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkGoldenrod,
                    ButtonColorBottom = Color4.Goldenrod,
                    Size = 60,
                    Action = () => changeClockSpeeds(0.75f),
                    Position = new Vector2(-200, 250),
                    Bind = Key.B
                },
            };

            ClockPitch.ValueChanged += value =>
            {
                applyRateAdjustments();
                if (sync_pitch.Value)
                    ClockSpeed.Value = ClockPitch.Value;
            };
            ClockSpeed.ValueChanged += value =>
            {
                applyRateAdjustments();
                if (sync_pitch.Value)
                    ClockPitch.Value = ClockSpeed.Value;
            };
            sync_pitch.ValueChanged += value =>
            {
                if (value)
                    ClockPitch.Value = ClockSpeed.Value;
            };

            Beatmap.ValueChanged += value =>
            {
                ClockPitch.TriggerChange();
                ClockSpeed.TriggerChange();
            };
        }

        protected override void Update()
        {
            base.Update();
            if (play != double.MinValue && Time.Current < play + 1000)
                applyRateAdjustments();
            else if (play != double.MinValue)
                play = double.MinValue;
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);
            setClockSpeed(Beatmap.Value.Track);
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);
            setClockSpeed(Beatmap.Value.Track);
            Beatmap.Value.Track.Start();
        }

        private void changeClockSpeeds(float value)
        {
            ClockPitch.Value = value;
            ClockSpeed.Value = value;
        }

        private void setClockSpeed(IAdjustableClock clock)
        {
            if (clock is IHasPitchAdjust pitchAdjust)
                clockPitch.Bindable.Value = pitchAdjust.PitchAdjust;
            clockSpeed.Bindable.Value = clock.Rate;
        }

        private void applyRateAdjustments()
        {
            if (Beatmap.Value.Track != null)
                applyToClock(Beatmap.Value.Track);
        }

        private void applyToClock(IAdjustableClock clock)
        {
            if (clock is IHasPitchAdjust pitchAdjust)
            {
                if (ClockPitch.Value > 1)
                {
                    pitchAdjust.PitchAdjust = ClockPitch.Value;
                    pitch = pitchAdjust.PitchAdjust;

                    if (clockSpeed.Bindable.Value > 1)
                        clock.Rate = ClockSpeed.Value - (ClockPitch.Value - 1) / 2 - (ClockSpeed.Value - 1) / 2;
                    else
                        clock.Rate = ClockSpeed.Value - (ClockPitch.Value - 1) / 2 + (ClockSpeed.Value - 1) * 0.5f;

                    speed = clock.Rate;
                }
                else if (ClockPitch.Value < 1)
                {
                    pitchAdjust.PitchAdjust = ClockPitch.Value;
                    pitch = pitchAdjust.PitchAdjust;

                    if (clockSpeed.Bindable.Value < 1)
                        clock.Rate = ClockSpeed.Value + (ClockPitch.Value - 1) * -2 + (ClockSpeed.Value - 1) * 0.5f;
                    else
                        clock.Rate = ClockSpeed.Value + (ClockPitch.Value - 1) * -2 - (ClockSpeed.Value - 1) / 2;

                    speed = clock.Rate;
                }
                else
                {
                    pitchAdjust.PitchAdjust = ClockPitch.Value;
                    pitch = pitchAdjust.PitchAdjust;

                    clock.Rate = ClockSpeed.Value;

                    speed = clock.Rate;
                }
            }
                
            else
                clock.Rate = ClockPitch.Value;
        }
    }

    internal class MusicBar : BeatSyncedContainer
    {
        private Box seekBar;
        private float beatLength = 1;
        private float lastBeatTime = 1;
        private int measure;
        private float measureLength = 1;
        private float lastMeasureTime = 1;

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Children = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.White,
                    Size = new Vector2(600, 4),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new Box
                {
                    Position = new Vector2(300, 0),
                    Colour = Color4.White,
                    Size = new Vector2(4, 30),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new Box
                {
                    Position = new Vector2(-300, 0),
                    Colour = Color4.White,
                    Size = new Vector2(4, 30),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new Box
                {
                    Position = new Vector2(-300f / 2f, 0),
                    Colour = Color4.White,
                    Size = new Vector2(3.5f, 22),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new Box
                {
                    Position = new Vector2(-0 , 0),
                    Colour = Color4.White,
                    Size = new Vector2(4, 26),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new Box
                {
                    Position = new Vector2(300f / 2f, 0),
                    Colour = Color4.White,
                    Size = new Vector2(3.5f, 22),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                seekBar = new Box
                {
                    Position = new Vector2(-300, 0),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(2, 20),
                },
            };
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);
            beatLength = (float)timingPoint.BeatLength;
            measureLength = (float)timingPoint.BeatLength * 4;
            if (lastMeasureTime <= (float)(Beatmap.Value.Track.CurrentTime - measureLength * 0.9f) || lastMeasureTime > (float)Beatmap.Value.Track.CurrentTime)
                lastMeasureTime = (float)Beatmap.Value.Track.CurrentTime;
            lastBeatTime = (float)Beatmap.Value.Track.CurrentTime;
            if(MapMixer.ClockPitch.Value > 0)
                measure++;
            if (MapMixer.ClockPitch.Value < 0)
                measure--;
            if (measure > 4)
                measure = 1;
            if (measure < 1)
                measure = 4;
        }

        protected override void Update()
        {
            base.Update();

            if (Beatmap.Value.Track.IsRunning)
                updateSeekBarPosition();
        }

        private void updateSeekBarPosition()
        {
            measure = (int)(((float)Beatmap.Value.Track.CurrentTime - lastMeasureTime) / measureLength * 4);
            float minX = measure * 150;
            
            Vector2 position = new Vector2(((float)Beatmap.Value.Track.CurrentTime - lastBeatTime) / beatLength * 150 + 300, 0);
            
            position.X %= 150;
            position.X += minX - 300;

            seekBar.Position = position;
        }

        private void halfBeat()
        {

        }

        private void quarterBeat()
        {

        }

        private void generateMeasure(float x)
        {

        }
    }



    internal class MusicBarTick : Container
    {
        private Box box;
        private Container glow;

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Children = new Drawable[]
            {
                box = new Box
                {
                    Depth = -2,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                glow = new Container
                {
                    Alpha = 0.25f,
                    Depth = 0,
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Radius = 4,
                    },
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    }
                },
            };
        }
        public void Activate(float beatLength , float flashIntensity)
        {
            glow.Alpha = 0.5f * flashIntensity;
            glow.FadeTo(0.25f, beatLength);
        }
    }
}
