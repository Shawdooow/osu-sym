using Mono.Nat;
using System.Collections.Generic;

namespace Symcol.Core.Networking
{
    public class NatMapping
    {
        public readonly Mapping UdpMapping;

        public static readonly List<INatDevice> NatDevices = new List<INatDevice>();

        public NatMapping(Mapping mapping)
        {
            UdpMapping = mapping;
        }
    }
}
