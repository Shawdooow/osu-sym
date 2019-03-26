﻿#region usings

using System;
using System.Runtime.Serialization;
using osuTK;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public class Vector2Packet : SharePacket
    {
        public float X => Vector2.X;
        public float Y => Vector2.Y;

        public virtual Vector2 Vector2 { get; set; } = Vector2.Zero;

        public Vector2Packet()
        {
        }

        public Vector2Packet(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Vector2 = new Vector2((float)info.GetValue("x", typeof(float)), (float)info.GetValue("y", typeof(float)));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("x", Vector2.X, typeof(float));
            info.AddValue("y", Vector2.Y, typeof(float));
        }
    }
}
