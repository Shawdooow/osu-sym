namespace Symcol.Core.Networking
{
    /// <summary>
    /// Just a client signature
    /// </summary>
    public class ClientInfo
    {
        public string Address;

        public string IP;

        public int Port;

        public string Gamekey;

        /// <summary>
        /// Last successful ping to this client
        /// </summary>
        public double Ping;

        public int ConnectionTryCount;

        public double LastConnectionTime;
    }
}
