#region usings

using osu.Framework.Audio.Track;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.HitObjects.DrawableHitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Abilities.Buffs;
using osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Worship.Characters.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Characters
{
    public class Enemy : DrawableCharacter
    {
        private Bindable<int> souls = VitaruSettings.VitaruConfigManager.GetBindable<int>(VitaruSetting.Souls);

        public override double MaxHealth => 60;

        protected override string CharacterName => "enemy";

        public override Color4 PrimaryColor => characterColor;

        protected override float HitboxWidth => 48;

        private readonly Color4 characterColor;

        private DrawableVitaruCluster drawableCluster;

        public Enemy(VitaruPlayfield playfield, DrawableVitaruCluster drawableCluster) : base(playfield)
        {
            this.drawableCluster = drawableCluster;

            AlwaysPresent = true;

            characterColor = drawableCluster.AccentColour;
        }

        protected override void MovementAnimations()
        {
            if (LeftSprite.Texture == null && RightSprite != null)
            {
                LeftSprite.Texture = RightSprite.Texture;
                LeftSprite.Size = new Vector2(-RightSprite.Size.X, RightSprite.Size.Y);
            }
            if (KiaiLeftSprite.Texture == null && KiaiRightSprite != null)
            {
                KiaiLeftSprite.Texture = KiaiRightSprite.Texture;
                KiaiLeftSprite.Size = new Vector2(-KiaiRightSprite.Size.X, KiaiRightSprite.Size.Y);
            }
            if (Position.X > LastX)
            {
                if (LeftSprite.Texture != null)
                    LeftSprite.Alpha = 0;
                if (RightSprite?.Texture != null)
                    RightSprite.Alpha = 1;
                if (StillSprite.Texture != null)
                    StillSprite.Alpha = 0;
                if (KiaiLeftSprite.Texture != null)
                    KiaiLeftSprite.Alpha = 0;
                if (KiaiRightSprite?.Texture != null)
                    KiaiRightSprite.Alpha = 1;
                if (KiaiStillSprite.Texture != null)
                    KiaiStillSprite.Alpha = 0;
            }
            else if (Position.X < LastX)
            {
                if (LeftSprite.Texture != null)
                    LeftSprite.Alpha = 1;
                if (RightSprite?.Texture != null)
                    RightSprite.Alpha = 0;
                if (StillSprite.Texture != null)
                    StillSprite.Alpha = 0;
                if (KiaiLeftSprite.Texture != null)
                    KiaiLeftSprite.Alpha = 1;
                if (KiaiRightSprite?.Texture != null)
                    KiaiRightSprite.Alpha = 0;
                if (KiaiStillSprite.Texture != null)
                    KiaiStillSprite.Alpha = 0;
            }
            LastX = Position.X;
        }

        protected override void LoadAnimationSprites(TextureStore textures, Storage storage)
        {
            base.LoadAnimationSprites(textures, storage);
            RightSprite.Texture = VitaruSkinElement.LoadSkinElement(CharacterName, storage);
            KiaiRightSprite.Texture = VitaruSkinElement.LoadSkinElement(CharacterName + "Kiai", storage);
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            if (effectPoint.KiaiMode && SoulContainer.Alpha == 1)
            {
                SoulContainer.FadeOutFromOne(timingPoint.BeatLength / 4);

                if (VitaruPlayfield.DrawableBoss == null)
                    KiaiContainer.FadeInFromZero(timingPoint.BeatLength / 4);
                else
                    Hitbox.HitDetection = false;
            }
            if (!effectPoint.KiaiMode && SoulContainer.Alpha == 0)
            {
                SoulContainer.FadeInFromZero(timingPoint.BeatLength);

                if (VitaruPlayfield.DrawableBoss == null)
                    KiaiContainer.FadeOutFromOne(timingPoint.BeatLength);
                else
                    Hitbox.HitDetection = true;
            }
        }

        protected override void Death()
        {
            Dead = true;
            Hitbox.HitDetection = false;
            drawableCluster.Death(this);

            if (souls < 100 && Untuned)
                souls.Value++;

            if (VitaruPlayfield.Player is DrawableReimu)
            {
                Buff buff = new Buff(VitaruPlayfield)
                {
                    Position = Position
                };
                VitaruPlayfield.Gamefield.Add(buff);
                buff.Untuned = Untuned;
                buff.MoveTo(new Vector2(VitaruPlayfield.Player.X, Position.Y - 100), 250, Easing.OutCubic)
                    .Delay(500)
                    .MoveToY(Position.Y + 1000, 8000, Easing.InSine);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            souls.UnbindAll();
            souls = null;
            drawableCluster = null;
            base.Dispose(isDisposing);
        }
    }
}
