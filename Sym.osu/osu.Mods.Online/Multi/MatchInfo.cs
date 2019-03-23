﻿#region usings

using System;
using System.Collections.Generic;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Settings;

#endregion

namespace osu.Mods.Online.Multi
{
    [Serializable]
    public class MatchInfo
    {
        public string Name = @"Welcome to Symcol!";

        public uint MatchID;

        public List<OsuUserInfo> Users = new List<OsuUserInfo>();

        public List<Setting> Settings = new List<Setting>();

        public Host Host;

        public Map Map;
    }
}