﻿using System;

namespace Symcol.Core.Networking.Packets
{
    [Serializable]
    public sealed class TestPacket : Packet
    {
        public override int PacketSize => 128;
    }
}