using System.Collections.Generic;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Core.Wiki.IncludedWikis.Lazer;
using Symcol.osu.Core.Wiki.IncludedWikis.Symcol;

namespace Symcol.osu.Core.Wiki
{
    public static class WikiSetStore
    {
        public static List<WikiSet> LoadedWikiSets = new List<WikiSet>();

        public static void ReloadWikiSets()
        {
            //We want to add a default one for "Home"
            LoadedWikiSets = new List<WikiSet>
            {
                new LazerWikiSet(),
                new SymcolWikiSet()
            };


            foreach (SymcolModSet set in SymcolModStore.LoadedModSets)
                if (set.GetWikiSet() != null)
                    LoadedWikiSets.Add(set.GetWikiSet());
        }
    }
}
