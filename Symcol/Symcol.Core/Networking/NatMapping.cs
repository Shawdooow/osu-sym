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

                    //TODO: Don't be old Dean
                    try
                    {
                        foreach (Mapping m in Mappings)
                            device.CreatePortMap(m);
                    }
                    catch { }
                }
            }
        }

        private static INatDevice device;

        public static void Add(Mapping mapping)
        {
            if (NatDevice == null) return;
            //NatDevice.CreatePortMap(mapping);
            Mappings.Add(mapping);
        }

        public static void Remove(Mapping mapping)
        {
            if (NatDevice == null) return;
            //NatDevice.DeletePortMap(mapping);
            Mappings.Remove(mapping);
        }
    }
}
