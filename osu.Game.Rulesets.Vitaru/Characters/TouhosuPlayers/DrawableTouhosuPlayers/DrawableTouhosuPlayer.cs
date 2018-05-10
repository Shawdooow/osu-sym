using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers.DrawableVitaruPlayers;
using osu.Game.Rulesets.Vitaru.UI;
using System;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.DrawableTouhosuPlayers
{
    public abstract class DrawableTouhosuPlayer : DrawableVitaruPlayer
    {
        public readonly TouhosuPlayer TouhosuPlayer;

        protected override string CharacterName => TouhosuPlayer.Name;

        public double Energy { get; protected set; }

        public Action<VitaruAction> Spell;

        protected bool SpellActive { get; set; }

        protected double SpellDeActivateTime { get; set; } = double.MinValue;

        protected double SpellEndTime { get; set; } = double.MinValue;

        public DrawableTouhosuPlayer(VitaruPlayfield playfield, TouhosuPlayer player) : base(playfield, player)
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
                SpellActive = true;
                Energy -= TouhosuPlayer.EnergyCost;
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
            if (CanHeal)
                Energy = Math.Min((float)Clock.ElapsedFrameTime / 500 + Energy, TouhosuPlayer.MaxEnergy);

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
