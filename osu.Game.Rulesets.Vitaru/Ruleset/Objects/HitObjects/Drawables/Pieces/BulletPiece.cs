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

        private readonly Color4 accent;

        public BulletPiece(Color4 accent, float diameter, Shape shape)
        {
            this.accent = accent;

            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            randomRotationValue = (float)RNG.Next(10, 15) / 10;
            randomRotateDirection = RNG.NextBool();

            if (graphics == GraphicsOptions.Old)
                Child = bulletKiai = new Sprite
                {
                    Scale = new Vector2(2),
                    Alpha = 0,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Colour = accent,
                    Texture = VitaruRuleset.VitaruTextures.Get("bulletKiai"),
                };

            Add(circle = new CircularContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Alpha = 1,
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(graphics != GraphicsOptions.Old && graphics != GraphicsOptions.Experimental ? 1 : 1.05f),
                BorderColour = accent,
                BorderThickness = graphics != GraphicsOptions.Old ? 0 : 6,
                Masking = true,

                Child = Box = new Box
                {
                    RelativeSizeAxes = Axes.Both
                }
            });

            if (shape == Shape.Triangle)
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
                    Hollow = shape == Shape.Circle,
                    Radius = diameter,
                    Type = EdgeEffectType.Shadow,
                    Colour = accent.Opacity(graphics != GraphicsOptions.Old ? 0.8f : 0.2f)
                };

            if (graphics == GraphicsOptions.Experimental)
            {
                circle.BorderThickness = 4;
                circle.BorderColour = Color4.White;
                Box.Colour = ColourInfo.GradientVertical(accent.Darken(0.4f), accent.Lighten(0.4f));

                circle.EdgeEffect = new EdgeEffectParameters
                {
                    Hollow = shape == Shape.Circle,
                    Radius = 4f,
                    Type = EdgeEffectType.Shadow,
                    Colour = accent.Opacity(0.5f)
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
                    Colour = accent.Lighten(0.4f),
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
        }
    }
}
