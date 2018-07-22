using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using OpenTK;
using osu.Game.Rulesets.Vitaru.Judgements;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Framework.Graphics.Cursor;
using osu.Game.Rulesets.Vitaru.UI.Cursor;
using osu.Framework.Configuration;
using System.Collections.Generic;
using osu.Game.Rulesets.Vitaru.Multi;
using Symcol.Rulesets.Core.Rulesets;
using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers.DrawableVitaruPlayers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Vitaru.Characters.Bosses;
using osu.Game.Rulesets.Vitaru.Characters.Bosses.DrawableBosses;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers;
using OpenTK.Graphics;
using osu.Game.Rulesets.Vitaru.Debug;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Pieces;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class VitaruPlayfield : SymcolPlayfield
    {
        private static readonly Bindable<Gamemodes> gamemode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.Gamemode);

        private readonly Bindable<bool> goodFps = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.GoodFPS);

        private readonly bool playfieldBorder = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.PlayfieldBorder);

        private readonly bool kiaiBoss = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.KiaiBoss);

        private readonly string character = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);

        public readonly VitaruInputManager VitaruInputManager;

        //public readonly MirrorField Mirrorfield;

        public static Action<Judgement> OnJudgement;
        //public Action<Judgement> RemoveJudgement;

        public readonly AspectLockedPlayfield Gamefield;

        public readonly Container BorderContainer;

        private readonly Container judgementLayer;
        private readonly List<DrawableVitaruPlayer> playerList = new List<DrawableVitaruPlayer>();

        //TODO: Make this not need to be static?
        public static List<VitaruClientInfo> LoadPlayerList = new List<VitaruClientInfo>();

        public readonly DrawableVitaruPlayer Player;

        public DrawableBoss DrawableBoss;

        public virtual bool Editor => false;

        public static Vector2 BaseSize
        {
            get
            {
                if (gamemode == Gamemodes.Touhosu)
                    return new Vector2(512 * 2, 820);
                else if (gamemode == Gamemodes.Dodge)
                    return new Vector2(512, 384);
                else
                    return new Vector2(512, 820);
            }
        }

        private readonly DebugStat<int> returnedJudgementCount;
        private readonly DebugStat<int> drawableHitobjectCount;
        private readonly DebugStat<int> drawablePatternCount;
        private readonly DebugStat<int> ranked;

        public VitaruPlayfield(VitaruInputManager vitaruInput) : base(BaseSize)
        {
            VitaruInputManager = vitaruInput;

            DrawableBullet.BoundryHacks = false;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Children = new Drawable[]
            {
                new PlayfieldDust(),
                judgementLayer = new Container
                {
                    Name = "Judgements",
                    RelativeSizeAxes = Axes.Both
                },
                Gamefield = new AspectLockedPlayfield
                {
                    Margin = 1
                }
            };

            DebugToolkit.GeneralDebugItems.Add(returnedJudgementCount = new DebugStat<int>(new Bindable<int>()) { Text = "Returned Judgement Count" });
            DebugToolkit.GeneralDebugItems.Add(drawableHitobjectCount = new DebugStat<int>(new Bindable<int>()) { Text = "Drawable Hitobject Count" });
            DebugToolkit.GeneralDebugItems.Add(drawablePatternCount = new DebugStat<int>(new Bindable<int>()) { Text = "Drawable Pattern Count" });
            
            if (playfieldBorder)
                Add(BorderContainer = new Container
                {
                    Name = "Border",
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    BorderColour = Color4.White,
                    BorderThickness = 3,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        AlwaysPresent = true
                    }
                });

            if (!Editor)
            {
                VitaruNetworkingClientHandler vitaruNetworkingClientHandler = RulesetNetworkingClientHandler as VitaruNetworkingClientHandler;

                if (gamemode == Gamemodes.Touhosu)
                    playerList.Add(Player = DrawableTouhosuPlayer.GetDrawableTouhosuPlayer(this, character, vitaruNetworkingClientHandler));
                else
                    playerList.Add(Player = DrawableVitaruPlayer.GetDrawableVitaruPlayer(this, character, vitaruNetworkingClientHandler));

                DebugToolkit.GeneralDebugItems.Add(new DebugAction()
                {
                    Text = "Add New Player",
                    Action = () =>
                    {
                        if (gamemode == Gamemodes.Touhosu)
                            Add(DrawableTouhosuPlayer.GetDrawableTouhosuPlayer(this, character, vitaruNetworkingClientHandler));
                        else
                            Add(DrawableVitaruPlayer.GetDrawableVitaruPlayer(this, character, vitaruNetworkingClientHandler));
                    }
                });

                foreach (DrawableVitaruPlayer player in playerList)
                    Gamefield.Add(player);

                if (gamemode == Gamemodes.Touhosu && kiaiBoss)
                    Gamefield.Add(DrawableBoss = new DrawableBoss(this, new Kokoro()));

                Player.Position = new Vector2(256, 700);
                if (gamemode == Gamemodes.Dodge)
                    Player.Position = BaseSize / 2;
                else if (gamemode == Gamemodes.Touhosu)
                    Player.Position = new Vector2(512, 700);
            }
            else
                Player = null;

            DebugToolkit.GeneralDebugItems.Add(new DebugAction() { Text = "Exclusive Testing Hax", Action = () => { BulletPiece.ExclusiveTestingHax = !BulletPiece.ExclusiveTestingHax; } });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            var cursor = CreateCursor();
            if (cursor != null)
                AddInternal(cursor);
        }

        private readonly List<Pattern> patterns = new List<Pattern>();

        private void add(Pattern p)
        {
            DrawablePattern drawable = new DrawablePattern(p, this);

            drawable.Depth = (float)drawable.HitObject.StartTime;

            drawable.Editor = Editor || !goodFps;

            drawableHitobjectCount.Bindable.Value++;
            drawable.OnDispose += isDisposing => { drawableHitobjectCount.Bindable.Value--; };

            drawablePatternCount.Bindable.Value++;
            drawable.OnDispose += isDisposing => { drawablePatternCount.Bindable.Value--; };

            drawable.OnJudgement += onJudgement;

            base.Add(drawable);
        }

        public override void Add(DrawableHitObject h)
        {
            if (!(h is DrawablePattern drawable))
                throw new InvalidOperationException("Only DrawablePatterss can be added to playfield!");

            if (Editor || !goodFps)
                add((Pattern)h.HitObject);
            else
                patterns.Add((Pattern)h.HitObject);
        }

        protected override void Update()
        {
            base.Update();

            foreach (Pattern p in patterns)
                if (Time.Current >= p.StartTime - p.TimePreempt * 2)
                {
                    add(p);
                    patterns.Remove(p);
                    break;
                }
        }

        private void onJudgement(DrawableHitObject judgedObject, Judgement judgement)
        {
            var vitaruJudgement = (VitaruJudgement)judgement;

            OnJudgement?.Invoke(vitaruJudgement);

            returnedJudgementCount.Bindable.Value++;

            DrawableVitaruJudgement explosion = new DrawableVitaruJudgement(judgement, judgedObject)
            {
                Alpha = 0.5f,
                Origin = Anchor.Centre,
                Position = judgedObject.Position
            };

            judgementLayer.Add(explosion);
        }

        protected virtual CursorContainer CreateCursor() => new GameplayCursor();

        protected override void Dispose(bool isDisposing)
        {
            BulletPiece.ExclusiveTestingHax = false;
            OnJudgement = null;
            base.Dispose(isDisposing);
        }
    }
}
