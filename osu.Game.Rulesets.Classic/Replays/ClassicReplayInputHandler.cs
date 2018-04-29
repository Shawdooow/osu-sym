// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using osu.Framework.Input;
using osu.Game.Rulesets.Replays;
using OpenTK;

namespace osu.Game.Rulesets.Classic.Replays
{
    public class ClassicReplayInputHandler : FramedReplayInputHandler
    {
        public ClassicReplayInputHandler(Replay replay)
            : base(replay)
        {
        }

        public override List<InputState> GetPendingStates()
        {
            List<ClassicAction> actions = new List<ClassicAction>();

            if (CurrentFrame?.MouseLeft ?? false) actions.Add(ClassicAction.LeftButton);
            if (CurrentFrame?.MouseRight ?? false) actions.Add(ClassicAction.RightButton);

            return new List<InputState>
            {
                new ReplayState<ClassicAction>
                {
                    Mouse = new ReplayMouseState(ToScreenSpace(Position ?? Vector2.Zero)),
                    PressedActions = actions
                }
            };
        }
    }
}
