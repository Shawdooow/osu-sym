using System;
using System.IO;
using System.Linq;
using osu.Framework.IO.Stores;
using osu.Game.Database;

namespace osu.Game.Rulesets.Vitaru.IO
{
    public class LegacyPatternParser<T> : IResourceStore<byte[]>
            where T : INamedFileInfo
    {
        private readonly IHasFiles<T> source;
        private readonly IResourceStore<byte[]> underlyingStore;

        public LegacyPatternParser(IHasFiles<T> source, IResourceStore<byte[]> underlyingStore)
        {
            this.source = source;
            this.underlyingStore = underlyingStore;
        }

        public string GetPatternFileContents()
        {
            return "";
        }

        public Stream GetStream(string name)
        {
            string path = getPathForFile(name);
            return path == null ? null : underlyingStore.GetStream(path);
        }

        byte[] IResourceStore<byte[]>.Get(string name)
        {
            string path = getPathForFile(name);
            return path == null ? null : underlyingStore.Get(path);
        }

        private string getPathForFile(string filename)
        {
            bool hasExtension = filename.Contains('.');

            string lastPiece = filename.Split('/').Last();

            var file = source.Files.FirstOrDefault(f =>
                string.Equals(hasExtension ? f.Filename : Path.GetFileNameWithoutExtension(f.Filename), lastPiece, StringComparison.InvariantCultureIgnoreCase));
            return file?.FileInfo.StoragePath;
        }
    }
}
