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

namespace osu.Game.Rulesets.Mix
{
    public class MixRuleset : Ruleset
    {
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

        public override RulesetContainer CreateRulesetContainerWith(WorkingBeatmap beatmap, bool isForCurrentRuleset) => new MixRulesetContainer(this, beatmap, isForCurrentRuleset);

        public override int? LegacyID => 7;

        public override string Description => "mix!";

        public override string ShortName => "mix";

        public override DifficultyCalculator CreateDifficultyCalculator(Beatmap beatmap, Mod[] mods = null) => new MixDifficultyCalculator(beatmap, mods);

        public override SettingsSubsection CreateSettings() => new MixSettings();

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.A, MixAction.EastLeftButton),
            new KeyBinding(InputKey.S, MixAction.WestLeftButton),
            new KeyBinding(InputKey.D, MixAction.NorthLeftButton),
            new KeyBinding(InputKey.F, MixAction.SouthLeftButton),
            new KeyBinding(InputKey.J, MixAction.SouthRightButton),
            new KeyBinding(InputKey.K, MixAction.NorthRightButton),
            new KeyBinding(InputKey.L, MixAction.WestRightButton),
            new KeyBinding(InputKey.Semicolon, MixAction.EastRightButton),
        };

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
