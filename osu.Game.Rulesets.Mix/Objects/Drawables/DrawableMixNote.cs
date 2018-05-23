using osu.Game.Rulesets.Mix.Objects.Drawables.Pieces;
using osu.Framework.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using OpenTK;
using OpenTK.Graphics;
using osu.Game.Rulesets.Mix.Judgements;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Mix.Objects.Drawables
{
    public class DrawableMixNote : DrawableMixHitObject
    {
        private SampleChannel sample;

        private readonly MixNote note;

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            base.CheckForJudgements(userTriggered, timeOffset);

            if (Time.Current >= note.StartTime)
            {
                AddJudgement(new MixJudgement { Result = HitResult.Great });
                sample?.Play();

                this.FadeOutFromOne(100)
                    .OnComplete((drawableNote) => Delete());
            }
        }

        public DrawableMixNote(MixNote note) : base(note)
        {
            this.note = note;

            Size = new Vector2(64);

            Anchor = Anchor.CentreLeft;
            Origin = Anchor.Centre;

            Child = new Note(note.Color);
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            string bank = "normal";
            string name = "hitnormal";

            if (note.Color == Color4.Green)
                bank = "drum";
            else if (note.Color == Color4.Blue)
                bank = "soft";

            if (note.Whistle)
                name = "hitwhistle";
            else if (note.Finish)
                name = "hitfinish";
            else if (note.Clap)
                name = "hitclap";

            sample = audio.Sample.Get($"Gameplay/{bank}-{name}");
        }

        public override bool OnPressed(MixAction action)
        {
            return false;
        }
    }
}
