using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Tests.Beatmaps;
using osu.Game.Tests.Visual;
using OpenTK;

namespace osu.Game.Rulesets.Vitaru.Tests
{
    [TestFixture]
    public class TestCaseVitaruPlayfield : OsuTestCase
    {
        private const double default_duration = 1000;

        protected override double TimePerAction => default_duration * 2;

        private VitaruRulesetContainer rulesetContainer;
        private Container playfieldContainer;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            VitaruSettings.VitaruConfigManager = new VitaruConfigManager(host.Storage);

            AddStep("Wave!", () => addPattern());

            var controlPointInfo = new ControlPointInfo();
            controlPointInfo.TimingPoints.Add(new TimingControlPoint());

            WorkingBeatmap beatmap = new TestWorkingBeatmap(new Beatmap
            {
                HitObjects = new List<HitObject>(),
                BeatmapInfo = new BeatmapInfo
                {
                    BaseDifficulty = new BeatmapDifficulty(),
                    Metadata = new BeatmapMetadata
                    {
                        Artist = @"Unknown",
                        Title = @"Sample Beatmap",
                        AuthorString = @"not_peppy",
                    },
                    Ruleset = new VitaruRuleset().RulesetInfo
                },
                ControlPointInfo = controlPointInfo
            });

            var rateAdjustClock = new StopwatchClock(true) { Rate = 1 };

            Add(playfieldContainer = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.X,
                Height = 768,
                Clock = new FramedClock(rateAdjustClock),
                Children = new[] { rulesetContainer = new VitaruRulesetContainer(new VitaruRuleset(), beatmap) }
            });
        }

        private void addPattern(int patternID = 1)
        {
            Pattern p = new Pattern
            {
                StartTime = Time.Current + 600,
                Position = new Vector2(384 / 2, 512 / 2),
                PatternComplexity = 2,
                PatternTeam = 1,
                PatternDiameter = 10,
                PatternSpeed = 0.25,
                PatternID = patternID,
            };

            if (rulesetContainer.Playfield is VitaruPlayfield vitaruPlayfield)
            {
                vitaruPlayfield.Add(new DrawablePattern(p, vitaruPlayfield));
            }
        }
    }
}
