using osu.Framework.Configuration;
using osu.Game.Graphics.Containers;
using Symcol.osu.Core.Wiki.Sections;

namespace Symcol.osu.Core.Wiki.Index
{
    public class WikiIndex : OsuScrollContainer
    {
        public Bindable<WikiSection> Current { get; } = new Bindable<WikiSection>();

        public WikiIndex()
        {

        }
    }
}
