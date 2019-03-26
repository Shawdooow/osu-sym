#region usings

using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.Pieces;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Debug;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers
{
    public class DrawableTouhosuPlayer : DrawableVitaruPlayer
    {
        private readonly bool boss = VitaruSettings.VitaruConfigManager.Get<bool>(VitaruSetting.KiaiBoss);

        public readonly TouhosuPlayer TouhosuPlayer;

        public double Energy { get; private set; }

        protected bool SpellActive { get; set; }

        protected bool EnergyHacks { get; private set; }

        protected OsuTextFlowContainer TextContainer { get; private set; }

        protected Totem LeftTotem;
        protected Totem RightTotem;

        public DrawableTouhosuPlayer(VitaruPlayfield playfield, TouhosuPlayer player) : base(playfield, player)
        {
            TouhosuPlayer = player;
            Charge(TouhosuPlayer.MaxEnergy / 2d);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (ControlType == ControlType.Player)
                DebugToolkit.GeneralDebugItems.Add(new DebugAction(() => { EnergyHacks = !EnergyHacks; }) { Text = "Energy Hacks" });

            AddRange(new Drawable[]
            {
                LeftTotem = new Totem(this, VitaruPlayfield)
                {
                    Position = new Vector2(-30, -40),
                    StartAngle = -25,
                },
                RightTotem = new Totem(this, VitaruPlayfield)
                {
                    Position = new Vector2(30, -40),
                    StartAngle = 25,
                }
            });
        }

        protected override void Update()
        {
            base.Update();

            if (EnergyHacks)
                Charge(999999);

            if (HealingProjectiles.Count > 0)
            {
                double fallOff = 1;

                for (int i = 0; i < HealingProjectiles.Count - 1; i++)
                    fallOff *= HEALING_FALL_OFF;

                foreach (HealingProjectile healingBullet in HealingProjectiles)
                    Charge(Clock.ElapsedFrameTime / 500 * (GetBulletHealingMultiplier(healingBullet.EdgeDistance) * fallOff));
            }

            SpellUpdate();
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            if (effectPoint.KiaiMode && boss && LeftTotem.Alpha == 0)
            {
                LeftTotem.FadeIn(timingPoint.BeatLength);
                RightTotem.FadeIn(timingPoint.BeatLength);
            }

            if (!effectPoint.KiaiMode && boss && LeftTotem.Alpha > 0)
            {
                LeftTotem.FadeOut(timingPoint.BeatLength);
                RightTotem.FadeOut(timingPoint.BeatLength);
            }
        }

        #region Spell Handling
        /// <summary>
        /// Called to see if a spell should go active
        /// </summary>
        protected virtual bool CheckSpellActivate(VitaruAction action)
        {
            if (action == VitaruAction.Spell && Energy >= TouhosuPlayer.EnergyCost)
                return true;
            return false;
        }

        /// <summary>
        /// Called to see if a spell should be deactivated
        /// </summary>
        /// <param name="action"></param>
        protected virtual bool CheckSpellDeactivate(VitaruAction action)
        {
            if (action == VitaruAction.Spell)
                return true;
            return false;
        }

        /// <summary>
        /// Called when a spell is activated
        /// </summary>
        /// <param name="action"></param>
        protected virtual void SpellActivate(VitaruAction action)
        {
            SpellActive = true;
            if (TouhosuPlayer.EnergyDrainRate == 0)
                Drain(TouhosuPlayer.EnergyCost);
        }

        protected virtual void SpellUpdate()
        {
            if (Energy <= 0)
            {
                Energy = 0;
                SpellDeactivate(VitaruAction.Spell);
            }
        }

        /// <summary>
        /// Called when a spell is deactivated
        /// </summary>
        /// <param name="action"></param>
        protected virtual void SpellDeactivate(VitaruAction action)
        {
            SpellActive = false;
        }

        #endregion

        public virtual double Charge(double amount)
        {
            Energy += amount;

            if (Energy > TouhosuPlayer.MaxEnergy)
                Energy = TouhosuPlayer.MaxEnergy;

            return Energy;
        }

        public virtual double Drain(double amount)
        {
            Energy -= amount;

            if (Energy < 0)
                Energy = 0;

            return Energy;
        }

        protected override void PatternWave()
        {
            base.PatternWave();

            if (LeftTotem.Alpha > 0)
            {
                //TODO: fix totems
                //LeftTotem.Shoot();
                //RightTotem.Shoot();
            }
        }

        protected override bool Pressed(VitaruAction action)
        {
            if (CheckSpellActivate(action))
                SpellActivate(action);

            return base.Pressed(action);
        }

        protected override bool Released(VitaruAction action)
        {
            if (CheckSpellDeactivate(action))
                SpellDeactivate(action);

            return base.Released(action);
        }

        protected virtual void Speak(string text)
        {
            if (TextContainer == null)
                Add(TextContainer = new OsuTextFlowContainer(t => { t.TextSize = 24; })
                {
                    Alpha = 0,
                    Position = new Vector2(0, 48),
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.TopCentre,
                    AutoSizeAxes = Axes.Both,
                    Text = "",
                });

            TextContainer.ClearTransforms();
            TextContainer.Text = text;
            double lengthOfSpeaking = 0;

            int y = 150;
            foreach (char i in text)
            {
                lengthOfSpeaking += y;
                y++;
            }

            TextContainer.FadeTo(0.5f, 200)
                         .Delay(lengthOfSpeaking)
                         .FadeOut(200);
        }
    }
}
