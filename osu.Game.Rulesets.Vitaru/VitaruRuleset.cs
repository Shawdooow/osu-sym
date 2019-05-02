#region usings

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Core;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.IO.Stores;
using osu.Game.Beatmaps;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Vitaru.ChapterSets;
using osu.Game.Rulesets.Vitaru.Ruleset;
using osu.Game.Rulesets.Vitaru.Ruleset.Beatmaps;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Edit;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osu.Game.Rulesets.Vitaru.Sym.Multi;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi;
using osu.Mods.Online.Multi.Rulesets;
using osu.Mods.Online.Multi.Settings.Options;

#endregion

namespace osu.Game.Rulesets.Vitaru
{
    public sealed class VitaruRuleset : Rulesets.Ruleset, IRulesetMulti
    {
        public static Bindable<double> MEMORY_LEAKED = new Bindable<double>();

        public override int? LegacyID => 4; 

        public override string Description => "vitaru!";

        public override string ShortName => "vitaru";

        public override IEnumerable<int> AvailableVariants
        {
            get
            {
                for (int i = 0; i <= (int)ControlScheme.Ryukoy; i++)
                    yield return (int)ControlScheme.Vitaru + i;
            }
        }

        public DrawableRuleset CreateRulesetContainerMulti(WorkingBeatmap beatmap, OsuNetworkingHandler networking, MatchInfo match) => new VitaruRulesetContainer(this, beatmap, networking, match);

        public Container<MultiplayerOption> RulesetSettings(OsuNetworkingHandler networking)
        {
            VitaruOnlineGamemodeSelection g = new VitaruOnlineGamemodeSelection(networking, 2);
            VitaruOnlineCharacterSelection c = new VitaruOnlineCharacterSelection(networking, 1, g.Gamemode);

            return new Container<MultiplayerOption>
            {
                RelativeSizeAxes = Axes.Both,

                Children = new MultiplayerOption[]
                {
                    g,
                    c,
                }
            };
        }

        public override IEnumerable<KeyBinding> GetDefaultKeyBindings(int variant = 0)
        {
            switch (getControlType(variant))
            {
                case ControlScheme.Vitaru:
                    return new[]
                    {
                        new KeyBinding(InputKey.W, VitaruAction.Up),
                        new KeyBinding(InputKey.S, VitaruAction.Down),
                        new KeyBinding(InputKey.A, VitaruAction.Left),
                        new KeyBinding(InputKey.D, VitaruAction.Right),
                        new KeyBinding(InputKey.Shift, VitaruAction.Slow),
                        new KeyBinding(InputKey.MouseLeft, VitaruAction.Shoot),
                    };
                case ControlScheme.Dodge:
                    return new[]
                    {
                        new KeyBinding(InputKey.W, VitaruAction.Up),
                        new KeyBinding(InputKey.S, VitaruAction.Down),
                        new KeyBinding(InputKey.A, VitaruAction.Left),
                        new KeyBinding(InputKey.D, VitaruAction.Right),
                        new KeyBinding(InputKey.Shift, VitaruAction.Slow),
                    };
                case ControlScheme.Touhosu:
                    return new[]
                    {
                        new KeyBinding(InputKey.W, VitaruAction.Up),
                        new KeyBinding(InputKey.S, VitaruAction.Down),
                        new KeyBinding(InputKey.A, VitaruAction.Left),
                        new KeyBinding(InputKey.D, VitaruAction.Right),
                        new KeyBinding(InputKey.Shift, VitaruAction.Slow),
                        new KeyBinding(InputKey.MouseLeft, VitaruAction.Shoot),
                        new KeyBinding(InputKey.MouseRight, VitaruAction.Spell),
                    };
                case ControlScheme.Sakuya:
                    return new[]
                    {
                        new KeyBinding(InputKey.W, VitaruAction.Up),
                        new KeyBinding(InputKey.S, VitaruAction.Down),
                        new KeyBinding(InputKey.A, VitaruAction.Left),
                        new KeyBinding(InputKey.D, VitaruAction.Right),
                        new KeyBinding(InputKey.Shift, VitaruAction.Slow),
                        new KeyBinding(InputKey.MouseLeft, VitaruAction.Shoot),
                        new KeyBinding(InputKey.MouseRight, VitaruAction.Spell),
                        new KeyBinding(InputKey.E, VitaruAction.Increase),
                        new KeyBinding(InputKey.Q, VitaruAction.Decrease),
                    };
                case ControlScheme.Ryukoy:
                    return new[]
                    {
                        new KeyBinding(InputKey.W, VitaruAction.Up),
                        new KeyBinding(InputKey.S, VitaruAction.Down),
                        new KeyBinding(InputKey.A, VitaruAction.Left),
                        new KeyBinding(InputKey.D, VitaruAction.Right),
                        new KeyBinding(InputKey.Shift, VitaruAction.Slow),
                        new KeyBinding(InputKey.MouseLeft, VitaruAction.Shoot),
                        new KeyBinding(InputKey.MouseRight, VitaruAction.Spell),
                        new KeyBinding(InputKey.F, VitaruAction.Pull),
                    };
            }

            return new KeyBinding[0];
        }

        public override string GetVariantName(int variant)
        {
            switch (getControlType(variant))
            {
                default:
                    return "null";
                case ControlScheme.Vitaru:
                    return "Vitaru";
                case ControlScheme.Dodge:
                    return "Dodge";
                case ControlScheme.Touhosu:
                    return "Touhosu";
                case ControlScheme.Sakuya:
                    return "Sakuya";
                case ControlScheme.Ryukoy:
                    return "Ryukoy";
            }
        }

        private ControlScheme getControlType(int variant) => (ControlScheme)Enum.GetValues(typeof(ControlScheme)).Cast<int>().OrderByDescending(i => i).First(v => variant >= v);

        public override IEnumerable<Mod> GetModsFor(ModType type)
        {
            switch (type)
            {
                case ModType.DifficultyReduction:
                    return new Mod[]
                    {
                        new VitaruModEasy(),
                        new VitaruModNoFail(),
                        new MultiMod(new VitaruModHalfTime(), new VitaruModTrueHalfTime(), new VitaruModDaycore(), new VitaruModTrueDaycore()),
                        new VitaruModCharged(),
                    };
                case ModType.DifficultyIncrease:
                    return new Mod[]
                    {
                        new VitaruModHardRock(),
                        new MultiMod(new VitaruModSuddenDeath(), new VitaruModPerfect(), new VitaruModTruePerfect()),
                        new MultiMod(new VitaruModDoubleTime(), new VitaruModTrueDoubleTime(), new VitaruModNightcore(), new VitaruModTrueNightcore()),
                        new MultiMod(new VitaruModHidden(), new VitaruModTrueHidden()),
                        new VitaruModFlashlight(), 
                    };
                case ModType.Fun:
                    return new Mod[]
                    {
                        new MultiMod(new VitaruModAccel(), new VitaruModDeccel()),
                    };
                default: return new Mod[] { };
            }
        }

        public override DrawableRuleset CreateDrawableRulesetWith(WorkingBeatmap beatmap, IReadOnlyList<Mod> mods) => new VitaruRulesetContainer(this, beatmap);

        public override IBeatmapConverter CreateBeatmapConverter(IBeatmap beatmap) => new VitaruBeatmapConverter(beatmap);

        public override DifficultyCalculator CreateDifficultyCalculator(WorkingBeatmap beatmap) => new VitaruDifficultyCalculator(this, beatmap);

        public override HitObjectComposer CreateHitObjectComposer() => VitaruSettings.Experimental ? new VitaruHitObjectComposer(this) : null;

        public override RulesetSettingsSubsection CreateSettings() => new VitaruSettings(this);

        public override IBeatmapProcessor CreateBeatmapProcessor(IBeatmap beatmap) => new VitaruBeatmapProcessor(beatmap);

        public override Drawable CreateIcon()
        {
            Texture t;

            try
            {
                t = ChapterStore.GetChapterSet(VitaruSettings.Gamemode).Icon ?? VitaruTextures.Get("icon");
            }
            catch
            {
                t = VitaruTextures.Get("icon");
            }

            Sprite icon = new Sprite { Texture = t };
            Container container = new Container
            {
                AutoSizeAxes = Axes.Both,
                Child = icon,
            };
            Icons.Add(icon);

            container.OnLoadComplete += d =>
            {
                SymManager.LoadComplete(VitaruSettings.Osu, VitaruSettings.Host);
            };

            return container;
        }

        //TODO: Custom Touhosu Icon?
        internal static readonly List<Sprite> Icons = new List<Sprite>();

        public static ResourceStore<byte[]> VitaruResources { get; private set; }
        public static TextureStore VitaruTextures { get; private set; }
        public static SampleManager VitaruSamples { get; internal set; }

        public VitaruRuleset(RulesetInfo rulesetInfo = null) : base(rulesetInfo)
        {
            if (VitaruResources == null)
            {
                VitaruResources = new ResourceStore<byte[]>();
                VitaruResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Game.Rulesets.Vitaru.dll"), "Assets"));
                VitaruTextures = new TextureStore(new TextureLoaderStore(new NamespacedResourceStore<byte[]>(VitaruResources, @"Textures")));
            }
        }
    }

    public enum ControlScheme
    {
        Vitaru,
        Dodge,

        Touhosu,
        Sakuya,
        Ryukoy,
    }
}
