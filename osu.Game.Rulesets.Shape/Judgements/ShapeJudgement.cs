using OpenTK;
using osu.Game.Rulesets.Shape.Objects.Drawables;
using Symcol.Rulesets.Core.Judgements;

namespace osu.Game.Rulesets.Shape.Judgements
{
    public class ShapeJudgement : SymcolJudgement
    {
        /// <summary>
        /// The positional hit offset.
        /// </summary>
        public Vector2 PositionOffset;

        public ComboResult Combo;
    }
}
