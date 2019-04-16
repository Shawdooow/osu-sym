#region usings

using System;
using System.Runtime.Serialization;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public class SharePacket : Packet, ISerializable
    {
        public override int PacketSize => 1024;

        public virtual string Name { get; set; }

        public virtual long ID { get; set; }

        public SharePacket()
        {
        }

        public SharePacket(SerializationInfo info, StreamingContext context)
        {
            ID = (long)info.GetValue("i", typeof(long));
            Name = info.GetString("n");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("i", ID);
            info.AddValue("n", Name);
        }
    }
}
