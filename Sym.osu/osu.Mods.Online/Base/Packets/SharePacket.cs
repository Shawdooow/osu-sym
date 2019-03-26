using System;
using System.Runtime.Serialization;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public class SharePacket : Packet, ISerializable
    {
        public virtual string Name { get; set; }

        public virtual long ID { get; set; }

        public SharePacket()
        {
        }

        public SharePacket(SerializationInfo info, StreamingContext context)
        {
            ID = (long)info.GetValue("i", typeof(long));
            Name = (string)info.GetValue("n", typeof(string));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("i", ID, typeof(long));
            info.AddValue("n", Name, typeof(string));
        }
    }
}
