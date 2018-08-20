using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Characters.Pieces
{
    public class Totem : BeatSyncedContainer
    {
        public readonly DrawableCharacter ParentCharacter;
        private readonly VitaruPlayfield vitaruPlayfield;

        public float StartAngle { get; set; } = 0;

        public Totem(DrawableCharacter vitaruCharacter, VitaruPlayfield playfield)
        {
            ParentCharacter = vitaruCharacter;
            vitaruPlayfield = playfield;
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
            s.MoveTo(ToSpaceOfOtherDrawable(new Vector2(0, 0), s));
        }

        protected override void LoadComplete()
        {
            Masking = true;
            Size = new Vector2(6);
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            BorderThickness = 2;
            BorderColour = ParentCharacter.PrimaryColor;
            CornerRadius = 3;
            Child= new Box
            {
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
