using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using osu.Framework.Configuration;
using osu.Framework.Logging;
using osu.Game.Rulesets.Vitaru.Mods.ChapterSets.Chapters;
using osu.Game.Rulesets.Vitaru.Mods.Gamemodes;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

namespace osu.Game.Rulesets.Vitaru.Mods.ChapterSets
{
    public static class ChapterStore
    {
        public static List<Chapterset> LoadedChapterSets { get; private set; } = new List<Chapterset>();

        public static List<LoadedGamemode> LoadedGamemodes { get; private set; } = new List<LoadedGamemode>();

        public static void ReloadChapterSets()
        {
            LoadedChapterSets = new List<Chapterset>
            {
                new VitaruChapterSet(),
            };

            Dictionary<Assembly, Type> loadedAssemblies = new Dictionary<Assembly, Type>();

            foreach (string file in Directory.GetFiles(Environment.CurrentDirectory, $"Vitaru.ChapterSets.*.dll"))
            {
                string filename = Path.GetFileNameWithoutExtension(file);

                if (loadedAssemblies.Values.Any(t => t.Namespace == filename)) return;

                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);
                    loadedAssemblies[assembly] = assembly.GetTypes().First(t => t.IsPublic && t.IsSubclassOf(typeof(Chapterset)));
                }
                catch (Exception)
                {
                    Logger.Log("Error loading a chapterset from a chapter file! [filename = " + filename + "]", LoggingTarget.Runtime, LogLevel.Error);
                }
            }

            List<Chapterset> instances = loadedAssemblies.Values.Select(g => (Chapterset)Activator.CreateInstance(g)).ToList();

            foreach (Chapterset s in instances)
                LoadedChapterSets.Add(s);

            ReloadGamemodes();
        }

        public static void ReloadGamemodes()
        {
            LoadedGamemodes = new List<LoadedGamemode>();

            foreach (Chapterset chapter in LoadedChapterSets)
            foreach (VitaruGamemode g in chapter.GetGamemodes())
            {
                bool add = true;
                foreach (LoadedGamemode m in LoadedGamemodes)
                    if (m.Gamemode.Name == g.Name)
                    {
                        add = false;

                        foreach (VitaruChapter ch in m.Chapters)
                            m.Chapters.Add(ch);
                        foreach (VitaruPlayer pl in m.Players)
                            m.Players.Add(pl);

                        break;
                    }
                if (add)
                    LoadedGamemodes.Add(new LoadedGamemode(g));
            }
        }
        
        /// <summary>
        /// Trys to find a loaded gamemode with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static VitaruGamemode GetGamemode(string name)
        {
            foreach (LoadedGamemode gamemode in LoadedGamemodes)
                if (gamemode.Gamemode.Name == name)
                    return gamemode.Gamemode;

            return new VitaruGamemode();
        }

        /// <summary>
        /// Trys to find a loaded chapter with the given title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static VitaruChapter GetChapter(string title)
        {
            foreach (LoadedGamemode gamemode in LoadedGamemodes)
                foreach (VitaruChapter chapter in gamemode.Chapters)
                    if (chapter.Title == title)
                        return chapter;
            return new VitaruChapter();
        }

        /// <summary>
        /// Trys to find a loaded player with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static VitaruPlayer GetPlayer(string name)
        {
            foreach (LoadedGamemode gamemode in LoadedGamemodes)
                foreach (VitaruPlayer p in gamemode.Players)
                    if (p.Name == name)
                        return p;
            return new VitaruPlayer();
        }

        /// <summary>
        /// Asks chapters if they have a special DrawablePlayer for the given player
        /// </summary>
        /// <param name="playfield"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static DrawableVitaruPlayer GetDrawablePlayer(VitaruPlayfield playfield, VitaruPlayer player)
        {
            foreach (LoadedGamemode gamemode in LoadedGamemodes)
                foreach (VitaruChapter chapter in gamemode.Chapters)
                    if (chapter.GetDrawablePlayer(playfield, player) != null)
                        return chapter.GetDrawablePlayer(playfield, player);
            return new DrawableVitaruPlayer(playfield, player);
        }

        public class LoadedGamemode
        {
            public readonly VitaruGamemode Gamemode;

            public readonly Bindable<string> SelectedCharacter = new Bindable<string>();

            public readonly List<VitaruChapter> Chapters = new List<VitaruChapter>();

            public readonly List<VitaruPlayer> Players = new List<VitaruPlayer>();

            public LoadedGamemode(VitaruGamemode gamemode)
            {
                Gamemode = gamemode;

                foreach (VitaruChapter v in gamemode.GetChapters())
                    Chapters.Add(v);

                foreach (VitaruChapter c in Chapters)
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
