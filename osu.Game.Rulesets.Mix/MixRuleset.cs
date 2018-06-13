using System.Collections.Generic;
using osu.Game.Rulesets.UI;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Mix.Mods;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Input.Bindings;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Mix.Settings;
using osu.Game.Rulesets.Mix.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Mix.UI;

namespace osu.Game.Rulesets.Mix
{
    public class MixRuleset : Ruleset
    {
        public const string RulesetVersion = "0.1.0";

        public override RulesetContainer CreateRulesetContainerWith(WorkingBeatmap beatmap) => new MixRulesetContainer(this, beatmap);
        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new MixBeatmapConverter(beatmap);
        public override IBeatmapProcessor CreateBeatmapProcessor(IBeatmap beatmap) => new MixBeatmapProcessor(beatmap);

        public override Drawable CreateIcon() => new Sprite { Texture = MixTextures.Get("icon") };

        public static ResourceStore<byte[]> MixResources;
        public static TextureStore MixTextures;

        public MixRuleset(RulesetInfo rulesetInfo)
            : base(rulesetInfo)
        {
            MixResources = new ResourceStore<byte[]>();
            MixResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Game.Rulesets.Mix.dll"), ("Assets")));
            MixResources.AddStore(new DllResourceStore("osu.Game.Rulesets.Mix.dll"));
            MixTextures = new TextureStore(new RawTextureLoaderStore(new NamespacedResourceStore<byte[]>(MixResources, @"Textures")));
            MixTextures.AddStore(new RawTextureLoaderStore(new OnlineStore()));
        }

        public override int? LegacyID => 7;

        public override string Description => "mix!";

        public override string ShortName => "mix";

        public override SettingsSubsection CreateSettings() => new MixSettings();

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.Number4, MixAction.NormalNormalLeft),
            new KeyBinding(InputKey.Number3, MixAction.NormalWhistleLeft),
            new KeyBinding(InputKey.Number2, MixAction.NormalFinishLeft),
            new KeyBinding(InputKey.Number1, MixAction.NormalClapLeft),

            new KeyBinding(InputKey.R, MixAction.DrumNormalLeft),
            new KeyBinding(InputKey.E, MixAction.DrumWhistleLeft),
            new KeyBinding(InputKey.W, MixAction.DrumFinishLeft),
            new KeyBinding(InputKey.Q, MixAction.DrumClapLeft),

            new KeyBinding(InputKey.F, MixAction.SoftNormalLeft),
            new KeyBinding(InputKey.D, MixAction.SoftWhistleLeft),
            new KeyBinding(InputKey.S, MixAction.SoftFinishLeft),
            new KeyBinding(InputKey.A, MixAction.SoftClapLeft),

            new KeyBinding(InputKey.Number7, MixAction.NormalNormalRight),
            new KeyBinding(InputKey.Number8, MixAction.NormalWhistleRight),
            new KeyBinding(InputKey.Number9, MixAction.NormalFinishRight),
            new KeyBinding(InputKey.Number0, MixAction.NormalClapRight),

            new KeyBinding(InputKey.U, MixAction.DrumNormalRight),
            new KeyBinding(InputKey.I, MixAction.DrumWhistleRight),
            new KeyBinding(InputKey.O, MixAction.DrumFinishRight),
            new KeyBinding(InputKey.P, MixAction.DrumClapRight),

            new KeyBinding(InputKey.J, MixAction.SoftNormalRight),
            new KeyBinding(InputKey.K, MixAction.SoftWhistleRight),
            new KeyBinding(InputKey.L, MixAction.SoftFinishRight),
            new KeyBinding(InputKey.Semicolon, MixAction.SoftClapRight),
        };

        public override DifficultyCalculator CreateDifficultyCalculator(IBeatmap beatmap, Mod[] mods = null) => new MixDifficultyCalculator(beatmap, mods);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new MixModEasy(),
                        new MixModNoFail(),
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new MixModHalfTime(),
                                new MixModDaycore(),
                            },
                        },
                    };

                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new MixModHardRock(),
                        new MixModSuddenDeath(),
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new MixModDoubleTime(),
                                new MixModNightcore(),
                            },
                        },
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new MixModHidden(),
                                new MixModFlashlight(),
                            },
                        },
                    };

                case ModType.Special:
                    return new Mod[]
                    {
                        new MixRelax()
                    };
                default : return new Mod[] { };
            }
        }
    }
}
