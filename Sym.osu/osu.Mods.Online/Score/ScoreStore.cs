using System.IO;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Mods.Online.Score.Packets;

namespace osu.Mods.Online.Score
{
    public class ScoreStore
    {
        private const string ext = ".sym";

        protected Storage BeatmapStorage;
        protected Storage UserStorage;

        public ScoreStore(Storage storage)
        {
            BeatmapStorage = storage.GetStorageForDirectory("server\\scores\\beatmaps");
            UserStorage = storage.GetStorageForDirectory("server\\scores\\users");
        }

        public void SetScore(ScoreSubmissionPacket scoreSubmission)
        {
            string username = $"{scoreSubmission.User.ID}\\{scoreSubmission.Score.RulesetShortname}\\{scoreSubmission.Score.OnlineBeatmapSetID}\\{scoreSubmission.Score.OnlineBeatmapID}{ext}";
            Stream stream = UserStorage.GetStream(username, FileAccess.Read, FileMode.Open);

            if (stream != null)
            {
                StreamReader reader = new StreamReader(stream);
                string data = reader.ReadToEnd();
                OnlineScore score = deSerialize(data);

                if (score != null && score.Score <= scoreSubmission.Score.Score)
                    save(username, UserStorage);
                else if (score == null)
                    save(username, UserStorage);
            }
            else
                save(username, UserStorage);

            string beatmap = $"{scoreSubmission.Score.OnlineBeatmapSetID}\\{scoreSubmission.Score.OnlineBeatmapID}\\{scoreSubmission.Score.RulesetShortname}\\{scoreSubmission.User.ID}{ext}";
            stream = BeatmapStorage.GetStream(beatmap, FileAccess.Read, FileMode.Open);

            if (stream != null)
            {
                StreamReader reader = new StreamReader(stream);
                string data = reader.ReadToEnd();
                OnlineScore score = deSerialize(data);

                if (score != null && score.Score <= scoreSubmission.Score.Score)
                    save(beatmap, BeatmapStorage);
                else if (score == null)
                    save(beatmap, BeatmapStorage);
            }
            else
                save(beatmap, BeatmapStorage);

            void save(string path, Storage store)
            {
                stream?.Close();
                StreamWriter writer = new StreamWriter(store.GetStream(path, FileAccess.Write, FileMode.Create));
                writer.Write(serialize(scoreSubmission.Score));
                writer.Close();
            }
        }

        private string serialize(OnlineScore score) => $"{score.Score}:{score.Combo}:{score.Accuracy}:{score.PP}";

        private OnlineScore deSerialize(string data)
        {
            OnlineScore score = new OnlineScore();
            string[] dataStrings = data.Split(':');

            try
            {
                score.Score = double.Parse(dataStrings[0]);
                score.Combo = double.Parse(dataStrings[1]);
                score.Accuracy = double.Parse(dataStrings[2]);
                score.PP = double.Parse(dataStrings[3]);
                return score;
            }
            catch
            {
                Logger.Log($"Corrupt File: {data}", LoggingTarget.Database, LogLevel.Error);
                return null;
            }
        }
    }
}
