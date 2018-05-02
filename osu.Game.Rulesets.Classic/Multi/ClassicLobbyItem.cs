﻿using osu.Framework.Graphics.Textures;
using Symcol.Rulesets.Core.Multiplayer.Screens;

namespace osu.Game.Rulesets.Classic.Multi
{
    public class ClassicLobbyItem : RulesetLobbyItem
    {
        public override Texture Icon => ClassicRuleset.ClassicTextures.Get("icon");

        public override string RulesetName => "Classic!";

        public override Texture Background => ClassicRuleset.ClassicTextures.Get("osu!classic Thumbnail");

        public override RulesetLobbyScreen RulesetLobbyScreen => new ClassicLobbyScreen();
    }
}