﻿using osu.Framework.Graphics;
using osu.Game.Screens;
using OpenTK;
using OpenTK.Graphics;
using Symcol.osu.Core.Containers.Shawdooow;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Core.Wiki;

namespace Symcol.osu.Mods.MapMixer
{
    public class MapMixerModSet : SymcolModSet
    {
        public override SymcolButton GetMenuButton() => new SymcolButton
        {
            ButtonName = "Map Mixer",
            ButtonFontSizeMultiplier = 0.8f,
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            ButtonColorTop = Color4.Purple,
            ButtonColorBottom = Color4.HotPink,
            ButtonSize = 120,
            ButtonPosition = new Vector2(-200, -150),
        };

        public override OsuScreen GetMenuScreen() => new MapMixer();

        //public override WikiSet GetWikiSet() => new MapMixerWikiSet();
    }
}