using Symcol.Core.Networking;
using Symcol.Rulesets.Core.Multiplayer.Networking;
using System;

namespace osu.Game.Rulesets.Osu.Multi
{
    [Serializable]
    public class OsuMatchPacket : Packet
    {
        public override int PacketSize => 1024;

        //public string PlayerColor;

        public float MouseX;

        public float MouseY;

        public OsuMatchPacket(RulesetClientInfo rulesetClientInfo) : base(rulesetClientInfo)
        {
        }
    }
}
