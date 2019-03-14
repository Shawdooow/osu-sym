using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Packets.Match;
using osu.Mods.Online.Multi.Settings;

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Multi
{
    public class VitaruOnlineCharacterSelection : MultiplayerOption
    {
        public VitaruOnlineCharacterSelection(OsuNetworkingHandler networking, string name, int quadrant, bool sync = true)
            : base(networking, name, quadrant, sync)
        {
        }

        protected override void TriggerBindableChange()
        {
            throw new System.NotImplementedException();
        }

        protected override void SetValue(SettingsPacket settings)
        {
            throw new System.NotImplementedException();
        }
    }
}
