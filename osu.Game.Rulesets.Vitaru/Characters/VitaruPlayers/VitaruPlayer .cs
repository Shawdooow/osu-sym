using OpenTK.Graphics;
using System.ComponentModel;

namespace osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers
{
    public class VitaruPlayer
    {
        public virtual VitaruCharacters Character => VitaruCharacters.Alex;

        public virtual double MaxHealth => 80;

        public virtual Color4 PrimaryColor { get; } = Color4.Green;

        public virtual Color4 SecondaryColor { get; } = Color4.LightBlue;

        public virtual Color4 ComplementaryColor { get; } = Color4.LightGreen;
    }

    //TODO: This shouldn't be neccesary
    public enum VitaruCharacters
    {
        [Description("Alex")]
        Alex,
    }
}
