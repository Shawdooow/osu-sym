#region usings

using System;
using System.Runtime.Serialization;

#endregion

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public class ValueArrayPacket<T, Y> : SharePacket
        where T : struct
    {
        public override uint PacketSize => 8192;

        public virtual T Value { get; set; }

        public virtual Y[] Array { get; set; }

        public ValueArrayPacket()
        {
        }

        public ValueArrayPacket(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Value = (T)info.GetValue("v", typeof(T));
            Array = (Y[])info.GetValue("a", typeof(Y[]));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("v", Value, typeof(T));
            info.AddValue("a", Array, typeof(Y[]));
        }
    }
}
