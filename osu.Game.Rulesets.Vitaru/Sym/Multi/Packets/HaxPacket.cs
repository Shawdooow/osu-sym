#region usings

using System;
using System.Runtime.Serialization;
using osu.Mods.Online.Base.Packets;
using Sym.Networking.Packets;

#endregion

namespace osu.Game.Rulesets.Vitaru.Sym.Multi.Packets
{
    [Serializable]
    public class HaxPacket : SharePacket
    {
        public bool Hax { get; set; }

        public HaxPacket()
        {
        }

        public HaxPacket(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Hax = info.GetBoolean("h");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("h", Hax);
        }
    }
}
