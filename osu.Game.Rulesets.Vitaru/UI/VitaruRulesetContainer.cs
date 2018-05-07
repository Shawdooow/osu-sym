using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Vitaru.Beatmaps;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Scoring;
using OpenTK;
using osu.Game.Rulesets.Vitaru.UI.Cursor;
using osu.Framework.Graphics.Cursor;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.Characters;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class VitaruRulesetContainer : RulesetContainer<VitaruHitObject>
    {
        public VitaruRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap, bool isForCurrentRuleset)
            : base(ruleset, beatmap, isForCurrentRuleset)
        {
            VitaruPlayfield = new VitaruPlayfield((VitaruInputManager)KeyBindingInputManager);
        }

        protected override CursorContainer CreateCursor() => new GameplayCursor();

        public override ScoreProcessor CreateScoreProcessor() => new VitaruScoreProcessor(this);

        protected override BeatmapConverter<VitaruHitObject> CreateBeatmapConverter() => new VitaruBeatmapConverter();

        protected override BeatmapProcessor<VitaruHitObject> CreateBeatmapProcessor() => new VitaruBeatmapProcessor();

        protected override Playfield CreatePlayfield() => VitaruPlayfield;

        protected VitaruPlayfield VitaruPlayfield;

        public override int Variant => (int)variant();

        private TouhosuCharacters selectedTouhosuCharacter = VitaruSettings.VitaruConfigManager.GetBindable<TouhosuCharacters>(VitaruSetting.TouhosuCharacter);
        private readonly Gamemodes currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);

        private ControlScheme variant()
        {
            if (currentGameMode == Gamemodes.Vitaru)
                return ControlScheme.Vitaru;
            else if (currentGameMode == Gamemodes.Dodge)
                return ControlScheme.Dodge;
            else
            {
                if (selectedTouhosuCharacter == TouhosuCharacters.SakuyaIzayoi || selectedTouhosuCharacter == TouhosuCharacters.RyukoyHakurei || selectedTouhosuCharacter == TouhosuCharacters.TomajiHakurei)
                    return ControlScheme.Sakuya;
                //else if (currentCharacter == Player.KokoroHatano)
                    //return ControlScheme.Kokoro;
                //else if (currentCharacter == Player.NueHoujuu)
                    //return ControlScheme.NueHoujuu;
                //else if (currentCharacter == Player.AliceMuyart)
                    //return ControlScheme.AliceMuyart;
                else
                    return ControlScheme.Touhosu;
            }
        }

        public override PassThroughInputManager CreateInputManager() => new VitaruInputManager(Ruleset.RulesetInfo, Variant);

        protected override DrawableHitObject<VitaruHitObject> GetVisualRepresentation(VitaruHitObject h)
        {
            if (h is Bullet bullet)
                return new DrawableBullet(bullet, VitaruPlayfield);
            if (h is Laser laser)
                return new DrawableLaser(laser, VitaruPlayfield);
            if (h is Pattern pattern)
                return new DrawablePattern(pattern, VitaruPlayfield);
            return null;
        }

        //protected override FramedReplayInputHandler CreateReplayInputHandler(Replay replay) => new VitaruReplayInputHandler(replay);

        protected override Vector2 GetAspectAdjustedSize()
        {
            var aspectSize = DrawSize.X * 0.75f < DrawSize.Y ? new Vector2(DrawSize.X, DrawSize.X * 0.75f) : new Vector2(DrawSize.Y * 10f / 16f, DrawSize.Y);

            if (currentGameMode == Gamemodes.Dodge)
                aspectSize = DrawSize.X * 0.75f < DrawSize.Y ? new Vector2(DrawSize.X, DrawSize.X * 0.75f) : new Vector2(DrawSize.Y * 4f / 3f, DrawSize.Y);
            else if (currentGameMode == Gamemodes.Gravaru)
                aspectSize = DrawSize.X * 0.75f < DrawSize.Y ? new Vector2(DrawSize.X, DrawSize.X * 0.75f) : new Vector2(DrawSize.Y * 2f / 1f, DrawSize.Y);

            return new Vector2(aspectSize.X / DrawSize.X, aspectSize.Y / DrawSize.Y);
        }
    }
}
