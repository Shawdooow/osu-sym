using System;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets
{
    [Serializable]
    public class MatchListPacket : Packet
    {
        public override int PacketSize => 1024;

        public string Name = @"Welcome to Symcol!";

        #region User

        public string Username = @"Shawdooow";

        public int UserID = 7726082;

        public string UserCountry = "US";

        #endregion

        #region Beatmap

        public string BeatmapTitle = "Lost Emotion";

        public string BeatmapArtist = "Masayoshi Minoshima feat.nomico";

        public double BeatmapStars = 1.96d;

        #endregion

        public int RulesetID = 4;
    }
}
