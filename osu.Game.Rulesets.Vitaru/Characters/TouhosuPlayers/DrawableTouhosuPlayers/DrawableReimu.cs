using OpenTK;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.DrawableTouhosuPlayers
{
    public class DrawableReimu : DrawableTouhosuPlayer
    {
        private const double leader_max = 1.5d;
        private const double leader_min = 0.75d;
        private const double leader_max_range = 64;
        private const double leader_min_range = 128;

        private List<DrawableTouhosuPlayer> leaderedPlayers = new List<DrawableTouhosuPlayer>();

        public DrawableReimu(VitaruPlayfield playfield, VitaruNetworkingClientHandler vitaruNetworkingClientHandler) : base(playfield, new Reimu(), vitaruNetworkingClientHandler)
        {
        }

        protected override void SpellUpdate()
        {
            base.SpellUpdate();

            if (SpellActive)
            {
                foreach (Drawable drawable in VitaruPlayfield.GameField.Current)
                    if (drawable is DrawableTouhosuPlayer drawableTouhosuPlayer)
                    {
                        Vector2 object2Pos = drawableTouhosuPlayer.ToSpaceOfOtherDrawable(Vector2.Zero, this) + new Vector2(6);
                        double distance = Math.Sqrt(Math.Pow(object2Pos.X, 2) + Math.Pow(object2Pos.Y, 2));

                        if (distance <= leader_min_range)
                        {
                            drawableTouhosuPlayer.HealingMultiplier = getLeaderDistanceMultiplier(distance);
                            drawableTouhosuPlayer.EnergyGainMultiplier = getLeaderDistanceMultiplier(distance);
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
            double scale = (leader_max - leader_min) / (leader_max_range - leader_min_range);
            return leader_min + ((value - leader_min_range) * scale);
        }
    }
}
