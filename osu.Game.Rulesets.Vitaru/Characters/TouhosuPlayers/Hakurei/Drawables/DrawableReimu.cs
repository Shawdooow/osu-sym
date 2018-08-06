using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using OpenTK;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Hakurei.Drawables
{
    public class DrawableReimu : DrawableTouhosuPlayer
    {
        //private readonly List<DrawableTouhosuPlayer> leaderedPlayers = new List<DrawableTouhosuPlayer>();

        public DrawableReimu(VitaruPlayfield playfield) : base(playfield, new Reimu())
        {
        }

        protected override void SpellUpdate()
        {
            base.SpellUpdate();

            if (SpellActive)
            {
                foreach (Drawable drawable in CurrentPlayfield)
                    if (drawable is DrawableTouhosuPlayer drawableTouhosuPlayer && drawableTouhosuPlayer.Team == Team)
                    {
                        Vector2 object2Pos = drawableTouhosuPlayer.ToSpaceOfOtherDrawable(Vector2.Zero, this) + new Vector2(6);
                        double distance = Math.Sqrt(Math.Pow(object2Pos.X, 2) + Math.Pow(object2Pos.Y, 2));

                        if (distance <= Reimu.LEADER_CLOSEST_RANGE)
                        {
                            drawableTouhosuPlayer.HealingMultiplier = getLeaderDistanceMultiplier(distance);
                            drawableTouhosuPlayer.EnergyGainMultiplier = getLeaderDistanceMultiplier(distance);
                            Energy -= (Clock.ElapsedFrameTime / 1000) * TouhosuPlayer.EnergyDrainRate;
                        }
                        else
                        {
                            drawableTouhosuPlayer.HealingMultiplier = 1;
                            drawableTouhosuPlayer.EnergyGainMultiplier = 1;
                        }
                    }
            }
        }

        private double getLeaderDistanceMultiplier(double value)
        {
            double scale = (Reimu.LEADER_MAX_BUFF - Reimu.LEADER_MIN_BUFF) / (Reimu.LEADER_FARTHEST_RANGE - Reimu.LEADER_CLOSEST_RANGE);
            return Reimu.LEADER_MIN_BUFF + ((value - Reimu.LEADER_CLOSEST_RANGE) * scale);
        }
    }
}
