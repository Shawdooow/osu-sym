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

        private const string file_extention = ".vitaru";

        private const string version_changelog = "Features:\n\n" +
            "-Debug mode (Must be a verified developer to use, contains cheats and whatnot to help debug stuff)\n" +
            "-Bug player to checkout settings once\n\n" +
            "Tweaks / Changes:\n\n" +
            "-Some serious optimizations\n" +
            "-Use standard Get functions for characters (community made characters soon!)\n" +
            "-Added Sakuya's Background\n" +
            "-Nerf Ryukoy's Health (80 => 60)\n" +
            "-Nerf Ryukoy's Max Energy (24 => 20)\n" +
            "-Nerf Tomaji's Max Energy (16 => 12)\n" +
            "-Tweak Tomaji's spell values:\n" +
            "   Time to charge a full blink reduced (2000ms => 600ms)\n" +
            "   Distance covered at full charge reduced (400 => 200)\n" +
            "-Nerf Sakuya's Health (100 => 80)\n" +
            "-Nerf Sakuya's Max Energy (36 => 32)";//\n\n" +
            //"Fixes:\n\n" +
            //"";

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            Storage changelogStorage = storage.GetStorageForDirectory("vitaru");

            Content.Add(new WikiParagraph("This changelog is cumulative, meaning only versions you install and run will be added here. " +
                "They are saved as text files in your osu! storage. " +
                "Below are all the versions you have saved + this version (" + VitaruRuleset.RulesetVersion + ")."));

            try
            {
                if (changelogStorage == null)
                    Logger.Log("changelogStorage == null", LoggingTarget.Database, LogLevel.Error);
                else
                {
                    using (Stream stream = changelogStorage.GetStream(VitaruRuleset.RulesetVersion + file_extention, FileAccess.Write, FileMode.Create))
                    using (StreamWriter w = new StreamWriter(stream))
                        w.WriteLine(version_changelog);
                }
            }
            catch { Logger.Log("Could not create / update current version file in changelogStorage!", LoggingTarget.Database, LogLevel.Error); }

            for (int i = 10; i >= 0; i--)
                for (int j = 20; j >= 0; j--)
                    for (int k = 20; k >= 0; k--)
                        if (changelogStorage.Exists(i + "." + j + "." + k + file_extention))
                        {
                            Content.Add(new WikiSubSectionHeader(i + "." + j + "." + k));

                            using (Stream stream = changelogStorage.GetStream(i + "." + j + "." + k + file_extention, FileAccess.Read, FileMode.Open))
                            using (StreamReader r = new StreamReader(stream))
                                Content.Add(new WikiParagraph(r.ReadToEnd()));
                        }
        }
    }
}
