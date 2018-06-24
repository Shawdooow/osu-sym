using osu.Game.Overlays.Toolbar;
using osu.Game.Screens;

namespace osu.Game.ModLoader
{
    public abstract class ModSet
    {
        public abstract OsuScreen GetMenuScreen();

        public virtual Toolbar GetToolbar() => null;

        public virtual void LoadComplete(OsuGame game) { }
    }
}
