#region usings

using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

#endregion

namespace osu.Core.Containers.Shawdooow
{
    public class HitSoundBoard : Container
    {
        public int ButtonSize = 100;

        public Bindable<bool> AlternateBindable = new Bindable<bool> { Default = false, Value = false };
        public AudioManager AlternateAudioManager;

        private SampleChannel nNormal;
        private SampleChannel sNormal;
        private SampleChannel dNormal;

        private SampleChannel nWhistle;
        private SampleChannel sWhistle;
        private SampleChannel dWhistle;

        private SampleChannel nFinish;
        private SampleChannel sFinish;
        private SampleChannel dFinish;

        private SampleChannel nClap;
        private SampleChannel sClap;
        private SampleChannel dClap;

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            AlternateBindable.ValueChanged += value =>
            {
                if (!value)
                {
                    nNormal = audio.Sample.Get($@"Gameplay/normal-hitnormal");
                    sNormal = audio.Sample.Get($@"Gameplay/soft-hitnormal");
                    dNormal = audio.Sample.Get($@"Gameplay/drum-hitnormal");

                    nWhistle = audio.Sample.Get($@"Gameplay/normal-hitwhistle");
                    sWhistle = audio.Sample.Get($@"Gameplay/soft-hitwhistle");
                    dWhistle = audio.Sample.Get($@"Gameplay/drum-hitwhistle");

                    nFinish = audio.Sample.Get($@"Gameplay/normal-hitfinish");
                    sFinish = audio.Sample.Get($@"Gameplay/soft-hitfinish");
                    dFinish = audio.Sample.Get($@"Gameplay/drum-hitfinish");

                    nClap = audio.Sample.Get($@"Gameplay/normal-hitclap");
                    sClap = audio.Sample.Get($@"Gameplay/soft-hitclap");
                    dClap = audio.Sample.Get($@"Gameplay/drum-hitclap");
                }
                else
                {
                    nNormal = AlternateAudioManager.Sample.Get($@"Gameplay/normal-hitnormal");
                    sNormal = AlternateAudioManager.Sample.Get($@"Gameplay/soft-hitnormal");
                    dNormal = AlternateAudioManager.Sample.Get($@"Gameplay/drum-hitnormal");

                    nWhistle = AlternateAudioManager.Sample.Get($@"Gameplay/normal-hitwhistle");
                    sWhistle = AlternateAudioManager.Sample.Get($@"Gameplay/soft-hitwhistle");
                    dWhistle = AlternateAudioManager.Sample.Get($@"Gameplay/drum-hitwhistle");

                    nFinish = AlternateAudioManager.Sample.Get($@"Gameplay/normal-hitfinish");
                    sFinish = AlternateAudioManager.Sample.Get($@"Gameplay/soft-hitfinish");
                    dFinish = AlternateAudioManager.Sample.Get($@"Gameplay/drum-hitfinish");

                    nClap = AlternateAudioManager.Sample.Get($@"Gameplay/normal-hitclap");
                    sClap = AlternateAudioManager.Sample.Get($@"Gameplay/soft-hitclap");
                    dClap = AlternateAudioManager.Sample.Get($@"Gameplay/drum-hitclap");
                }
            };
            AlternateBindable.TriggerChange();

            Children = new Drawable[]
            {
                //Noramal
                new SymcolButton
                {
                    ButtonText = "Normal",
                    ButtonLabel = "N",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    Size = ButtonSize,
                    Action = () => playSample(nNormal),
                    Position = new Vector2(-ButtonSize - ButtonSize / 2, -ButtonSize),
                    Bind = Key.Number1
                },
                new SymcolButton
                {
                    ButtonText = "Normal",
                    ButtonLabel = "S",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkBlue,
                    ButtonColorBottom = Color4.Blue,
                    Size = ButtonSize,
                    Action = () => playSample(sNormal),
                    Position = new Vector2(-ButtonSize - ButtonSize / 2, ButtonSize),
                    Bind = Key.A
                },
                new SymcolButton
                {
                    ButtonText = "Normal",
                    ButtonLabel = "D",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkGreen,
                    ButtonColorBottom = Color4.Green,
                    Size = ButtonSize,
                    Action = () => playSample(dNormal),
                    Position = new Vector2(-ButtonSize - ButtonSize / 2, 0),
                    Bind = Key.Q
                },

                //Whistle
                new SymcolButton
                {
                    ButtonText = "Whistle",
                    ButtonLabel = "N",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    Size = ButtonSize,
                    Action = () => playSample(nWhistle),
                    Position = new Vector2(-ButtonSize / 2f, -ButtonSize),
                    Bind = Key.Number2
                },
                new SymcolButton
                {
                    ButtonText = "Whistle",
                    ButtonLabel = "S",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkBlue,
                    ButtonColorBottom = Color4.Blue,
                    Size = ButtonSize,
                    Action = () => playSample(sWhistle),
                    Position = new Vector2(-ButtonSize / 2f, ButtonSize),
                    Bind = Key.S
                },
                new SymcolButton
                {
                    ButtonText = "Whistle",
                    ButtonLabel = "D",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkGreen,
                    ButtonColorBottom = Color4.Green,
                    Size = ButtonSize,
                    Action = () => playSample(dWhistle),
                    Position = new Vector2(-ButtonSize / 2f, 0),
                    Bind = Key.W
                },

                //Finish
                new SymcolButton
                {
                    ButtonText = "Finish",
                    ButtonLabel = "N",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    Size = ButtonSize,
                    Action = () => playSample(nFinish),
                    Position = new Vector2(ButtonSize / 2f, -ButtonSize),
                    Bind = Key.Number3
                },
                new SymcolButton
                {
                    ButtonText = "Finish",
                    ButtonLabel = "S",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkBlue,
                    ButtonColorBottom = Color4.Blue,
                    Size = ButtonSize,
                    Action = () => playSample(sFinish),
                    Position = new Vector2(ButtonSize * 0.5f, ButtonSize),
                    Bind = Key.D
                },
                new SymcolButton
                {
                    ButtonText = "Finish",
                    ButtonLabel = "D",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkGreen,
                    ButtonColorBottom = Color4.Green,
                    Size = ButtonSize,
                    Action = () => playSample(dFinish),
                    Position = new Vector2(ButtonSize * 0.5f, 0),
                    Bind = Key.E
                },

                //Clap
                new SymcolButton
                {
                    ButtonText = "Clap",
                    ButtonLabel = "N",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkRed,
                    ButtonColorBottom = Color4.Red,
                    Size = ButtonSize,
                    Action = () => playSample(nClap),
                    Position = new Vector2(ButtonSize * 1.5f , -ButtonSize),
                    Bind = Key.Number4
                },
                new SymcolButton
                {
                    ButtonText = "Clap",
                    ButtonLabel = "S",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkBlue,
                    ButtonColorBottom = Color4.Blue,
                    Size = ButtonSize,
                    Action = () => playSample(sClap),
                    Position = new Vector2(ButtonSize * 1.5f , ButtonSize),
                    Bind = Key.F
                },
                new SymcolButton
                {
                    ButtonText = "Clap",
                    ButtonLabel = "D",
                    Depth = -2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    ButtonColorTop = Color4.DarkGreen,
                    ButtonColorBottom = Color4.Green,
                    Size = ButtonSize,
                    Action = () => playSample(dClap),
                    Position = new Vector2(ButtonSize * 1.5f, 0),
                    Bind = Key.R
                },
            };
        }

        private void playSample(SampleChannel sample)
        {
            sample.Play();
        }
    }
}
