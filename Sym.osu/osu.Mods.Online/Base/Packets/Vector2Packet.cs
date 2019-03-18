using System;
using System.Runtime.Serialization;
using osuTK;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public class Vector2Packet : Packet, ISerializable
    {
        public float X => Vector2.X;
        public float Y => Vector2.Y;

        public virtual Vector2 Vector2 { get; set; } = Vector2.Zero;

        public virtual string Name { get; set; }

        public virtual long ID { get; set; }

        public Vector2Packet()
        {
        }

        public Vector2Packet(SerializationInfo info, StreamingContext context)
        {
            ID = (long)info.GetValue("i", typeof(long));
            Name = (string)info.GetValue("n", typeof(string));
            Vector2 = new Vector2((float)info.GetValue("x", typeof(float)), (float)info.GetValue("y", typeof(float)));
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("i", ID, typeof(long));
            info.AddValue("n", Name, typeof(string));
            info.AddValue("x", Vector2.X, typeof(float));
            info.AddValue("y", Vector2.Y, typeof(float));
        }
    }
}
