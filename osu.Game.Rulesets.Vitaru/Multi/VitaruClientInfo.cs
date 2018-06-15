using System;
using Symcol.Rulesets.Core.LegacyMultiplayer.Networking;

namespace osu.Game.Rulesets.Vitaru.Multi
{
    [Serializable]
    public class VitaruClientInfo : RulesetClientInfo
    {
        public VitaruPlayerInformation PlayerInformation;
    }
}
