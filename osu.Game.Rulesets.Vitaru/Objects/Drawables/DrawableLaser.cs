using OpenTK;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Vitaru.Judgements;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using Symcol.Core.GameObjects;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableLaser : DrawableVitaruHitObject
    {
        private VitaruGamemode currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<VitaruGamemode>(VitaruSetting.GameMode);

        //Set to "true" when a judgement should be returned
        private bool returnJudgement;

        private bool returnedJudgement;

        public bool ReturnGreat = false;

        //Can be set for the Graze ScoringMetric
        public int ScoreZone;

        //Should be set to true when a character is hit
        public bool Hit;

        //Incase we want to be deleted in the near future
        public double LaserDeleteTime = -1;        

        public SymcolHitbox Hitbox;
        private LaserPiece laserPiece;

        private readonly DrawablePattern drawablePattern;
        public readonly Laser Laser;

        private const float fade_in_time = 200;
        private const float fade_out_time = 200;

        public DrawableLaser(Laser laser, DrawablePattern drawablePattern, VitaruPlayfield playfield) : base(laser, playfield)
        {
            AlwaysPresent = true;
            Alpha = 0;

            Anchor = Anchor.TopLeft;
            Origin = Anchor.BottomCentre;

            Laser = laser;
            this.drawablePattern = drawablePattern;

            Size = new Vector2(Laser.LaserSize.X / 2, Laser.LaserSize.Y / 8);
            Rotation = MathHelper.RadiansToDegrees(Laser.LaserAngleRadian);
        }

        public DrawableLaser(Laser laser, VitaruPlayfield playfield) : base(laser, playfield)
        {
            AlwaysPresent = true;
            Alpha = 0;

            Anchor = Anchor.TopLeft;
            Origin = Anchor.BottomCentre;

            Laser = laser;

            Size = new Vector2(Laser.LaserSize.X / 2, Laser.LaserSize.Y / 8);
            Rotation = MathHelper.RadiansToDegrees(Laser.LaserAngleRadian);
        }

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            base.CheckForJudgements(userTriggered, timeOffset);

            if (returnJudgement)
            {
                switch (ScoreZone)
                {
                    case 0:
                        AddJudgement(new VitaruJudgement { Result = HitResult.Miss });
                        break;
                    case 50:
                        AddJudgement(new VitaruJudgement { Result = HitResult.Meh });
                        break;
                    case 100:
                        AddJudgement(new VitaruJudgement { Result = HitResult.Good });
                        break;
                    case 300:
                        AddJudgement(new VitaruJudgement { Result = HitResult.Great });
                        break;
                }
            }

            else if (Hit && !returnedJudgement)
            {
                if (!Laser.DummyMode)
                    AddJudgement(new VitaruJudgement { Result = HitResult.Miss });
                returnedJudgement = true;
            }

            else if (ReturnGreat)
            {
                AddJudgement(new VitaruJudgement { Result = HitResult.Great });
            }
        }
    }
}
