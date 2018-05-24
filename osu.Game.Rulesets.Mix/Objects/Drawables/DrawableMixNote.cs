using osu.Game.Rulesets.Mix.Objects.Drawables.Pieces;
using osu.Framework.Graphics;
using OpenTK;
using OpenTK.Graphics;
using osu.Game.Rulesets.Mix.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;
using osu.Game.Audio;
using osu.Framework.Input.Bindings;
using System;

namespace osu.Game.Rulesets.Mix.Objects.Drawables
{
    public class DrawableMixNote : DrawableMixHitObject, IKeyBindingHandler<MixAction>
    {
        private SkinnableSound sample;

        private readonly MixNote note;

        private readonly MixAction[] binds;

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            if (!userTriggered)
            {
                if (!HitObject.HitWindows.CanBeHit(timeOffset))
                {
                    AddJudgement(new MixJudgement { Result = HitResult.Miss });
                    Delete();
                }
                return;
            }

            var result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None)
                return;

            AddJudgement(new MixJudgement
            {
                Result = result,
                PositionOffset = Vector2.Zero
            });
            sample.Play();

            this.FadeOutFromOne(100)
                .OnComplete((m) => Delete());
        }

        public DrawableMixNote(MixNote note) : base(note)
        {
            this.note = note;

            Size = new Vector2(56);

            Anchor = Anchor.CentreLeft;
            Origin = Anchor.Centre;

            Child = new Note(note);

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

            sample = new SkinnableSound(new SampleInfo
            {
                Bank = bank,
                Name = name,
                Volume = note.Volume > 0 ? note.Volume : HitObject.SampleControlPoint.SampleVolume,
                Namespace = SampleNamespace
            });
            Add(sample);

            if (bank == "normal")
            {
                if (name == "hitnormal")
                    binds = new MixAction[]
                    {
                        MixAction.NormalNormalLeft,
                        MixAction.NormalNormalRight
                    };
                else if (name == "hitwhistle")
                    binds = new MixAction[]
                    {
                        MixAction.NormalWhistleLeft,
                        MixAction.NormalWhistleRight
                    };
                else if (name == "hitfinish")
                    binds = new MixAction[]
                    {
                        MixAction.NormalFinishLeft,
                        MixAction.NormalFinishRight
                    };
                else if (name == "hitclap")
                    binds = new MixAction[]
                    {
                        MixAction.NormalClapLeft,
                        MixAction.NormalClapRight
                    };
                else
                    throw new Exception("Go tell Shawdooow its fucked!");
            }
            else if (bank == "drum")
            {
                if (name == "hitnormal")
                    binds = new MixAction[]
                    {
                        MixAction.DrumNormalLeft,
                        MixAction.DrumNormalRight
                    };
                else if (name == "hitwhistle")
                    binds = new MixAction[]
                    {
                        MixAction.DrumWhistleLeft,
                        MixAction.DrumWhistleRight
                    };
                else if (name == "hitfinish")
                    binds = new MixAction[]
                    {
                        MixAction.DrumFinishLeft,
                        MixAction.DrumFinishRight
                    };
                else if (name == "hitclap")
                    binds = new MixAction[]
                    {
                        MixAction.DrumClapLeft,
                        MixAction.DrumClapRight
                    };
                else
                    throw new Exception("Go tell Shawdooow its fucked!");
            }
            else if (bank == "soft")
            {
                if (name == "hitnormal")
                    binds = new MixAction[]
                    {
                        MixAction.SoftNormalLeft,
                        MixAction.SoftNormalRight
                    };
                else if (name == "hitwhistle")
                    binds = new MixAction[]
                    {
                        MixAction.SoftWhistleLeft,
                        MixAction.SoftWhistleRight
                    };
                else if (name == "hitfinish")
                    binds = new MixAction[]
                    {
                        MixAction.SoftFinishLeft,
                        MixAction.SoftFinishRight
                    };
                else if (name == "hitclap")
                    binds = new MixAction[]
                    {
                        MixAction.SoftClapLeft,
                        MixAction.SoftClapRight
                    };
                else
                    throw new Exception("Go tell Shawdooow its fucked!");
            }
            else
                throw new Exception("Go tell Shawdooow its fucked!");
        }

        public override bool OnPressed(MixAction action)
        {
            foreach (MixAction bind in binds)
                if (bind == action)
                    return UpdateJudgement(true);
            return false;
        }
    }
}
