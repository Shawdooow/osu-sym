#region usings

using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Abilities;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;
using osuTK;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Characters.Pieces
{
    public class Totem : BeatSyncedContainer, ITuneable
    {
        public AspectLockedPlayfield CurrentPlayfield { get; set; }

        public float StartAngle { get; set; } = 0;

        public virtual bool Untuned { get; set; }

        public readonly DrawableCharacter ParentCharacter;

        private readonly VitaruPlayfield vitaruPlayfield;

        public Totem(DrawableCharacter vitaruCharacter, VitaruPlayfield playfield)
        {
            ParentCharacter = vitaruCharacter;
            vitaruPlayfield = playfield;
            CurrentPlayfield = playfield.Gamefield;

            AlwaysPresent = true;
            Masking = true;

            Alpha = 0;

            Size = new Vector2(12);
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            BorderThickness = 3;
            BorderColour = ParentCharacter.PrimaryColor;
            CornerRadius = 4;
            Child = new Box
            {
                RelativeSizeAxes = Axes.Both
            };
        }

        public void Shoot()
        {
            DrawableSeekingBullet s;
            vitaruPlayfield.Gamefield.Add(s = new DrawableSeekingBullet(new SeekingBullet
            {
                Team = ParentCharacter.Team,
                Speed = 0.8f,
                Damage = 5,
                ColorOverride = ParentCharacter.PrimaryColor,
                StartAngle = StartAngle,
            }, vitaruPlayfield));
            s.MoveTo(ToSpaceOfOtherDrawable(Vector2.Zero, s));
            s.Untuned = Untuned;
        }
    }
}
