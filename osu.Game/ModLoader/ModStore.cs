using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using osu.Framework.Logging;

namespace osu.Game.ModLoader
{
    public static class ModStore
    {
        public static List<ModSet> ModSets = new List<ModSet>();

        private static Dictionary<Assembly, Type> loadedAssemblies = new Dictionary<Assembly, Type>();

        public static void LoadModSets()
        {
            ModSets = new List<ModSet>();

            loadedAssemblies = new Dictionary<Assembly, Type>();

            foreach (string file in Directory.GetFiles(Environment.CurrentDirectory, "Symcol.osu.Core.dll"))
            {
                var filename = Path.GetFileNameWithoutExtension(file);

                if (loadedAssemblies.Values.Any(t => t.Namespace == filename)) return;

                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    loadedAssemblies[assembly] = assembly.GetTypes().First(t => t.IsPublic && t.IsSubclassOf(typeof(ModSet)));
                }
                catch (Exception)
                {
                    Logger.Log("Error loading a modset assembly!", LoggingTarget.Runtime, LogLevel.Error);
                }
            }

            var instances = loadedAssemblies.Values.Select(g => (ModSet)Activator.CreateInstance(g)).ToList();

            foreach (ModSet s in instances)
                ModSets.Add(s);
        }
    }
}
