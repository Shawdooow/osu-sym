#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using osu.Framework.Bindables;
using osu.Framework.Logging;
using osu.Game.Rulesets.Vitaru.ChapterSets.Dodge;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu;
using osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets
{
    public static class ChapterStore
    {
        public static List<LoadedChapterSet> LoadedChapterSets { get; private set; } = new List<LoadedChapterSet>();

        public static void ReloadChapterSets()
        {
            List<ChapterSet> loadedSets = new List<ChapterSet>
            {
                new VitaruChapterSet(),
                new TouhosuChapterSet(),
                new DodgeChapterSet(),
            };

            Dictionary<Assembly, Type> loadedAssemblies = new Dictionary<Assembly, Type>();

            foreach (string file in Directory.GetFiles(Environment.CurrentDirectory, $"Vitaru.ChapterSets.*.dll"))
            {
                string filename = Path.GetFileNameWithoutExtension(file);

                if (loadedAssemblies.Values.Any(t => t.Namespace == filename)) return;

                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    loadedAssemblies[assembly] = assembly.GetTypes().First(t => t.IsPublic && t.IsSubclassOf(typeof(ChapterSet)));
                }
                catch (Exception)
                {
                    Logger.Log($"Error loading a chapterset from a chapter file! [filename = {filename}]", LoggingTarget.Runtime, LogLevel.Error);
                }
            }

            List<ChapterSet> instances = loadedAssemblies.Values.Select(g => (ChapterSet)Activator.CreateInstance(g)).ToList();

            foreach (ChapterSet s in instances)
                loadedSets.Add(s);

            foreach (ChapterSet s in loadedSets)
                LoadedChapterSets.Add(new LoadedChapterSet(s));
        }
        
        /// <summary>
        /// Trys to find a loaded gamemode with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ChapterSet GetChapterSet(string name)
        {
            foreach (LoadedChapterSet set in LoadedChapterSets)
                if (set.ChapterSet.Name == name)
                    return set.ChapterSet;

            return null;
        }

        /// <summary>
        /// Trys to find a loaded chapter with the given title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static Chapter GetChapter(string title)
        {
            foreach (LoadedChapterSet set in LoadedChapterSets)
                foreach (Chapter chapter in set.Chapters)
                    if (chapter.Title == title)
                        return chapter;
            return null;
        }

        /// <summary>
        /// Trys to find a loaded player with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static VitaruPlayer GetPlayer(string name)
        {
            foreach (LoadedChapterSet set in LoadedChapterSets)
                foreach (VitaruPlayer p in set.Players)
                    if (p.Name == name)
                        return p;
            return null;
        }

        /// <summary>
        /// Asks chapters if they have a special DrawablePlayer for the given player
        /// </summary>
        /// <param name="playfield"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static DrawableVitaruPlayer GetDrawablePlayer(VitaruPlayfield playfield, VitaruPlayer player)
        {
            foreach (LoadedChapterSet set in LoadedChapterSets)
                foreach (Chapter chapter in set.Chapters)
                    if (chapter.GetDrawablePlayer(playfield, player) != null)
                        return chapter.GetDrawablePlayer(playfield, player);
            return null;
        }

        public class LoadedChapterSet
        {
            public readonly ChapterSet ChapterSet;

            public readonly Bindable<string> SelectedCharacter = new Bindable<string>();

            public readonly List<Chapter> Chapters = new List<Chapter>();

            public readonly List<VitaruPlayer> Players = new List<VitaruPlayer>();

            public LoadedChapterSet(ChapterSet set)
            {
                ChapterSet = set;

                foreach (Chapter v in set.GetChapters())
                    Chapters.Add(v);

                foreach (Chapter c in Chapters)
                    foreach (VitaruPlayer v in c.GetPlayers())
                    {
                        bool add = true;
                        foreach (VitaruPlayer p in Players)
                            if (p.Name == v.Name)
                            {
                                add = false;
                                break;
                            }
                        if (add)
                            Players.Add(v);
                    }

                SelectedCharacter.Default = Players.First().Name;
                SelectedCharacter.SetDefault();
            }
        }
    }
}
