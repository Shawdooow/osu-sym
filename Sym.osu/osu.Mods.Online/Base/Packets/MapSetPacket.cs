using System;
using System.Runtime.Serialization;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public sealed class MapSetPacket : Packet, ISerializable
    {
        public override uint PacketSize => 8192;

        public string MapName { get; private set; }

        public int Pieces { get; private set; }

        public MapSetPacket(string map, int pieces)
        {
            MapName = map;
            Pieces = pieces;
        }

        public MapSetPacket(SerializationInfo info, StreamingContext context)
        {
            MapName = info.GetString("f");
            Pieces = info.GetInt32("p");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("f", MapName, typeof(string));
            info.AddValue("p", Pieces, typeof(int));
        }
    }
}
