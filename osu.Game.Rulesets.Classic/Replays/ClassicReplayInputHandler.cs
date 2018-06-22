// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using osu.Framework.Input;
using osu.Game.Rulesets.Replays;
using OpenTK;
using System.Linq;
using osu.Framework.MathUtils;

namespace osu.Game.Rulesets.Classic.Replays
{
    public class ClassicReplayInputHandler : FramedReplayInputHandler<ClassicReplayFrame>
    {
        public ClassicReplayInputHandler(Replay replay)
            : base(replay)
        {
        }

        protected override bool IsImportant(ClassicReplayFrame frame) => frame.Actions.Any();

        protected Vector2? Position
        {
            get
            {
                if (!HasFrames)
                    return null;

                return Interpolation.ValueAt(CurrentTime, CurrentFrame.Position, NextFrame.Position, CurrentFrame.Time, NextFrame.Time);
            }
        }

        public override List<InputState> GetPendingStates()
        {
            return new List<InputState>
            {
                new ReplayState<ClassicAction>
                {
                    Mouse = new ReplayMouseState(GamefieldToScreenSpace(Position ?? Vector2.Zero)),
                    PressedActions = CurrentFrame.Actions
                }
            };
        }
    }
}
