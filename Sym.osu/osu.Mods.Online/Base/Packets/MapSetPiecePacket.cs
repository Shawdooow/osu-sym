using System;
using System.Runtime.Serialization;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public sealed class MapSetPiecePacket : Packet, ISerializable
    {
        public override uint PacketSize => 8192 * 32;

        public string MapName { get; private set; }

        public string ZipSerial { get; private set; }

        public int Piece { get; private set; }

        public MapSetPiecePacket(string serial, string map, int piece)
        {
            MapName = map;
            ZipSerial = serial;
            Piece = piece;
        }

        public MapSetPiecePacket(SerializationInfo info, StreamingContext context)
        {
            MapName = info.GetString("f");
            ZipSerial = info.GetString("z");
            Piece = info.GetInt32("p");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("f", MapName, typeof(string));
            info.AddValue("z", ZipSerial, typeof(string));
            info.AddValue("p", Piece, typeof(int));
        }
    }
}
