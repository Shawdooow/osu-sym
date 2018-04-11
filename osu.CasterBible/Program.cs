using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Game.Graphics;
using osu.Game.Screens.Symcol.CasterBible;

namespace osu.CasterBible
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using (GameHost host = Host.GetSuitableHost("CasterBible"))
            {
                host.Run(new CasterBibleBase());
            }
        }

        private class CasterBibleBase : osu.Framework.Game
        {
            private DependencyContainer dependencies;

            protected override IReadOnlyDependencyContainer CreateLocalDependencies(IReadOnlyDependencyContainer parent) =>
    dependencies = new DependencyContainer(base.CreateLocalDependencies(parent));

            protected override void LoadComplete()
            {
                dependencies.Cache(new OsuColour());
                base.LoadComplete();
                Add(new TournyCasterBible());
            }
        }
    }
}
