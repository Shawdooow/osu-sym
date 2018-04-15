using OpenTK;
using OpenTK.Input;
using osu.Framework.Input.Bindings;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters
{
    public abstract class Player : Character, IKeyBindingHandler<VitaruAction>
    {
        #region Fields
        protected readonly VitaruGamemode CurrentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<VitaruGamemode>(VitaruSetting.GameMode);

        public abstract PlayableCharacters PlayableCharacter { get; }

        protected override string CharacterName => PlayableCharacter.ToString();

        public virtual double MaxEnergy { get; protected set; } = 36;

        public virtual double EnergyCost { get; protected set; } = 12;

        public virtual double EnergyCostPerSecond { get; protected set; }

        public double Energy { get; protected set; }

        public Action<VitaruAction> Spell;

        public Dictionary<VitaruAction, bool> Actions = new Dictionary<VitaruAction, bool>();

        //(MinX,MaxX,MinY,MaxY)
        protected Vector4 PlayerBounds
        {
            get
            {
                if (CurrentGameMode == VitaruGamemode.Touhosu)
                    return new Vector4(0, 512, 0, 820);
                else
                    return new Vector4(0, 512, 0, 820);
            }
        }

        protected bool SpellActive { get; set; }
        #endregion

        public Player(VitaruPlayfield playfield) : base(playfield)
        {
            Actions[VitaruAction.Up] = false;
            Actions[VitaruAction.Down] = false;
            Actions[VitaruAction.Left] = false;
            Actions[VitaruAction.Right] = false;
            Actions[VitaruAction.Slow] = false;
            Actions[VitaruAction.Shoot] = false;
        }

        protected override void Update()
        {
            base.Update();

            Position = GetNewPlayerPosition(0.25d);
        }

        protected virtual Vector2 GetNewPlayerPosition(double playerSpeed)
        {
            double yTranslationDistance = playerSpeed * Clock.ElapsedFrameTime;
            double xTranslationDistance = playerSpeed * Clock.ElapsedFrameTime;
            Vector2 playerPosition = Position;

            if (Actions[VitaruAction.Slow])
            {
                xTranslationDistance /= 2;
                yTranslationDistance /= 2;
            }
            if (false)//Actions[VitaruAction.Fast])
            {
                xTranslationDistance *= 2;
                yTranslationDistance *= 2;
            }

            if (Actions[VitaruAction.Up])
                playerPosition.Y -= (float)yTranslationDistance;
            if (Actions[VitaruAction.Left])
                playerPosition.X -= (float)xTranslationDistance;
            if (Actions[VitaruAction.Down])
                playerPosition.Y += (float)yTranslationDistance;
            if (Actions[VitaruAction.Right])
                playerPosition.X += (float)xTranslationDistance;

            playerPosition = Vector2.ComponentMin(playerPosition, PlayerBounds.Yw);
            playerPosition = Vector2.ComponentMax(playerPosition, PlayerBounds.Xz);

            return playerPosition;
        }

        #region Spell Handling
        protected virtual bool SpellActivate(VitaruAction action)
        {
            if (Energy <= EnergyCost)
            {
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

        }
        #endregion

        #region Input Handling
        public override bool ReceiveMouseInputAt(Vector2 screenSpacePos) => true;

        public bool OnPressed(VitaruAction action)
        {
            if (true)//!Bot && !Puppet)
                return Pressed(action);
            else
                return false;
        }

        protected virtual bool Pressed(VitaruAction action)
        {
            //Keyboard Stuff
            if (action == VitaruAction.Up)
                Actions[VitaruAction.Up] = true;
            if (action == VitaruAction.Down)
                Actions[VitaruAction.Down] = true;
            if (action == VitaruAction.Left)
                Actions[VitaruAction.Left] = true;
            if (action == VitaruAction.Right)
                Actions[VitaruAction.Right] = true;
            if (action == VitaruAction.Slow)
                Actions[VitaruAction.Slow] = true;

            //Mouse Stuff
            if (action == VitaruAction.Shoot)
                Actions[VitaruAction.Shoot] = true;

            SpellActivate(action);

            //if (!Puppet)
                //sendPacket(action);

            return true;
        }

        public bool OnReleased(VitaruAction action)
        {
            if (true)//!Bot && !Puppet)
                return Released(action);
            else
                return false;
        }

        protected virtual bool Released(VitaruAction action)
        {
            //Keyboard Stuff
            if (action == VitaruAction.Up)
                Actions[VitaruAction.Up] = false;
            if (action == VitaruAction.Down)
                Actions[VitaruAction.Down] = false;
            if (action == VitaruAction.Left)
                Actions[VitaruAction.Left] = false;
            if (action == VitaruAction.Right)
                Actions[VitaruAction.Right] = false;
            if (action == VitaruAction.Slow)
                Actions[VitaruAction.Slow] = false;

            //Mouse Stuff
            if (action == VitaruAction.Shoot)
                Actions[VitaruAction.Shoot] = false;
            SpellDeactivate(action);

            //if (!Puppet)
                //sendPacket(VitaruAction.None, action);

            return true;
        }
        #endregion
    }

    public enum PlayableCharacters
    {
        /*
        [System.ComponentModel.Description("Reimu Hakurei")]
        ReimuHakurei = 1,
        [System.ComponentModel.Description("Marisa Kirisame")]
        MarisaKirisame,
        */
        [System.ComponentModel.Description("Sakuya Izayoi")]
        SakuyaIzayoi = 3,
        /*
        [System.ComponentModel.Description("Flandre Scarlet")]
        FlandreScarlet,
        [System.ComponentModel.Description("Remilia Scarlet")]
        RemiliaScarlet,
        [System.ComponentModel.Description("Rinnosuke Morichika")]
        RinnosukeMorichika,
        [System.ComponentModel.Description("Cirno")]
        Cirno,
        //[System.ComponentModel.Description("Tenshi Hinanai")]
        //TenshiHinanai,
        [System.ComponentModel.Description("Yuyuko Saigyouji")]
        YuyukoSaigyouji,
        [System.ComponentModel.Description("Yukari Yakumo")]
        YukariYakumo,
        //[System.ComponentModel.Description("Ran Yakumo")]
        //RanYakumo,
        //[System.ComponentModel.Description("Chen")]
        //Chen,
        [System.ComponentModel.Description("Alice Letrunce")]
        AliceLetrunce,
        [System.ComponentModel.Description("Vaster Letrunce")]
        VasterLetrunce,
        //[System.ComponentModel.Description("Komachi Onozuka")]
        //KomachiOnozuka,
        [System.ComponentModel.Description("Byakuren Hijiri")]
        ByakurenHijiri,
        //[System.ComponentModel.Description("Rumia")]
        //Rumia,
        //[System.ComponentModel.Description("Sikieiki Yamaxanadu")]
        //SikieikiYamaxanadu,
        //[System.ComponentModel.Description("Suwako Moriya")]
        //SuwakoMoriya,
        //[System.ComponentModel.Description("Youmu Konpaku")]
        //YoumuKonpaku,
        [System.ComponentModel.Description("Kokoro Hatano")]
        KokoroHatano,
        [System.ComponentModel.Description("Kaguya")]
        Kaguya,
        [System.ComponentModel.Description("Ibaraki Kasen")]
        IbarakiKasen,
        [System.ComponentModel.Description("Nue Houjuu")]
        NueHoujuu,
        [System.ComponentModel.Description("Alice Muyart")]
        AliceMuyart,
        [System.ComponentModel.Description("Arysa Muyart")]
        ArysaMuyart
        */
    }
}
