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

        private const string version_changelog = "-Updated to lazer version 2018.514.0\n" +
            "-Updated to lazer version 2018.511.0\n" +
            "-Co-op online multiplayer\n" +
            "-Implement Tomaji's spell\n" +
            "-Adjust Tomaji's stats based on his now implemented spell\n" +
            "-Nerf Sakuya's energy drain rate (6 => 8)\n" +
            "-Split old \"Gameplay\" wiki section into three new sections: General, Gamemodes and Characters\n" +
            "-Fix Seal not loading properly for non touhosu characters\n" +
            "-Make Healing and energy gain per bullet and the amount distance based\n" +
            "-Change most of the patterns to have variable bullet size and damage\n\n" +
            "-SkinV2 graphics option (give it a try, we have AnimatedSprites now!)";

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
