using osu.Framework.Logging;
using osu.Framework.Platform;
using System;
using System.IO;

namespace osu.Game.Screens.Symcol.CasterBible.Pieces
{
    public class CasterBibleFileSystem
    {
        public const string FileName = "teams.mango";

        private readonly Storage storage;

        public CasterBibleFileSystem(Storage storage)
        {
            this.storage = storage;

            if (!storage.Exists(FileName))
            {
                try
                {
                    if (storage == null)
                        Logger.Log("CasterBibleFileSystem - storage == null", LoggingTarget.Database, LogLevel.Error);
                    else
                    {
                        string blank = "";

                        foreach (Country country in System.Enum.GetValues(typeof(Country)))
                            blank = blank + country.ToString() + "/" + "Players=/" + "Stats=/" + "Notes=/" + "Seed=/." + Environment.NewLine;

                        using (Stream stream = storage.GetStream(FileName, FileAccess.Write, FileMode.Create))
                        using (StreamWriter w = new StreamWriter(stream))
                            w.WriteLine(blank);
                    }
                }
                catch
                {
                    Logger.Log("CasterBibleFileSystem - Failed to save world to file\"" + FileName + "\"", LoggingTarget.Database, LogLevel.Error);
                }
            }
        }

        public string GetFileContent()
        {
            try
            {
                if (storage == null)
                    Logger.Log("CasterBibleFileSystem - storage == null", LoggingTarget.Debug, LogLevel.Error);
                else
                {
                    using (Stream stream = storage.GetStream(FileName, FileAccess.Read, FileMode.Open))
                    using (StreamReader r = new StreamReader(stream))
                        return r.ReadToEnd();
                }
            }
            catch
            {
                Logger.Log("CasterBibleFileSystem - Failed to load world from file\"" + FileName + "\"", LoggingTarget.Database, LogLevel.Error);
            }
            return "null";
        }
    }

    public enum Country
    {
        Argentina,
        Australia,
        Brazil,
        Canada,
        Chile,
        Finland,
        France,
        Germany,
        HongKong,
        Indonesia,
        Italy,
        Japan,
        Malaysia,
        Netherlands,
        Poland,
        Portugal,
        RussianFederation,
        Spain,
        SouthKorea,
        Taiwan,
        Ukraine,
        UnitedKingdom,
        UnitedStates,
        Venezuela,
    }
}
