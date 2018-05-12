// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Replays;
using osu.Game.Rulesets.Replays.Legacy;
using osu.Game.Rulesets.Replays.Types;
using OpenTK;

namespace osu.Game.Rulesets.Classic.Replays
{
    public class ClassicReplayFrame : ReplayFrame, IConvertibleReplayFrame
    {
        public Vector2 Position;
        public List<ClassicAction> Actions = new List<ClassicAction>();

        public ClassicReplayFrame()
        {
        }

        public ClassicReplayFrame(double time, Vector2 position, params ClassicAction[] actions)
            : base(time)
        {
            Position = position;
            Actions.AddRange(actions);
        }

        public void ConvertFrom(LegacyReplayFrame legacyFrame, IBeatmap beatmap)
        {
            Position = legacyFrame.Position;
            if (legacyFrame.MouseLeft) Actions.Add(ClassicAction.LeftButton);
            if (legacyFrame.MouseRight) Actions.Add(ClassicAction.RightButton);
        }
    }
}
