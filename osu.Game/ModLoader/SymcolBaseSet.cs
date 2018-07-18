using osu.Game.Overlays.Toolbar;
using osu.Game.Screens;
using System;

namespace osu.Game.ModLoader
{
    public abstract class SymcolBaseSet : IDisposable
    {
        public abstract OsuScreen GetMenuScreen();

        public virtual Toolbar GetToolbar() => null;

        public virtual void LoadComplete(OsuGame game) { }

        public virtual void Dispose() { }
    }
}
