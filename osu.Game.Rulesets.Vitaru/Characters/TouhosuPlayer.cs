using osu.Game.Rulesets.Vitaru.UI;
using System;
using System.ComponentModel;

namespace osu.Game.Rulesets.Vitaru.Characters
{
    public abstract class TouhosuPlayer : Player
    {
        public const double DefaultEnergy = 36;

        public const double DefaultEnergyCost = 4;

        public const double DefaultEnergyCostPerSecond = 0;

        public virtual double MaxEnergy { get; } = DefaultEnergy;

        public virtual double EnergyCost { get; } = DefaultEnergyCost;

        public virtual double EnergyCostPerSecond { get; } = DefaultEnergyCostPerSecond;

        public double Energy { get; protected set; }

        public Action<VitaruAction> Spell;

        protected bool SpellActive { get; set; }

        protected double SpellDeActivateTime { get; set; } = double.MinValue;

        protected double SpellEndTime { get; set; } = double.MinValue;

        public abstract TouhosuCharacters PlayableCharacter { get; }

        protected override string CharacterName => PlayableCharacter.ToString();

        public TouhosuPlayer(VitaruPlayfield playfield) : base(playfield)
        {
        }

        protected override void Update()
        {
            base.Update();

            SpellUpdate();
        }

        #region Spell Handling
        protected virtual bool SpellActivate(VitaruAction action)
        {
            if (Energy >= EnergyCost && !SpellActive)
            {
                SpellActive = true;
                Energy -= EnergyCost;
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
                Energy = Math.Min((float)Clock.ElapsedFrameTime / 500 + Energy, MaxEnergy);

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

    public enum TouhosuCharacters
    {
        //The Hakurei Family, or whats left of them
        [Description("Reimu Hakurei")]
        ReimuHakurei,
        [Description("Ryukoy Hakurei")]
        RyukoyHakurei,
        [Description("Tomaji Hakurei")]
        TomajiHakurei,

        //Hakurei Family Friends, 
        [Description("Sakuya Izayoi")]
        SakuyaIzayoi,
        //[System.ComponentModel.Description("Flandre Scarlet")]
        //FlandreScarlet,
        //[System.ComponentModel.Description("Remilia Scarlet")]
        //RemiliaScarlet,

        //Uncle Vaster and Aunty Alice
        //[System.ComponentModel.Description("Alice Letrunce")]
        //AliceLetrunce,
        //[System.ComponentModel.Description("Vaster Letrunce")]
        //VasterLetrunce,

        //[System.ComponentModel.Description("Marisa Kirisame")]
        //MarisaKirisame,
    }
}
