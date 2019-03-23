#region usings

using osu.Game.Rulesets.Vitaru.Mods.Gamemodes;

#endregion

namespace osu.Game.Rulesets.Vitaru.Mods.ChapterSets
{
    public abstract class Chapterset
    {
        public abstract VitaruGamemode[] GetGamemodes();
    }
}
