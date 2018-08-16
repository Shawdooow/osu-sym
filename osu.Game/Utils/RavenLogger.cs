// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;

namespace osu.Game.Utils
{
    /// <summary>
    /// Report errors to sentry.
    /// </summary>
    public class RavenLogger : IDisposable
    {
        public RavenLogger(OsuGame game)
        {
        }

        public void Dispose()
        {
        }
    }
}
