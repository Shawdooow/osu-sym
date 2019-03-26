using System;
using System.Runtime.Serialization;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public class ArrayPacket<T> : Packet, ISerializable
        where T : struct
    {
        public virtual T[] Array { get; set; }

        public virtual string Name { get; set; }

        public virtual long ID { get; set; }

        public ArrayPacket()
        {
        }

        public ArrayPacket(SerializationInfo info, StreamingContext context)
        {
            ID = (long)info.GetValue("i", typeof(long));
            Name = (string)info.GetValue("n", typeof(string));
            Array = (T[])info.GetValue("a", typeof(T[]));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("i", ID, typeof(long));
            info.AddValue("n", Name, typeof(string));
            info.AddValue("a", Array, typeof(T[]));
        }
    }
}
