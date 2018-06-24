using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Symcol.osu.Core.Wiki.Sections.SectionPieces;
using Symcol.osu.Core.Wiki.Sections.Subsection;

namespace Symcol.osu.Core.Wiki.Sections
{
    public abstract class WikiChangelogSection : WikiSection
    {
        protected abstract string RulesetVersion { get; }

        protected abstract string RulesetStorage { get; }

        protected abstract string FileExtention { get; }

        /// <summary>
        /// Whats changed in this version
        /// </summary>
        protected abstract string VersionChangelog { get; }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            Storage changelogStorage = storage.GetStorageForDirectory(RulesetStorage);

            Content.Add(new WikiParagraph("This changelog is cumulative, meaning only versions you install and run will be added here. " +
                "They are saved as text files in your osu! storage. " +
                "Below are all the versions you have saved + this version (" + RulesetVersion + ")."));

            try
            {
                if (changelogStorage == null)
                    Logger.Log("changelogStorage == null", LoggingTarget.Database, LogLevel.Error);
                else
                {
                    using (Stream stream = changelogStorage.GetStream(RulesetVersion + FileExtention, FileAccess.Write, FileMode.Create))
                    using (StreamWriter w = new StreamWriter(stream))
                        w.WriteLine(VersionChangelog);
                }
            }
            catch { Logger.Log("Could not create / update current version file in changelogStorage!", LoggingTarget.Database, LogLevel.Error); }

            try
            {
                for (int i = 10; i >= 0; i--)
                    for (int j = 20; j >= 0; j--)
                        for (int k = 20; k >= 0; k--)
                            if (changelogStorage.Exists(i + "." + j + "." + k + FileExtention))
                            {
                                Content.Add(new WikiSubSectionHeader(i + "." + j + "." + k));

                                using (Stream stream = changelogStorage.GetStream(i + "." + j + "." + k + FileExtention, FileAccess.Read, FileMode.Open))
                                using (StreamReader r = new StreamReader(stream))
                                    Content.Add(new WikiParagraph(r.ReadToEnd()));
                            }
            }
            catch { Logger.Log("Error reading saved versions (if you have any, if not thats the problem likely)", LoggingTarget.Database, LogLevel.Error); }
        }
    }
}
