using System;
using osu.Framework.Audio.Track;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.MathUtils;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Cursor;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;
using osuTK.Graphics;
using Symcol.Base.Graphics.Sprites;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables.Pieces
{
    public class BulletPiece : BeatSyncedContainer
    {
        private readonly GraphicsOptions graphics = VitaruSettings.VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.BulletVisuals);

        private readonly Sprite bulletKiai;
        private SymcolSprite dean;
        private readonly CircularContainer circle;
        public readonly Box Box;

        public static bool ExclusiveTestingHax;

        private readonly float randomRotationValue;
        private readonly bool randomRotateDirection;

        private readonly DrawableBullet drawableBullet;

        public BulletPiece(DrawableBullet drawableBullet)
        {
            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            this.drawableBullet = drawableBullet;

            randomRotationValue = (float)RNG.Next(10, 15) / 10;
            randomRotateDirection = RNG.NextBool();

            if (graphics == GraphicsOptions.Old)
                Child = bulletKiai = new Sprite
                {
                    Scale = new Vector2(2),
                    Alpha = 0,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Colour = drawableBullet.AccentColour,
                    Texture = VitaruRuleset.VitaruTextures.Get("bulletKiai"),
                };

            Add(circle = new CircularContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Alpha = 1,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(graphics != GraphicsOptions.Old && graphics != GraphicsOptions.Experimental ? 1 : 1.05f),
                BorderColour = drawableBullet.AccentColour,
                BorderThickness = graphics != GraphicsOptions.Old ? 0 : 6,
                Masking = true,

                Child = Box = new Box
                {
                    RelativeSizeAxes = Axes.Both
                }
            });

            if (drawableBullet.HitObject.Shape == Shape.Triangle)
            {
                Add(new EquilateralTriangle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                });
                Box.Alpha = 0;
            }

            if (graphics != GraphicsOptions.HighPerformance)
                circle.EdgeEffect = new EdgeEffectParameters
                {
                    Hollow = drawableBullet.HitObject.Shape == Shape.Circle,
                    Radius = (float)drawableBullet.HitObject.Diameter,
                    Type = EdgeEffectType.Shadow,
                    Colour = drawableBullet.AccentColour.Opacity(graphics != GraphicsOptions.Old ? 0.8f : 0.2f)
                };

            if (graphics == GraphicsOptions.Experimental)
            {
                circle.BorderThickness = 4;
                circle.BorderColour = Color4.White;
                Box.Colour = ColourInfo.GradientVertical(drawableBullet.AccentColour.Darken(0.4f), drawableBullet.AccentColour.Lighten(0.4f));

                circle.EdgeEffect = new EdgeEffectParameters
                {
                    Hollow = drawableBullet.HitObject.Shape == Shape.Circle,
                    Radius = 4f,
                    Type = EdgeEffectType.Shadow,
                    Colour = drawableBullet.AccentColour.Opacity(0.5f)
                };
            }
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            if (graphics == GraphicsOptions.Old)
            {
                if (effectPoint.KiaiMode && bulletKiai.Alpha == 0)
                    bulletKiai.FadeInFromZero(timingPoint.BeatLength / 4);
                if (!effectPoint.KiaiMode && bulletKiai.Alpha == 1)
                    bulletKiai.FadeOutFromOne(timingPoint.BeatLength);
            }
        }

        protected override void Update()
        {
            base.Update();

            if (graphics == GraphicsOptions.Old && bulletKiai.Alpha > 0)
            {
                if (randomRotateDirection)
                    bulletKiai.Rotation += (float)(Clock.ElapsedFrameTime / 1000 * 90) * randomRotationValue;
                else
                    bulletKiai.Rotation += (float)(-Clock.ElapsedFrameTime / 1000 * 90) * randomRotationValue;
            }

            if (ExclusiveTestingHax && circle.Alpha == 1)
            {
                circle.Alpha = 0;
                Add(dean = new SymcolSprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Colour = drawableBullet.AccentColour.Lighten(0.4f),
                    Texture = VitaruRuleset.VitaruTextures.Get("Dean"),
                });
            }
            else if (!ExclusiveTestingHax && circle.Alpha == 0)
            {
                circle.Alpha = 1;
                Remove(dean);
                dean.Delete();
            }

            if (ExclusiveTestingHax)
                dean.Rotation += (float)Clock.ElapsedFrameTime / 2;

            if (graphics == GraphicsOptions.Experimental)
            {
                Vector2 pos = SymcolCursor.VitaruCursor.CenterCircle.ToSpaceOfOtherDrawable(Vector2.Zero, Parent.Parent) + new Vector2(6);
                circle.Rotation = (float)MathHelper.RadiansToDegrees(Math.Atan2(pos.Y - drawableBullet.Position.Y, pos.X - drawableBullet.Position.X));
            }
        }
    }
}
