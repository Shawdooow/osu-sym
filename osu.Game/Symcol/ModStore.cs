using System;
using osu.Framework.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace osu.Game.Symcol
{
    public static class ModStore
    {
        public static List<ModSet> ModSets = new List<ModSet>();

        private static Dictionary<Assembly, Type> loadedAssemblies = new Dictionary<Assembly, Type>();

        public static ModSet GetModSet()
        {
            loadedAssemblies = new Dictionary<Assembly, Type>();

            ModSet set = null;

            foreach (string file in Directory.GetFiles(Environment.CurrentDirectory, "Symcol.osu.Core.dll"))
            {
                var filename = Path.GetFileNameWithoutExtension(file);

                if (loadedAssemblies.Values.Any(t => t.Namespace == filename))
                    return null;

                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    loadedAssemblies[assembly] = assembly.GetTypes().First(t => t.IsPublic && t.IsSubclassOf(typeof(ModSet)));
                }
                catch (Exception)
                {
                    Logger.Log("Error loading a modset!", LoggingTarget.Runtime, LogLevel.Error);
                }
            }

            var instances = loadedAssemblies.Values.Select(g => (ModSet)Activator.CreateInstance(g)).ToList();

            //add any other mods
            foreach (ModSet s in instances)
            {
                Logger.Log("Successfully loaded un-official gamemode");
                set = s;
            }

            return set;
        }
    }
}
