using osu.Framework.Allocation;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Symcol.Rulesets.Core.Wiki;
using System.IO;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    //TODO: abstract this for all rulesets to use, its super useful!
    public class ChangelogSection : WikiSection
    {
        public override string Title => "Changelog";

        private const string fileExtention = ".vitaru";

        private const string version_changelog = "-Implemented cumulative changelog\n" +
            "-Fix character hiearchy\n" +
            "-Custom character support prep (characterName.vitaru in your map or something)\n" +
            "-Fix several multiplayer crashes";

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            Storage changelogStorage = storage.GetStorageForDirectory("vitaru");

            Content.Add(new WikiParagraph("This changelog is cumulative, meaning only versions you install and run will be added here. They are saved as text files in your osu! storage"));

            if (!changelogStorage.Exists(VitaruRuleset.RulesetVersion + fileExtention))
            {
                try
                {
                    if (changelogStorage == null)
                        Logger.Log("changelogStorage == null", LoggingTarget.Database, LogLevel.Error);
                    else
                    {
                        using (Stream stream = changelogStorage.GetStream(VitaruRuleset.RulesetVersion + fileExtention, FileAccess.Write, FileMode.Create))
                        using (StreamWriter w = new StreamWriter(stream))
                            w.WriteLine(version_changelog);
                    }
                }
                catch { Logger.Log("Could not create current version file in changelogStorage!", LoggingTarget.Database, LogLevel.Error); }
            }

            for (int i = 10; i >= 0; i--)
                for (int j = 20; j >= 0; j--)
                    for (int k = 20; k >= 0; k--)
                        if (changelogStorage.Exists(i + "." + j + "." + k + fileExtention))
                        {
                            Content.Add(new WikiSubSectionHeader(i + "." + j + "." + k));

                            using (Stream stream = changelogStorage.GetStream(i + "." + j + "." + k + fileExtention, FileAccess.Read, FileMode.Open))
                            using (StreamReader r = new StreamReader(stream))
                                Content.Add(new WikiParagraph(r.ReadToEnd()));
                        }
        }
    }
}
