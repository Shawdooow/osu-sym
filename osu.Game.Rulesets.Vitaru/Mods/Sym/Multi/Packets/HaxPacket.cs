﻿#region usings

using System;
using Sym.Networking.Packets;

#endregion

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Multi.Packets
{
    [Serializable]
    public class HaxPacket : Packet
    {
        public bool Hax;
    }
}