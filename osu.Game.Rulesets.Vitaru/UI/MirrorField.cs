using OpenTK;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Beatmaps;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Settings;
using System.Collections.Generic;
using osu.Game.Audio;
using osu.Game.Rulesets.Vitaru.Characters;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class MirrorField : VitaruPlayfield
    {
        private readonly Gamemodes currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);

        private readonly List<VitaruPlayer> enemyList = new List<VitaruPlayer>();

        private readonly VitaruPlayfield vitaruPlayfield;

        public MirrorField(VitaruPlayfield vitaruPlayfield, VitaruInputManager vitaruInput) : base (vitaruInput)
        {
            this.vitaruPlayfield = vitaruPlayfield;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Position = new Vector2(-40, 400);
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;
            Rotation = 180;

            foreach (var o in VitaruBeatmapConverter.HitObjectList)
            {
                var p = (Pattern)o;
                p.Samples = new List<SampleInfo>();
                Add(new DrawablePattern(p, this));
            }
            VitaruBeatmapConverter.HitObjectList = new List<Rulesets.Objects.HitObject>();
        }

        public override void Add(DrawableHitObject h)
        {
            h.Depth = (float)h.HitObject.StartTime;
            base.Add(h);
        }
    }
}
