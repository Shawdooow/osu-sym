namespace Symcol.Core.NetworkingV2.Packets
{
    public class DisconnectPacket : Packet
    {
        public DisconnectPacket(string address)
            : base(address)
        {
        }
    }
}
