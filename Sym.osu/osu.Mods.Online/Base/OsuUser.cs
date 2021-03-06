﻿#region usings

using System;
using System.Collections.Generic;
using osu.Mods.Online.Multi.Match.Pieces;
using osu.Mods.Online.Multi.Settings;

#endregion

namespace osu.Mods.Online.Base
{
    /// <summary>
    /// osu User information
    /// DOES NOT INCLUDE PASSWORD!!!
    /// </summary>
    [Serializable]
    public class OsuUser
    {
        public string Username = "";

        public long ID = -1;

        public string Colour = "#ffffff";

        public string Pic;

        public string Background;

        public string Country;

        public string CountryFlagName;

        public List<Setting> UserSettings = new List<Setting>();

        public PlayerStatues Statues;
    }
}
