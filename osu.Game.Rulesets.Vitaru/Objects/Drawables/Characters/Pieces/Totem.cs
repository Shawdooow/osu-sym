using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters.Pieces
{
    public class Totem : BeatSyncedContainer
    {
        public readonly Character ParentCharacter;
        private readonly VitaruPlayfield vitaruPlayfield;

        public float StartAngle { get; set; } = 0;

        public Totem(Character vitaruCharacter, VitaruPlayfield playfield)
        {
            ParentCharacter = vitaruCharacter;
            vitaruPlayfield = playfield;
        }

        public void Shoot()
        {
            DrawableSeekingBullet s;
            vitaruPlayfield.BulletField.Add(s = new DrawableSeekingBullet(new SeekingBullet
            {
                Team = ParentCharacter.Team,
                BulletSpeed = 0.8f,
                BulletDamage = 5,
                ColorOverride = ParentCharacter.CharacterColor,
                StartAngle = StartAngle,
            }, vitaruPlayfield));
            s.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), s));
        }

        protected override void LoadComplete()
        {
            Masking = true;
            Size = new Vector2(6);
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            BorderThickness = 2;
            BorderColour = ParentCharacter.CharacterColor;
            CornerRadius = 3;
            Child= new Box
            {
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
