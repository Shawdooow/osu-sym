using osu.Framework.Logging;
using osu.Framework.Platform;
using System;
using System.IO;

namespace Symcol.Rulesets.Core.VectorVideos
{
    public class VectorVideosFileSystem
    {
        public const string FileName = "vector.symcol";

        private readonly Storage storage;

        public VectorVideosFileSystem(Storage storage)
        {
            this.storage = storage;

            if (!storage.Exists(FileName))
            {
                try
                {
                    if (storage == null)
                        Logger.Log("VectorVideosFileSystem - storage == null", LoggingTarget.Database, LogLevel.Error);
                    else
                    {
                        string blank = "";

                        foreach (ObjectType type in System.Enum.GetValues(typeof(ObjectType)))
                            blank = blank + type.ToString() + "/" + "Position=/" + "Scale=/" + "Color=/." + Environment.NewLine;

                        using (Stream stream = storage.GetStream(FileName, FileAccess.Write, FileMode.Create))
                        using (StreamWriter w = new StreamWriter(stream))
                            w.WriteLine(blank);
                    }
                }
                catch
                {
                    Logger.Log("VectorVideosFileSystem - Failed to save world to file\"" + FileName + "\"", LoggingTarget.Database, LogLevel.Error);
                }
            }
        }

        public string GetFileContent()
        {
            try
            {
                if (storage == null)
                    Logger.Log("VectorVideosFileSystem - storage == null", LoggingTarget.Database, LogLevel.Error);
                else
                {
                    using (Stream stream = storage.GetStream(FileName, FileAccess.Read, FileMode.Open))
                    using (StreamReader r = new StreamReader(stream))
                        return r.ReadToEnd();
                }
            }
            catch
            {
                Logger.Log("VectorVideosFileSystem - Failed to load world from file\"" + FileName + "\"", LoggingTarget.Database, LogLevel.Error);
            }
            return "null";
        }
    }
}
