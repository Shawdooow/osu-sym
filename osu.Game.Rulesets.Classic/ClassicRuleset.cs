// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Classic.Mods;
using osu.Game.Rulesets.Classic.Objects;
using osu.Game.Rulesets.Classic.ClassicDifficulty;
using osu.Game.Rulesets.Classic.UI;
using osu.Game.Rulesets.UI;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Framework.Input.Bindings;
using osu.Framework.IO.Stores;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Classic.Settings;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Classic.Edit;
using osu.Game.Rulesets.Classic.Beatmaps;

namespace osu.Game.Rulesets.Classic
{
    public class ClassicRuleset : Ruleset
    {
        public override RulesetContainer CreateRulesetContainerWith(WorkingBeatmap beatmap) => new ClassicRulesetContainer(this, beatmap);
        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new ClassicBeatmapConverter(beatmap);
        public override IBeatmapProcessor CreateBeatmapProcessor(IBeatmap beatmap) => new ClassicBeatmapProcessor(beatmap);

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0) => new[]
        {
            new KeyBinding(InputKey.Z, ClassicAction.LeftButton),
            new KeyBinding(InputKey.X, ClassicAction.RightButton),
            new KeyBinding(InputKey.MouseLeft, ClassicAction.LeftButton),
            new KeyBinding(InputKey.MouseRight, ClassicAction.RightButton),
        };

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new ClassicModEasy(),
                        new ClassicModNoFail(),
                        new ClassicModHalfTime(),
                    };

                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new ClassicModHardRock(),
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new ClassicModSuddenDeath(),
                                new ClassicModPerfect(),
                            },
                        },
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new ClassicModDoubleTime(),
                                new ClassicModNightcore(),
                            },
                        },
                        new ClassicModHidden(),
                        new ClassicModFlashlight(),
                    };

                case ModType.Special:
                    return new Mod[]
                    {
                        new ClassicModRelax(),
                        new ClassicModAutopilot(),
                        new ClassicModSpunOut(),
                        new MultiMod
                        {
                            Mods = new Mod[]
                            {
                                new ClassicModAutoplay(),
                                new ModCinema(),
                            },
                        },
                        new ClassicModTarget(),
                    };

                default:
                    return new Mod[] { };
            }
        }

        public override DifficultyCalculator CreateDifficultyCalculator(IBeatmap beatmap, Mod[] mods = null) => new ClassicDifficultyCalculator(beatmap, mods);

        public override int? LegacyID => 6;

        public override string Description => "classic!";

        public override string ShortName => "classic";

        public override HitObjectComposer CreateHitObjectComposer() => new ClassicHitObjectComposer(this);

        public override SettingsSubsection CreateSettings() => new ClassicSettings();

        public override Drawable CreateIcon() => new Sprite { Texture = ClassicTextures.Get("icon") };

        public static ResourceStore<byte[]> ClassicResources;
        public static TextureStore ClassicTextures;

        public ClassicRuleset(RulesetInfo rulesetInfo)
            : base(rulesetInfo)
        {
            if (ClassicResources == null)
            {
                ClassicResources = new ResourceStore<byte[]>();
                ClassicResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Game.Rulesets.Classic.dll"), ("Assets")));
                ClassicResources.AddStore(new DllResourceStore("osu.Game.Rulesets.Classic.dll"));
                ClassicTextures = new TextureStore(new RawTextureLoaderStore(new NamespacedResourceStore<byte[]>(ClassicResources, @"Textures")));
                ClassicTextures.AddStore(new RawTextureLoaderStore(new OnlineStore()));
            }
        }
    }
}
