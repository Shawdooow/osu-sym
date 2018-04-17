using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters;
using System;

namespace osu.Game.Rulesets.Vitaru.Multi
{
    [Serializable]
    public class VitaruPlayerInformation
    {
        public string PlayerID = "0";

        public VitaruPlayer Character;

        public float PlayerX;

        public float PlayerY;

        public float MouseX;

        public float MouseY;

        public float ClockSpeed;

        public VitaruAction PressedAction;

        public VitaruAction ReleasedAction;
    }
}
