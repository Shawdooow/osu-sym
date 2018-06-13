using System;

namespace Symcol.Core.NetworkingV2
{
    /// <summary>
    /// Just a client signature
    /// </summary>
    [Serializable]
    public class ClientInfo
    {
        public string ClientID;

        public int ConncetionTryCount;

        public double LastConnectionTime;
    }
}
