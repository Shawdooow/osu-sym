using System;
using OpenTK;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Mix.Objects.Drawables;

namespace osu.Game.Rulesets.Mix.Judgements
{
    public class MixJudgement : Judgement
    {
        /// <summary>
        /// The positional hit offset.
        /// </summary>
        public Vector2 PositionOffset;

        public ComboResult Combo;
    }
}
