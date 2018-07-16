using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using osu.Framework.Logging;

namespace osu.Game.Rulesets.Vitaru.VitaruMods
{
    public static class CharacterModStore
    {
        public static List<CharacterModSet> LoadedCharacterSets = new List<CharacterModSet>();

        private static Dictionary<Assembly, Type> loadedAssemblies = new Dictionary<Assembly, Type>();

        public static void ReloadModSets()
        {
            LoadedCharacterSets = new List<CharacterModSet>();

            loadedAssemblies = new Dictionary<Assembly, Type>();

            foreach (string file in Directory.GetFiles(Environment.CurrentDirectory, $"osu.Game.Rulesets.Vitaru.Mods.*.dll"))
            {
                var filename = Path.GetFileNameWithoutExtension(file);

                if (loadedAssemblies.Values.Any(t => t.Namespace == filename)) return;

                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    loadedAssemblies[assembly] = assembly.GetTypes().First(t => t.IsPublic && t.IsSubclassOf(typeof(CharacterModSet)));
                }
                catch (Exception)
                {
                    Logger.Log("Error loading a modset from a mod file! [filename = " + filename + "]", LoggingTarget.Runtime, LogLevel.Error);
                }
            }

            var instances = loadedAssemblies.Values.Select(g => (CharacterModSet)Activator.CreateInstance(g)).ToList();

            foreach (CharacterModSet s in instances)
                LoadedCharacterSets.Add(s);
        }
    }
}
