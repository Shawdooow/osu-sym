using System;
using Symcol.Core.LegacyNetworking;

namespace osu.Game.Rulesets.Vitaru.OldMulti
{
    [Serializable]
    public class VitaruInMatchPacket : Packet
    {
        /// <summary>
        /// This player's information
        /// </summary>
        public VitaruPlayerInformation PlayerInformation;

        public override int PacketSize => 2048;

        public VitaruInMatchPacket(ClientInfo clientInfo) : base(clientInfo)
        {

        }
    }
}
