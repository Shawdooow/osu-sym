using osu.Game.Rulesets.Vitaru.UI;
using System.ComponentModel;

namespace osu.Game.Rulesets.Vitaru.Characters
{
    public abstract class VitaruPlayer : Character
    {
        public abstract VitaruCharacters PlayableCharacter { get; }

        protected override string CharacterName => PlayableCharacter.ToString();

        public VitaruPlayer(VitaruPlayfield playfield) : base(playfield)
        {

        }
    }

    public enum VitaruCharacters
    {
        [Description("Alex")]
        Alex,
    }
}
