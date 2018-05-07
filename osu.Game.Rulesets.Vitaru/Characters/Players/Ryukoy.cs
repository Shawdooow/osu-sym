﻿using OpenTK.Graphics;
using osu.Framework.Configuration;
using osu.Game.Rulesets.Vitaru.UI;
using System;

namespace osu.Game.Rulesets.Vitaru.Characters.Players
{
    public class Ryukoy : TouhosuPlayer
    {
        #region Fields
        public const double RyukoyHealth = 60;

        public const double RyukoyEnergy = 24;

        public const double RyukoyEnergyCost = 2;

        public const double RyukoyEnergyCostPerSecond = 6;

        public static readonly Color4 RyukoyColor = Color4.MediumPurple;

        public override TouhosuCharacters PlayableCharacter => TouhosuCharacters.RyukoyHakurei;

        public override double MaxHealth => RyukoyHealth;

        public override double MaxEnergy => RyukoyEnergy;

        public override double EnergyCost => RyukoyEnergyCost;

        public override double EnergyCostPerSecond => RyukoyEnergyCostPerSecond;

        public override Color4 PrimaryColor => RyukoyColor;

        private int level = 1;

        private readonly Bindable<int> abstraction;
        #endregion

        public Ryukoy(VitaruPlayfield playfield, Bindable<int> abstraction) : base(playfield)
        {
            this.abstraction = abstraction;
            Abstraction = 3;

            Spell += (input) =>
            {
                abstraction.Value = level;
            };
        }

        protected override void SpellUpdate()
        {
            base.SpellUpdate();

            if (SpellActive)
            {
                Energy -= (Clock.ElapsedFrameTime / 1000) * EnergyCostPerSecond * (level * 0.25f);

                abstraction.Value = level;
            }
            else
                abstraction.Value = 0;
        }

        protected override bool Pressed(VitaruAction action)
        {
            if (action == VitaruAction.Increase)
                level = Math.Min(abstraction.Value + 1, 3);
            if (action == VitaruAction.Decrease)
                level = Math.Max(abstraction.Value - 1, 0);

            return base.Pressed(action);
        }

        #region Touhosu Story Content
        public const string Background = "Being the elder sibling comes with many responsabilitys in the Hakurei family. " +
            "She has the weight of the Hakurei name to uphold as the next inline to be the keeper of their family shrine. " +
            "Her mother would tell her stories about her adventures with her friends to stop the evil fairies from claiming it, and how they always would succeed. " +
            "One day she would like to go on an adventure of her own she would think. \"Becareful what you wish for\" Reimu would tell her. " +
            "Now that she is almost a legal adult she has a very different view however, she is calm and level headed. " +
            "She doesn't actively seek trouble to solve or ways to cause trouble, she simply wishes for peace, quiet and an easy life as the next Hakurei Maiden.";
        #endregion
    }
}