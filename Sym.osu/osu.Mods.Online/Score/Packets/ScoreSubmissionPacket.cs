using System;
using osu.Mods.Online.Base.Packets;

namespace osu.Mods.Online.Score.Packets
{
    [Serializable]
    public class ScoreSubmissionPacket : OnlinePacket
    {
        public OnlineScore Score;
    }
}
