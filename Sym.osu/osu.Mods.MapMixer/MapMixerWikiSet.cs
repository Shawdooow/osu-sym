using osu.Core.Wiki;
using osu.Core.Wiki.Sections;

namespace osu.Mods.MapMixer
{
    public class MapMixerWikiSet : WikiSet
    {
        public override string Name => "Map Mixer";

        public override string IndexTooltip => "\"how to mix maps\"";

        public override WikiSection[] GetSections() => null;
    }
}
