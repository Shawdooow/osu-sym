using System;
using System.Collections.Generic;
using System.Text;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters.Players
{
    public class Shaka : VitaruPlayer
    {
        public override SelectableCharacters PlayableCharacter => SelectableCharacters.ShakaZulu;

        public Shaka(VitaruPlayfield playfield) : base(playfield)
        {
        }
    }
}
