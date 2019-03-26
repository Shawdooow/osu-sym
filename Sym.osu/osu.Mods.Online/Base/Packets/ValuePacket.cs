using System;
using System.Runtime.Serialization;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public class ValuePacket<T> : Packet, ISerializable
        where T : struct
    {
        public virtual T Value { get; set; }

        public virtual string Name { get; set; }

        public virtual long ID { get; set; }

        public ValuePacket()
        {
        }

        public ValuePacket(SerializationInfo info, StreamingContext context)
        {
            ID = (long)info.GetValue("i", typeof(long));
            Name = (string)info.GetValue("n", typeof(string));
            Value = (T)info.GetValue("v", typeof(T));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("i", ID, typeof(long));
            info.AddValue("n", Name, typeof(string));
            info.AddValue("v", Value, typeof(T));
        }
    }
}
