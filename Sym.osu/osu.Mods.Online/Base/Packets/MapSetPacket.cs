#region usings

using System;
using System.Runtime.Serialization;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public sealed class MapSetPacket : Packet, ISerializable
    {
        public override uint PacketSize => 8192;

        public string MapName { get; private set; }

        public MapSetPacket(string map)
        {
            MapName = map;
        }

        public MapSetPacket(SerializationInfo info, StreamingContext context)
        {
            MapName = info.GetString("f");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("f", MapName, typeof(string));
        }
    }
}
