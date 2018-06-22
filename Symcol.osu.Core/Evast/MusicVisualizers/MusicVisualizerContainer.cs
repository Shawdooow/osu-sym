// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Configuration;
using osu.Framework.Graphics.Containers;
using osu.Game.Beatmaps;

namespace Symcol.osu.Core.Evast.MusicVisualizers
{
    public abstract class MusicVisualizerContainer : Container
    {
        private readonly Bindable<WorkingBeatmap> beatmap = new Bindable<WorkingBeatmap>();

        protected MusicVisualizerContainer(Bindable<WorkingBeatmap> b)
        {
            beatmap.BindTo(b);
        }

        private int updateDelay = 1;
        public int UpdateDelay
        {
            set
            {
                if (updateDelay == value)
                    return;
                updateDelay = value;

                if (!IsLoaded)
                    return;

                Restart();
            }
            get { return updateDelay; }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Start();
        }

        private void updateAmplitudes()
        {
            var frequencyAmplitudes = beatmap.Value.Track?.CurrentAmplitudes.FrequencyAmplitudes ?? new float[256];
            OnAmplitudesUpdate(frequencyAmplitudes);
            Scheduler.AddDelayed(updateAmplitudes, UpdateDelay);
        }

        protected void Start() => updateAmplitudes();

        protected abstract void OnAmplitudesUpdate(float[] amplitudes);

        protected void Restart()
        {
            Scheduler.CancelDelayedTasks();
            Start();
        }
    }
}
