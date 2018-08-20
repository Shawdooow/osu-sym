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
        private Gamemodes currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.Gamemode);

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

        public readonly Laser Laser;

        private const float fade_in_time = 200;
        private const float fade_out_time = 200;

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

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            base.CheckForResult(userTriggered, timeOffset);

            if (returnJudgement)
            {
                switch (ScoreZone)
                {
                    case 0:
                        ApplyResult(r => r.Type = HitResult.Miss);
                        break;
                    case 50:
                        ApplyResult(r => r.Type = HitResult.Meh);
                        break;
                    case 100:
                        ApplyResult(r => r.Type = HitResult.Good);
                        break;
                    case 300:
                        ApplyResult(r => r.Type = HitResult.Great);
                        break;
                }
            }

            else if (Hit && !returnedJudgement)
            {
                if (!Laser.DummyMode)
                    ApplyResult(r => r.Type = HitResult.Miss);
                returnedJudgement = true;
            }

            else if (ReturnGreat)
            {
                ApplyResult(r => r.Type = HitResult.Great);
            }
        }
    }
}
