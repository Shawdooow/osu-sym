﻿using OpenTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers
{
    public class Ryukoy : TouhosuPlayer
    {
        public override string Name => "Ryukoy Hakurei";

        public override double MaxHealth => 80;

        public override double MaxEnergy => 24;

        public override double EnergyCost => 2;

        public override double EnergyCostPerSecond => 6;

        public override Color4 PrimaryColor => base.PrimaryColor;

        public override Color4 SecondaryColor => base.SecondaryColor;

        public override Color4 ComplementaryColor => base.ComplementaryColor;

        public override string Background => "Being the elder sibling comes with many responsabilitys in the Hakurei family. " +
            "She has the weight of the Hakurei name to uphold as the next inline to be the keeper of their family shrine. " +
            "Her mother would tell her stories about her adventures with her friends to stop the evil fairies from claiming it, and how they always would succeed. " +
            "One day she would like to go on an adventure of her own she would think. \"Becareful what you wish for\" Reimu would tell her. " +
            "Now that she is almost a legal adult she has a very different view however, she is calm and level headed. " +
            "She doesn't actively seek trouble to solve or ways to cause trouble, she simply wishes for peace, quiet and an easy life as the next Hakurei Maiden.";
    }
}
