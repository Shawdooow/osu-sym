﻿using Symcol.Core.Networking;
using System;

namespace Symcol.Rulesets.Core.Multiplayer.Networking
{
    [Serializable]
    public class RulesetPacket : Packet
    {
        public new readonly RulesetClientInfo ClientInfo;

        public override int PacketSize => 4096;

        public bool SetMap;

        public int OnlineBeatmapSetID = -1;

        public int OnlineBeatmapID = -1;

        public string BeatmapName;

        public string Mapper;

        public string BeatmapDifficulty;

        public bool HaveMap;

        public string ChatContent;

        //public string RulesetName = "";

        public RulesetPacket(RulesetClientInfo rulesetClientInfo) : base(rulesetClientInfo)
        {
            ClientInfo = rulesetClientInfo;
        }
    }
}
