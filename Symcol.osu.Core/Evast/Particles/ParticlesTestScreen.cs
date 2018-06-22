// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

namespace Symcol.osu.Core.Evast.Particles
{
    public class ParticlesTestScreen : BeatmapScreen
    {
        public ParticlesTestScreen()
        {
            Child = new ParticlesContainer(70, 70, 5);
        }
    }
}
