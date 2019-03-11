using System;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.UI;
using osu.Mods.Online.Score.Packets;
using osu.Mods.Rulesets.Core.Rulesets;
using Symcol.Networking.NetworkingHandlers;

namespace osu.Mods.Online.Score.Rulesets
{
    public abstract class OnlineScoreProcessor<TObject> : SymcolScoreProcessor<TObject>
        where TObject : HitObject
    {
        protected virtual bool Ranked => false;

        protected event Action<OnlineScore> OnCompletion;

        protected readonly RulesetContainer<TObject> RulesetContainer;

        protected override bool HasCompleted
        {
            get
            {
                if (JudgedHits != MaxHits) return false;

                if (Ranked)
                    OnCompletion?.Invoke(new OnlineScore
                    {
                        OnlineBeatmapSetID = RulesetContainer.Beatmap.BeatmapInfo.BeatmapSet.OnlineBeatmapSetID ?? 0,
                        OnlineBeatmapID = RulesetContainer.Beatmap.BeatmapInfo.OnlineBeatmapID ?? 0,
                        BeatmapTitle = RulesetContainer.Beatmap.Metadata.Title,
                        BeatmapArtist = RulesetContainer.Beatmap.Metadata.Artist,
                        BeatmapMapper = RulesetContainer.Beatmap.Metadata.Author.Username,
                        BeatmapDifficulty = RulesetContainer.Beatmap.BeatmapInfo.Version,
                        RulesetShortname = RulesetContainer.Ruleset.ShortName,
                        Score = TotalScore.Value,
                        Combo = Combo.Value,
                        Accuracy = Accuracy.Value,
                        PP = PP,
                    });
                return true;

            }
        }

        protected OnlineScoreProcessor()
        {
        }

        protected OnlineScoreProcessor(RulesetContainer<TObject> rulesetContainer) : base(rulesetContainer)
        {
            RulesetContainer = rulesetContainer;
            OnCompletion += score =>
            {
                if (OnlineModset.OsuNetworkingHandler != null && OnlineModset.OsuNetworkingHandler.ConnectionStatues >= ConnectionStatues.Connected)
                    OnlineModset.OsuNetworkingHandler.SendToServer(new ScoreSubmissionPacket
                    {
                        Score = score
                    });
            };
        }
    }
}
