using Mono.Nat;

namespace Symcol.Core.LegacyNetworking
{
    public class NatMapping
    {
        public readonly Mapping UdpMapping;

        public static INatDevice NatDevice;

        public NatMapping(Mapping mapping)
        {
            UdpMapping = mapping;
        }
    }
}
