using System;
using Symcol.Core.LegacyNetworking;

namespace Symcol.Rulesets.Core.LegacyMultiplayer.Networking
{
    [Serializable]
    public class ScorePacket : Packet
    {
        public override int PacketSize => 2048;

        public int Score;

        public ScorePacket(ClientInfo clientInfo, int score) : base(clientInfo)
        {
            Score = score;
        }
    }
}
