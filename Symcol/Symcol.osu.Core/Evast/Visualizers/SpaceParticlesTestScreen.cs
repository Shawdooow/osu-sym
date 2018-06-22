// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

namespace Symcol.osu.Core.Evast.Visualizers
{
    public class SpaceParticlesTestScreen : BeatmapScreen
    {
        public SpaceParticlesTestScreen()
        {
            Child = new SpaceParticlesContainer();
        }
    }
}
