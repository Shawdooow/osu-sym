using System;

namespace Symcol.Networking.Packets
{
    [Serializable]
    public abstract class Packet
    {
        /// <summary>
        /// Just a Signature
        /// </summary>
        public string Address;

        /// <summary>
        /// Specify starting size of a packet (bytes) for efficiency
        /// </summary>
        public virtual int PacketSize => 512;
    }
}
