﻿using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers.DrawableVitaruPlayers;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.DrawableTouhosuPlayers
{
    public class DrawableTouhosuPlayer : DrawableVitaruPlayer
    {
        public readonly TouhosuPlayer TouhosuPlayer;

        public double Energy { get; protected set; }

        public Action<VitaruAction> Spell;

        protected bool SpellActive { get; set; }

        protected double SpellStartTime { get; set; } = double.MaxValue;

        protected double SpellDeActivateTime { get; set; } = double.MinValue;

        protected double SpellEndTime { get; set; } = double.MinValue;

        public DrawableTouhosuPlayer(VitaruPlayfield playfield, TouhosuPlayer player, VitaruNetworkingClientHandler vitaruNetworkingClientHandler) : base(playfield, player, vitaruNetworkingClientHandler)
        {
            TouhosuPlayer = player;
        }

        protected override void Update()
        {
            base.Update();
            SpellUpdate();
        }

        #region Spell Handling
        protected virtual bool SpellActivate(VitaruAction action)
        {
            if (Energy >= TouhosuPlayer.EnergyCost && !SpellActive)
            {
                if (TouhosuPlayer.EnergyDrainRate == 0)
                    Energy -= TouhosuPlayer.EnergyCost;

                SpellActive = true;
                SpellStartTime = Time.Current;
                Spell?.Invoke(action);
                return true;
            }
            else
                return false;
        }

        protected virtual void SpellDeactivate(VitaruAction action)
        {
            SpellActive = false;
        }

        protected virtual void SpellUpdate()
        {
            if (HealingBullets.Count > 0)
                foreach (KeyValuePair<DrawableBullet, double> HealingBullet in HealingBullets)
                    Energy = Math.Min((Clock.ElapsedFrameTime / 500) * GetBulletHealingMultiplier(HealingBullet.Value), TouhosuPlayer.MaxEnergy);

            if (Energy <= 0)
            {
                Energy = 0;
                SpellActive = false;
            }
        }
        #endregion

        protected override bool Pressed(VitaruAction action)
        {
            if (action == VitaruAction.Spell)
                SpellActivate(action);

            return base.Pressed(action);
        }

        protected override bool Released(VitaruAction action)
        {
            if (action == VitaruAction.Spell)
                SpellDeactivate(action);

            return base.Released(action);
        }
    }
}
