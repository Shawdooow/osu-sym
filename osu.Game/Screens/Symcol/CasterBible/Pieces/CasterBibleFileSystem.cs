using osu.Framework.Logging;
using osu.Framework.Platform;
using System;
using System.IO;

namespace osu.Game.Screens.Symcol.CasterBible.Pieces
{
    public class CasterBibleFileSystem
    {
        public readonly Storage Storage;

        private readonly string fileName;

        public CasterBibleFileSystem(Storage storage, string fileName)
        {
            Storage = storage;
            this.fileName = fileName;

            if (!Storage.Exists(fileName))
            {
                try
                {
                    if (fileName == "teams.mango")
                    {
                        if (Storage == null)
                            Logger.Log("CasterBibleFileSystem - storage == null", LoggingTarget.Database, LogLevel.Error);
                        else
                        {
                            string blank = "";

                            foreach (Country country in System.Enum.GetValues(typeof(Country)))
                                blank = blank + country.ToString() + "/Players=/Stats=/Notes=/Seed=/." + Environment.NewLine;

                            using (Stream stream = Storage.GetStream(fileName, FileAccess.Write, FileMode.Create))
                            using (StreamWriter w = new StreamWriter(stream))
                                w.WriteLine(blank);
                        }
                    }
                    else if (fileName == "maps.mango")
                    {
                        if (Storage == null)
                            Logger.Log("CasterBibleFileSystem - storage == null", LoggingTarget.Database, LogLevel.Error);
                        else
                        {
                            string blank = "";

                            for (int i = 1; i <= 14; i++)
                                blank = blank + "BeatmapSetID=" + i.ToString() + "|BeatmapID=" + i.ToString() + "|Mod=|Information=|." + Environment.NewLine;

                            using (Stream stream = Storage.GetStream(fileName, FileAccess.Write, FileMode.Create))
                            using (StreamWriter w = new StreamWriter(stream))
                                w.WriteLine(blank);
                        }
                    }
                }
                catch
                {
                    Logger.Log("CasterBibleFileSystem - Failed to save to file\"" + fileName + "\"", LoggingTarget.Database, LogLevel.Error);
                }
            }
        }

        public string GetFileContent()
        {
            try
            {
                if (Storage == null)
                    Logger.Log("CasterBibleFileSystem - storage == null", LoggingTarget.Debug, LogLevel.Error);
                else
                {
                    using (Stream stream = Storage.GetStream(fileName, FileAccess.Read, FileMode.Open))
                    using (StreamReader r = new StreamReader(stream))
                        return r.ReadToEnd();
                }
            }
            catch
            {
                Logger.Log("CasterBibleFileSystem - Failed to load from file\"" + fileName + "\"", LoggingTarget.Database, LogLevel.Error);
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

    public enum Mods
    {
        NoMod,
        Hidden,
        HardRock,
        DoubleTime,
        FreeMod,
        TieBreaker,
    }
}
