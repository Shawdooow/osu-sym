﻿using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using OpenTK;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Hakurei.Drawables
{
    public class DrawableTomaji : DrawableTouhosuPlayer
    {
        /// <summary>
        /// scale from 0 - 1 on how charged our blink is
        /// </summary>
        private double charge;

        public DrawableTomaji(VitaruPlayfield playfield) : base(playfield, new Tomaji())
        {
        }

        protected override void SpellUpdate()
        {
            base.SpellUpdate();

            if (SpellActive)
            {
                double fullChargeTime = SpellStartTime + Tomaji.CHARGE_TIME;

                charge = Math.Min(1 - ((fullChargeTime - Time.Current) / Tomaji.CHARGE_TIME), 1);

                Energy -= (Clock.ElapsedFrameTime / 1000) * TouhosuPlayer.EnergyDrainRate * charge;
            }
            else if (charge > 0)
            {
                double cursorAngle = (MathHelper.RadiansToDegrees(Math.Atan2((Cursor.Position.Y - Position.Y), (Cursor.Position.X - Position.X))) + Rotation) - 12;
                double x = Position.X + (charge * Tomaji.BLINK_DISTANCE) * Math.Cos(MathHelper.DegreesToRadians(cursorAngle));
                double y = Position.Y + (charge * Tomaji.BLINK_DISTANCE) * Math.Sin(MathHelper.DegreesToRadians(cursorAngle));

                Hitbox.HitDetection = false;
                SpellEndTime = Time.Current + 200 * charge;
                Alpha = 0.25f;

                this.MoveTo(new Vector2((float)x, (float)y), 200 * charge, Easing.OutSine)
                    .FadeIn(200 * charge, Easing.InCubic);

                charge = 0;
            }

            if (Time.Current >= SpellEndTime)
                Hitbox.HitDetection = true;
        }
    }
}
