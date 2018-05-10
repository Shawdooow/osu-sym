using System;

namespace osu.Game.Rulesets.Vitaru.Multi
{
    [Serializable]
    public class VitaruPlayerInformation
    {
        public string PlayerID = "0";

        public string Character;

        public float PlayerX;

        public float PlayerY;

        public float MouseX;

        public float MouseY;

        public float ClockSpeed;

        public VitaruAction PressedAction;

        public VitaruAction ReleasedAction;
    }
}
