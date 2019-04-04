using System;
using System.Runtime.Serialization;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public sealed class MapSetPacket : Packet, ISerializable
    {
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
