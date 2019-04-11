#region usings

using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
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

        public virtual bool Flip { get; set; }

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
            AlternateBindable.ValueChanged += valuechange =>
            {
                bool value = valuechange.NewValue;
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
                GetButton("Normal", "N", Color4.DarkRed, Color4.Red, nNormal, new Vector2(-ButtonSize - ButtonSize / 2, -ButtonSize), Flip ? Key.Number4 : Key.Number7),
                GetButton("Normal", "S", Color4.DarkBlue, Color4.Blue, sNormal, new Vector2(-ButtonSize - ButtonSize / 2, ButtonSize), Flip ? Key.F : Key.J),
                GetButton("Normal", "D", Color4.DarkGreen, Color4.Green, dNormal, new Vector2(-ButtonSize - ButtonSize / 2, 0), Flip ? Key.R : Key.U),

                //Whistle
                GetButton("Whistle", "N", Color4.DarkRed, Color4.Red, nWhistle, new Vector2(-ButtonSize / 2f, -ButtonSize), Flip ? Key.Number3 : Key.Number8),
                GetButton("Whistle", "S", Color4.DarkBlue, Color4.Blue, sWhistle, new Vector2(-ButtonSize / 2f, ButtonSize), Flip ? Key.D : Key.K),
                GetButton("Whistle", "D", Color4.DarkGreen, Color4.Green, dWhistle, new Vector2(-ButtonSize / 2f, 0), Flip ? Key.E : Key.I),

                //Finish
                GetButton("Finish", "N", Color4.DarkRed, Color4.Red, nFinish, new Vector2(ButtonSize / 2f, -ButtonSize), Flip ? Key.Number2 : Key.Number9),
                GetButton("Finish", "S", Color4.DarkBlue, Color4.Blue, sFinish, new Vector2(ButtonSize * 0.5f, ButtonSize), Flip ? Key.S : Key.L),
                GetButton("Finish", "D", Color4.DarkGreen, Color4.Green, dFinish, new Vector2(ButtonSize * 0.5f, 0), Flip ? Key.W : Key.O),

                //Clap
                GetButton("Clap", "N", Color4.DarkRed, Color4.Red, nClap, new Vector2(ButtonSize * 1.5f , -ButtonSize), Flip ? Key.Number1 : Key.Number0),
                GetButton("Clap", "S", Color4.DarkBlue, Color4.Blue, sClap, new Vector2(ButtonSize * 1.5f , ButtonSize), Flip ? Key.A : Key.Semicolon),
                GetButton("Clap", "D", Color4.DarkGreen, Color4.Green, dClap, new Vector2(ButtonSize * 1.5f, 0), Flip ? Key.Q : Key.P),
            };
        }

        protected virtual SymcolButton GetButton(string name, string label, Color4 top, Color4 bottom, SampleChannel sample, Vector2 pos, Key bind) => new SymcolButton
        {
            ButtonText = name,
            ButtonLabel = label,
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            ButtonColorTop = top,
            ButtonColorBottom = bottom,
            Size = ButtonSize,
            Action = () => playSample(sample),
            Position = Flip ? new Vector2(-pos.X, pos.Y) : pos,
            Bind = bind,
        };

        private void playSample(SampleChannel sample) => sample.Play();
    }
}
