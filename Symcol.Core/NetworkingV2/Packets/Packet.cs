using System;

namespace Symcol.Core.NetworkingV2.Packets
{
    [Serializable]
    public class Packet
    {
        /// <summary>
        /// Just a Signature
        /// </summary>
        public string Address;

        /// <summary>
        /// Specify starting size of a packet (bytes) for efficiency
        /// </summary>
        public virtual int PacketSize => 512;

        public Packet(string address)
        {
            Address = address;
        }
    }
}
