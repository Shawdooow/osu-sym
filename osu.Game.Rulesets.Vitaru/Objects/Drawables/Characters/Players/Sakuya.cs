using OpenTK.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using System;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters.Players
{
    public class Sakuya : Player
    {
        public double SetRate { get; private set; } = 0.8d;

        public override PlayableCharacters PlayableCharacter => PlayableCharacters.SakuyaIzayoi;

        public override double MaxHealth => 80;

        protected override Color4 CharacterColor => Color4.Navy;

        public Sakuya(VitaruPlayfield playfield) : base(playfield)
        {

        }

        protected override bool Pressed(VitaruAction action)
        {
            bool late = true;

            if (action == VitaruAction.Increase && !late)
                SetRate = Math.Min((float)Math.Round(SetRate + 0.2f, 1), 0.8f);
            else if (action == VitaruAction.Increase && late)
                SetRate = Math.Min((float)Math.Round(SetRate + 0.2f, 1), 1.2f);
            if (action == VitaruAction.Decrease && !late)
                SetRate = Math.Max((float)Math.Round(SetRate - 0.2f, 1), 0.4f);
            else if (action == VitaruAction.Decrease && late)
                SetRate = Math.Max((float)Math.Round(SetRate - 0.2f, 1), 0.2f);

            return base.Pressed(action);
        }
    }
}
