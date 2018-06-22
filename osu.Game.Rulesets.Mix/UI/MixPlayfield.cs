using osu.Framework.Graphics;
using osu.Game.Rulesets.Mix.Objects;
using osu.Game.Rulesets.Mix.Objects.Drawables;
using OpenTK;
using osu.Game.Rulesets.Mix.Judgements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.UI.Scrolling;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Mix.UI
{
    public class MixPlayfield : ScrollingPlayfield
    {
        public const float DEFAULT_HEIGHT = 60;

        private List<Row> rows = new List<Row>();

        public MixPlayfield() : base(ScrollingDirection.Left)
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;

            RelativeSizeAxes = Axes.Both;

            Row row;
            rows.Add(row = new Row(rows.Count + 1));
            AddNested(row);
            Add(row);
        }

        private double currentRow;
        private double lastObjectStartTime = double.MinValue;

        public override void Add(DrawableHitObject h)
        {
            h.OnJudgement += onJudgement;

            if (lastObjectStartTime == h.HitObject.StartTime && currentRow >= rows.Count)
            {
                Row row;
                AddNested(row = new Row(rows.Count + 1));
                Add(row);
                rows.Add(row);
                row.Add(h);
                currentRow++;
            }
            else if (lastObjectStartTime == h.HitObject.StartTime)
            {
                currentRow++;
                foreach (Row row in rows)
                    if (row.RowNumber == currentRow)
                        row.Add(h);
            }
            else
            {
                rows.First().Add(h);
                currentRow = 1;
            }

            lastObjectStartTime = h.HitObject.StartTime;
        }

        private void onJudgement(DrawableHitObject judgedObject, Judgement judgement)
        {
            var shapeJudgement = (MixJudgement)judgement;
            var shapeObject = (MixHitObject)judgedObject.HitObject;

            DrawableMixJudgement explosion = new DrawableMixJudgement(shapeJudgement, judgedObject)
            {
                Scale = new Vector2(0.5f),
                Alpha = 0.5f,
                Origin = Anchor.Centre,
                Position = judgedObject.Position,
            };

            //judgementLayer.Add(explosion);
        }
    }
}
