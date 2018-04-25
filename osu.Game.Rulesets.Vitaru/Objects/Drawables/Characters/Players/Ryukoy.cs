using OpenTK.Graphics;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters.Players
{
    public class Ryukoy : VitaruPlayer
    {
        #region Fields
        public const double RyukoyHealth = 60;

        public const double RyukoyEnergy = 24;

        public const double RyukoyEnergyCost = 12;

        public const double RyukoyEnergyCostPerSecond = 0;

        public static readonly Color4 RyukoyColor = Color4.MediumPurple;

        public override SelectableCharacters PlayableCharacter => SelectableCharacters.RyukoyHakurei;

        public override double MaxHealth => RyukoyHealth;

        public override double MaxEnergy => RyukoyEnergy;

        public override double EnergyCost => RyukoyEnergyCost;

        public override double EnergyCostPerSecond => RyukoyEnergyCostPerSecond;

        public override Color4 PrimaryColor => RyukoyColor;
        #endregion

        public Ryukoy(VitaruPlayfield playfield) : base(playfield)
        {
            Spell += (action) =>
            {

            };
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
