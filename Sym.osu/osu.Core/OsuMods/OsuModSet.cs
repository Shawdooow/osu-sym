#region usings

using osu.Core.Containers.Shawdooow;
using osu.Core.Settings;
using osu.Core.Wiki;
using osu.Framework.Platform;
using osu.Game;
using osu.Game.Screens;

#endregion

namespace osu.Core.OsuMods
{
    public abstract class OsuModSet
    {
        public virtual SymcolButton GetMenuButton() => null;

        public virtual OsuScreen GetScreen() => null;

        public virtual ModSubSection GetSettings() => null;

        public virtual WikiSet GetWikiSet() => null;

        /// <summary>
        /// Initialize the mod
        /// </summary>
        public virtual void Init(OsuGame game, GameHost host) { }

        /// <summary>
        /// Always called After Init and on the Update thread
        /// </summary>
        /// <param name="game"></param>
        /// <param name="host"></param>
        public virtual void LoadComplete(OsuGame game, GameHost host) { }
    }
}
