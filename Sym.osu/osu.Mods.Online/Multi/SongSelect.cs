#region usings

using System;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Screens;

#endregion

namespace osu.Mods.Online.Multi
{
    public class SongSelect : Game.Screens.Select.SongSelect
    {
        private bool exiting;

        protected override BackgroundScreen CreateBackground() => null;

        public Action<WorkingBeatmap> SelectionFinalised;

        protected override bool OnStart()
        {
            if (!exiting)
            {
                SelectionFinalised(Beatmap.Value);
                exiting = true;
                this.Exit();
            }
            return true;
        }
    }
}
