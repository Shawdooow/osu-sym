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
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class MirrorField : VitaruPlayfield
    {
        private readonly VitaruGamemode currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<VitaruGamemode>(VitaruSetting.GameMode);
        private SelectableCharacters currentCharacter = VitaruSettings.VitaruConfigManager.GetBindable<SelectableCharacters>(VitaruSetting.Characters);
        private readonly bool multiplayer = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.ShittyMultiplayer);
        private bool enemyPlayerOverride = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.EnemyPlayerOverride);
        private readonly Bindable<int> enemyPlayerCount = VitaruSettings.VitaruConfigManager.GetBindable<int>(VitaruSetting.EnemyPlayerCount);

        private readonly Player enemyOne = VitaruSettings.VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyOne);
        private readonly Player enemyTwo = VitaruSettings.VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyTwo);
        private readonly Player enemyThree = VitaruSettings.VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyThree);
        private readonly Player enemyFour = VitaruSettings.VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyFour);
        private readonly Player enemyFive = VitaruSettings.VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyFive);
        private readonly Player enemySix = VitaruSettings.VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemySix);
        private readonly Player enemySeven = VitaruSettings.VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemySeven);
        private readonly Player enemyEight = VitaruSettings.VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyEight);

        private readonly List<Player> enemyList = new List<Player>();

        private readonly VitaruPlayfield vitaruPlayfield;

        public MirrorField(VitaruPlayfield vitaruPlayfield) : base()
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

            //Multiplayer testing :P
            /*
            if (multiplayer && currentGameMode != VitaruGamemode.Dodge)
            {
                switch (enemyPlayerCount)
                {
                    case 0:
                        break;
                    case 1:
                        enemyList.Add(new VitaruPlayer(this, enemyOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512f / 2, 100), Auto = true, Bot = true, Team = 2 });
                        break;
                    case 2:
                        enemyList.Add(new VitaruPlayer(this, enemyOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 200, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(200, 100), Auto = true, Bot = true, Team = 2 });
                        break;
                    case 3:
                        enemyList.Add(new VitaruPlayer(this, enemyOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(0, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(200, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyThree) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512f / 2, 100), Auto = true, Bot = true, Team = 2 });
                        break;
                    case 4:
                        enemyList.Add(new VitaruPlayer(this, enemyOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(0, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(200, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyThree) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 200, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyFour) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512, 100), Auto = true, Bot = true, Team = 2 });
                        break;
                    case 5:
                        enemyList.Add(new VitaruPlayer(this, enemyOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(0, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(200, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyThree) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512f / 2, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyFour) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 200, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyFive) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512, 100), Auto = true, Bot = true, Team = 2 });
                        break;
                    case 6:
                        enemyList.Add(new VitaruPlayer(this, enemyOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(0, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(150, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyThree) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(250, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyFour) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 250, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyFive) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 150, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemySix) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512, 100), Auto = true, Bot = true, Team = 2 });
                        break;
                    case 7:
                        enemyList.Add(new VitaruPlayer(this, enemyOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(0, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(125, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyThree) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(200, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyFour) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512f / 2, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyFive) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 200, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemySix) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 125, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemySeven) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512, 100), Auto = true, Bot = true, Team = 2 });
                        break;
                    case 8:
                        enemyList.Add(new VitaruPlayer(this, enemyOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(0, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(125, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyThree) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(200, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyFour) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(250, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyFive) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 250, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemySix) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 200, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemySeven) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 125, 100), Auto = true, Bot = true, Team = 2 });
                        enemyList.Add(new VitaruPlayer(this, enemyEight) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512, 100), Auto = true, Bot = true, Team = 2 });
                        break;
                }

                foreach (VitaruPlayer enemy in enemyList)
                    CharacterField.Add(enemy);
            }
            */

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
