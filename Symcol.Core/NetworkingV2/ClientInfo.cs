namespace Symcol.Core.NetworkingV2
{
    /// <summary>
    /// Just a client signature
    /// </summary>
    public class ClientInfo
    {
        public string Address;

        public string IP;

        public int Port;

        /// <summary>
        /// Last successful ping to this client
        /// </summary>
        public double Ping;

        public int ConncetionTryCount;

        public double LastConnectionTime;
    }
}
