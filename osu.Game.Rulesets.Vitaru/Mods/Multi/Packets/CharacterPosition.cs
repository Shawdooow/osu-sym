using System;
using Symcol.Networking.Packets;

namespace osu.Game.Rulesets.Vitaru.Mods.Multi.Packets
{
    [Serializable]
    public class CharacterPosition : Packet
    {
        public float X;

        public float Y;

        public float Z;
    }
}
