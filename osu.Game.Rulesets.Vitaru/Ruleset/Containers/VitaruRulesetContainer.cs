#region usings

using System;
using System.Collections.Generic;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Cursor;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Debug;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Scoring;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Containers
{
    public class VitaruRulesetContainer : RulesetContainer<VitaruHitObject>
    {
        private readonly DebugStat<int> ranked;

        private readonly bool rankedFilter = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.RankedFilter);

        public VitaruRulesetContainer(Rulesets.Ruleset ruleset, WorkingBeatmap beatmap, OsuNetworkingHandler osuNetworkingHandler = null, MatchInfo match = null)
            : base(ruleset, beatmap)
        {
            //TODO: make this a function if it works
            if (DebugToolkit.GeneralDebugItems.Count > 0)
                DebugToolkit.GeneralDebugItems = new List<Container>();

            DebugToolkit.GeneralDebugItems.Add(ranked = new DebugStat<int>(new Bindable<int>()) { Text = "Ranked" });

            VitaruPlayfield = CreateVitaruPlayfield((VitaruInputManager)KeyBindingInputManager, osuNetworkingHandler, match);
            VitaruScoreprocessor = new VitaruScoreProcessor(this, VitaruPlayfield);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            //This is a hack, please ignore it
            foreach (Drawable draw in VitaruPlayfield.VitaruInputManager.LoadCompleteChildren)
                VitaruPlayfield.VitaruInputManager.Add(draw);

            VitaruPlayfield.VitaruInputManager.LoadCompleteChildren = null;

            if (!rankedFilter)
            {
                OsuColour osu = new OsuColour();
                ranked.Text = "Ranked (Rank Filter Disabled)";
                ranked.SpriteText.Colour = osu.Green;
            }

            VitaruInputManager vitaruInputManager = (VitaruInputManager)KeyBindingInputManager;
            vitaruInputManager.DebugToolkit?.UpdateItems();
        }

        protected override void Update()
        {
            base.Update();

            //Ranked test, kinda sucks and needs to be re-done
            if (Clock.ElapsedFrameTime > 1000)
                ranked.Bindable.Value += 1000;
            else if (Clock.ElapsedFrameTime > 1000 / 10d)
                ranked.Bindable.Value += 100;
            else if (Clock.ElapsedFrameTime > 1000 / 30d)
                ranked.Bindable.Value += 10;
            else if (Clock.ElapsedFrameTime > 1000 / 45d)
                ranked.Bindable.Value += 5;
            else if (Clock.ElapsedFrameTime > 1000 / 60d)
                ranked.Bindable.Value++;

            if (ranked.Bindable.Value >= 1000 && rankedFilter)
            {
                OsuColour osu = new OsuColour();
                ranked.SpriteText.Colour = osu.Red;
                ranked.Text = "Unranked (Bad PC)";
            }
        }

        protected override CursorContainer CreateCursor() => new SymcolCursor();

        internal VitaruScoreProcessor VitaruScoreprocessor { get; private set; }

        public override ScoreProcessor CreateScoreProcessor() => VitaruScoreprocessor;

        protected override Playfield CreatePlayfield() => VitaruPlayfield;

        public VitaruPlayfield VitaruPlayfield { get; protected set; }

        protected virtual VitaruPlayfield CreateVitaruPlayfield(VitaruInputManager inputManager, OsuNetworkingHandler osuNetworkingHandler, MatchInfo match) => new VitaruPlayfield(inputManager, osuNetworkingHandler, match);

        public override int Variant => (int)variant();

        private readonly Bindable<string> character = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);
        private readonly Bindable<string> gamemode = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Gamemode);

        private ControlScheme variant()
        {
            if (gamemode == "Vitaru")
                return ControlScheme.Vitaru;
            else if (gamemode == "Dodge")
                return ControlScheme.Dodge;
            else
            {
                switch (character.Value)
                {
                    default:
                        return ControlScheme.Touhosu;
                    case "Sakuya Izayoi":
                        return ControlScheme.Sakuya;
                    case "Ryukoy Hakurei":
                        return ControlScheme.Ryukoy;
                }
            }
        }

        public override PassThroughInputManager CreateInputManager() => new VitaruInputManager(Ruleset.RulesetInfo, Variant);

        public override DrawableHitObject<VitaruHitObject> GetVisualRepresentation(VitaruHitObject h)
        {
            if (h is Cluster cluster)
                return new DrawableCluster(cluster, null);
            throw new InvalidOperationException("Only clusters allowed!");
        }

        protected override void Dispose(bool isDisposing)
        {
            VitaruScoreprocessor.Dispose();
            base.Dispose(isDisposing);
        }
    }
}
