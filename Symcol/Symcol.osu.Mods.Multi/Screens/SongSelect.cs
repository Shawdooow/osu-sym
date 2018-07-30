﻿using System;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Screens;
using Symcol.osu.Mods.Multi.Networking;

namespace Symcol.osu.Mods.Multi.Screens
{
    public class SongSelect : global::osu.Game.Screens.Select.SongSelect
    {
        private bool exiting;

        protected override BackgroundScreen CreateBackground() => null;

        public Action<WorkingBeatmap> SelectionFinalised;

        public readonly OsuNetworkingClientHandler OsuNetworkingClientHandler;

        public SongSelect(OsuNetworkingClientHandler osuNetworkingClientHandler)
        {
            OsuNetworkingClientHandler = osuNetworkingClientHandler;
        }

        protected override void OnEntering(Screen last)
        {
            Add(OsuNetworkingClientHandler);
            base.OnEntering(last);
        }

        protected override bool OnStart()
        {
            if (!exiting)
            {
                SelectionFinalised(Beatmap.Value);
                exiting = true;
                Remove(OsuNetworkingClientHandler);
                Exit();
            }
            return true;
        }
    }
}
