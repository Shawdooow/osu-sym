using System.Collections.Generic;
using osu.Game.Rulesets.UI;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Shape.Mods;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Audio;
using osu.Framework.Input.Bindings;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Shape.Settings;

namespace osu.Game.Rulesets.Shape
{
    public class MixRuleset : Ruleset
    {
        public override Drawable CreateIcon() => new Sprite { Texture = MixTextures.Get("icon") };

        public static ResourceStore<byte[]> MixResources;
        public static TextureStore MixTextures;
        public static AudioManager ShapeClassicAudio;

        public MixRuleset(RulesetInfo rulesetInfo)
            : base(rulesetInfo)
        {
            MixResources = new ResourceStore<byte[]>();
            MixResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Game.Rulesets.Mix.dll"), ("Assets")));
            MixResources.AddStore(new DllResourceStore("osu.Game.Rulesets.Mix.dll"));
            MixTextures = new TextureStore(new RawTextureLoaderStore(new NamespacedResourceStore<byte[]>(MixResources, @"Textures")));
            MixTextures.AddStore(new RawTextureLoaderStore(new OnlineStore()));
        }

        public override RulesetContainer CreateRulesetContainerWith(WorkingBeatmap beatmap, bool isForCurrentRuleset) => new ShapeRulesetContainer(this, beatmap, isForCurrentRuleset);

        public override int? LegacyID => 7;

        public override string Description => "mix!";

        public override string ShortName => "mix";

        public override DifficultyCalculator CreateDifficultyCalculator(Beatmap beatmap, Mod[] mods = null) => new MixDifficultyCalculator(beatmap, mods);

        public override SettingsSubsection CreateSettings() => new ShapeSettings();

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.A, ShapeAction.EastLeftButton),
            new KeyBinding(InputKey.S, ShapeAction.WestLeftButton),
            new KeyBinding(InputKey.D, ShapeAction.NorthLeftButton),
            new KeyBinding(InputKey.F, ShapeAction.SouthLeftButton),
            new KeyBinding(InputKey.J, ShapeAction.SouthRightButton),
            new KeyBinding(InputKey.K, ShapeAction.NorthRightButton),
            new KeyBinding(InputKey.L, ShapeAction.WestRightButton),
            new KeyBinding(InputKey.Semicolon, ShapeAction.EastRightButton),
        };

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new ShapeModEasy(),
                        new ShapeModNoFail(),
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new ShapeModHalfTime(),
                                new ShapeModDaycore(),
                            },
                        },
                    };

                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new ShapeModHardRock(),
                        new ShapeModSuddenDeath(),
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new ShapeModDoubleTime(),
                                new ShapeModNightcore(),
                            },
                        },
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new ShapeModHidden(),
                                new ShapeModFlashlight(),
                            },
                        },
                    };

                case ModType.Special:
                    return new Mod[]
                    {
                        new ShapeRelax()
                    };
                default : return new Mod[] { };
            }
        }
    }
}
