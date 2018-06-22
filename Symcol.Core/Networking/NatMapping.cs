using System.Collections.Generic;
using Mono.Nat;

namespace Symcol.Core.Networking
{
    public static class NatMapping
    {
        public static List<Mapping> Mappings = new List<Mapping>();

        public static INatDevice NatDevice
        {
            get => device;
            set
            {
                if (!Equals(value, device) && device == null)
                {
                    device = value;

                    foreach (Mapping m in Mappings)
                        device.CreatePortMap(m);
                }
            }
        }

        private static INatDevice device;

        public static void Add(Mapping mapping)
        {
            Mappings.Add(mapping);
            NatDevice?.CreatePortMap(mapping);
        }

        public static void Remove(Mapping mapping)
        {
            NatDevice?.DeletePortMap(mapping);
            Mappings.Remove(mapping);
        }
    }
}
