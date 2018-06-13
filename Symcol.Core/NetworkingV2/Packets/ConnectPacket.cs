using System;

namespace Symcol.Core.NetworkingV2.Packets
{
    [Serializable]
    public class ConnectPacket : Packet
    {
        public ConnectPacket(string address)
            : base(address)
        {
        }
    }
}
