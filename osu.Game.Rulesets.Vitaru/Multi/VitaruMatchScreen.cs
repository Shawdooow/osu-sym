﻿using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Rulesets.Vitaru.Characters;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using Symcol.Core.Networking;
using Symcol.Rulesets.Core.Multiplayer.Options;
using Symcol.Rulesets.Core.Multiplayer.Pieces;
using Symcol.Rulesets.Core.Multiplayer.Screens;
using Symcol.Rulesets.Core.Rulesets;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Multi
{
    public class VitaruMatchScreen : RulesetMatchScreen
    {
        private Bindable<TouhosuCharacters> selectedTouhosuCharacter = VitaruSettings.VitaruConfigManager.GetBindable<TouhosuCharacters>(VitaruSetting.TouhosuCharacter);
        private readonly Bindable<GraphicsPresets> currentGraphics = VitaruSettings.VitaruConfigManager.GetBindable<GraphicsPresets>(VitaruSetting.GraphicsPresets);
        private readonly Bindable<Gamemodes> currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);
        private readonly Bindable<bool> comboFire = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.ComboFire);

        public readonly VitaruNetworkingClientHandler VitaruNetworkingClientHandler;

        public VitaruMatchScreen(VitaruNetworkingClientHandler vitaruNetworkingClientHandler) : base(vitaruNetworkingClientHandler)
        {
            VitaruNetworkingClientHandler = vitaruNetworkingClientHandler;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            MatchTools.Mode.ValueChanged += (value) =>
            {
                if (value == MatchScreenMode.RulesetSettings)
                    MatchTools.SelectedContent.Child = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            new MultiplayerToggleOption(comboFire, "Enable Combo Fire", 4, false),
                            new MultiplayerDropdownEnumOption<GraphicsPresets>(currentGraphics, "Graphics", 3, false),
                            new MultiplayerDropdownEnumOption<Gamemodes>(currentGameMode, "Vitaru Gamemode", 1),
                            new MultiplayerDropdownEnumOption<TouhosuCharacters>(selectedTouhosuCharacter, "Character", 2, false),
                        }
                    };
            };
        }

        protected override void OnEntering(Screen last)
        {
            base.OnEntering(last);

            VitaruPlayfield.LoadPlayerList = new List<VitaruClientInfo>();
            SymcolPlayfield.RulesetNetworkingClientHandler = VitaruNetworkingClientHandler;
            VitaruNetworkingClientHandler.OnLoadGame = (i) => Load(i);
        }

        protected override void OnResuming(Screen last)
        {
            base.OnResuming(last);
            VitaruPlayfield.LoadPlayerList = new List<VitaruClientInfo>();
        }

        protected override void Load(List<ClientInfo> playerList)
        {
            foreach (ClientInfo client in playerList)
                if (client is VitaruClientInfo vitaruClientInfo)
                    VitaruPlayfield.LoadPlayerList.Add(vitaruClientInfo);

            base.Load(playerList);
        }
    }
}
