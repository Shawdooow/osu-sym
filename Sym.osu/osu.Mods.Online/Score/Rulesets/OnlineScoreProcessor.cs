#region usings

using System;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.UI;
using osu.Mods.Online.Multi;
using osu.Mods.Online.Score.Packets;
using osu.Mods.Rulesets.Core.Rulesets;
using Sym.Networking.NetworkingHandlers;

#endregion

namespace osu.Mods.Online.Score.Rulesets
{
    public abstract class OnlineScoreProcessor<TObject> : SymcolScoreProcessor<TObject>, IDisposable
        where TObject : HitObject
    {
        protected virtual bool Ranked => false;

        protected Action<OnlineScore> OnCompletion;

        protected DrawableRuleset<TObject> RulesetContainer { get; private set; }

        protected virtual OnlineScore GetOnlineScore() => new OnlineScore
        {
            Map = new Map
            {
                OnlineBeatmapSetID = RulesetContainer.Beatmap.BeatmapInfo.BeatmapSet.OnlineBeatmapSetID ?? 0,
                OnlineBeatmapID = RulesetContainer.Beatmap.BeatmapInfo.OnlineBeatmapID ?? 0,
                BeatmapTitle = RulesetContainer.Beatmap.Metadata.Title,
                BeatmapArtist = RulesetContainer.Beatmap.Metadata.Artist,
                BeatmapMapper = RulesetContainer.Beatmap.Metadata.Author.Username,
                BeatmapDifficulty = RulesetContainer.Beatmap.BeatmapInfo.Version,
                RulesetShortname = RulesetContainer.Ruleset.ShortName,
            },
            Score = TotalScore.Value,
            Combo = Combo.Value,
            Accuracy = Accuracy.Value,
            PP = PP,
        };

        protected virtual ScoreSubmissionPacket GetScoreSubmissionPacket(OnlineScore score) => new ScoreSubmissionPacket
        {
            Score = score,
        };

        protected override bool HasCompleted
        {
            get
            {
                if (JudgedHits != MaxHits) return false;

                if (Ranked)
                    OnCompletion?.Invoke(GetOnlineScore());
                return true;
            }
        }

        protected OnlineScoreProcessor()
        {
        }

        protected OnlineScoreProcessor(DrawableRuleset<TObject> rulesetContainer) : base(rulesetContainer)
        {
            RulesetContainer = rulesetContainer;
            OnCompletion += score =>
            {
                //Whoever wrote this is a fucking moron!
                if (OnlineModset.OsuNetworkingHandler != null && OnlineModset.OsuNetworkingHandler.Host.Statues >= ConnectionStatues.Connected)
                    OnlineModset.OsuNetworkingHandler.SendToServer(GetScoreSubmissionPacket(score));
            };
        }

        public virtual void Dispose()
        {
            RulesetContainer = null;
            OnCompletion = null;
        }
    }
}
