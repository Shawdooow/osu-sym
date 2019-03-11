using osu.Core.Wiki;
using osu.Core.Wiki.Sections;
using osu.Mods.Online.Wiki.Sections;

namespace osu.Mods.Online.Wiki
{
    public class OnlineWiki : WikiSet
    {
        public override string Name => "online";

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new Changelog(),
        };
    }
}
