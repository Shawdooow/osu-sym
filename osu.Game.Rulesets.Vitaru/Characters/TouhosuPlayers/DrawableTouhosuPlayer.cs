using System;
using osu.Framework.Allocation;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Hakurei.Drawables;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Inlaws.Drawables;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Scarlet.Drawables;
using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers.DrawableVitaruPlayers;
using osu.Game.Rulesets.Vitaru.Debug;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers
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

        protected bool EnergyHacks { get; private set; }

        //reset after healing is done
        public double EnergyGainMultiplier = 1;

        public DrawableTouhosuPlayer(VitaruPlayfield playfield, TouhosuPlayer player, VitaruNetworkingClientHandler vitaruNetworkingClientHandler) : base(playfield, player, vitaruNetworkingClientHandler)
        {
            TouhosuPlayer = player;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (!Puppet)
                DebugToolkit.GeneralDebugItems.Add(new DebugAction(() => { EnergyHacks = !EnergyHacks; }) { Text = "Energy Hacks" });
        }

        protected override void OnQuarterBeat()
        {
            base.OnQuarterBeat();

            EnergyGainMultiplier = 1;
        }

        protected override void Update()
        {
            base.Update();

            if (EnergyHacks)
                Energy = TouhosuPlayer.MaxEnergy;

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
            {
                double fallOff = 1;

                for (int i = 0; i < HealingBullets.Count - 1; i++)
                    fallOff *= HEALING_FALL_OFF;

                foreach (HealingBullet HealingBullet in HealingBullets)
                    Energy = Math.Min(((Clock.ElapsedFrameTime / 500) * (GetBulletHealingMultiplier(HealingBullet.EdgeDistance) * fallOff)) + Energy, TouhosuPlayer.MaxEnergy);
            }

            if (Energy <= 0)
            {
                Energy = 0;
                SpellActive = false;
            }
        }
        #endregion

        protected override void Pressed(VitaruAction action)
        {
            base.Pressed(action);

            if (action == VitaruAction.Spell)
                SpellActivate(action);
        }

        protected override void Released(VitaruAction action)
        {
            base.Released(action);

            if (action == VitaruAction.Spell)
                SpellDeactivate(action);
        }

        public static DrawableTouhosuPlayer GetDrawableTouhosuPlayer(VitaruPlayfield playfield, string name, VitaruNetworkingClientHandler vitaruNetworkingClientHandler)
        {
            switch (name)
            {
                default:
                    return new DrawableTouhosuPlayer(playfield, TouhosuPlayer.GetTouhosuPlayer(name), vitaruNetworkingClientHandler);

                case "ReimuHakurei":
                    return new DrawableReimu(playfield, vitaruNetworkingClientHandler);
                case "RyukoyHakurei":
                    return new DrawableRyukoy(playfield, vitaruNetworkingClientHandler);
                case "TomajiHakurei":
                    return new DrawableTomaji(playfield, vitaruNetworkingClientHandler);

                case "SakuyaIzayoi":
                    return new DrawableSakuya(playfield, vitaruNetworkingClientHandler);
                case "RemiliaScarlet":
                    return new DrawableTouhosuPlayer(playfield, TouhosuPlayer.GetTouhosuPlayer(name), vitaruNetworkingClientHandler);
                case "FlandreScarlet":
                    return new DrawableTouhosuPlayer(playfield, TouhosuPlayer.GetTouhosuPlayer(name), vitaruNetworkingClientHandler);

                case "AliceLetrunce":
                    return new DrawableAlice(playfield, vitaruNetworkingClientHandler);
                case "VasterLetrunce":
                    return new DrawableTouhosuPlayer(playfield, TouhosuPlayer.GetTouhosuPlayer(name), vitaruNetworkingClientHandler);

                case "MarisaKirisame":
                    return new DrawableTouhosuPlayer(playfield, TouhosuPlayer.GetTouhosuPlayer(name), vitaruNetworkingClientHandler);
            }
        }
    }
}
