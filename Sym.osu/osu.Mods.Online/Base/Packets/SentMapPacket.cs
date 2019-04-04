#region usings

using System;
using System.Runtime.Serialization;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public sealed class SentMapPacket : Packet, ISerializable
    {
        public override uint PacketSize => 8192;

        public string MapName { get; private set; }

        public long Size { get; private set; }

        public SentMapPacket(string map, long size)
        {
            MapName = map;
            Size = size;
        }

        public SentMapPacket(SerializationInfo info, StreamingContext context)
        {
            MapName = info.GetString("f");
            Size = info.GetInt64("s");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("f", MapName, typeof(string));
            info.AddValue("s", Size, typeof(long));
        }
    }
}
