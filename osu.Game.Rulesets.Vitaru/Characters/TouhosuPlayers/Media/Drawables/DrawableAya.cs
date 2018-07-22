﻿using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Abilities;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.UI;
using OpenTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Media.Drawables
{
    public class DrawableAya : DrawableTouhosuPlayer
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly Camera camera;

        public DrawableAya(VitaruPlayfield playfield, VitaruNetworkingClientHandler vitaruNetworkingClientHandler)
            : base(playfield, new Aya(), vitaruNetworkingClientHandler)
        {
            VitaruPlayfield.Gamefield.Add(camera = new Camera());

            Spell += action =>
            {
                ScreenSnap snap = new ScreenSnap(camera.CameraBox);
                playfield.VitaruInputManager.Add(snap);

                foreach (Drawable draw in CurrentPlayfield)
                {
                    DrawableBullet bullet = draw as DrawableBullet;
                    if (bullet?.Hitbox != null && Hitbox.HitDetect(camera.Hitbox, bullet.Hitbox))
                    {
                        bullet.Bullet.Damage = 0;
                        bullet.ReturnJudgement = true;
                        bullet.Masking = true;
                        bullet.Alpha = 0;
                    }
                }
            };
        }

        protected override void Update()
        {
            base.Update();
            camera.Position = Cursor.Position;
        }
    }
}