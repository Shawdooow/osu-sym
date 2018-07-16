using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Characters.Bosses.DrawableBosses;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Abilities;
using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers.DrawableVitaruPlayers;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.UI;
using OpenTK;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Hakurei.Drawables
{
    public class DrawableRyukoy : DrawableTouhosuPlayer
    {
        public override bool Untuned
        {
            get => untuned;
            set
            {
                if (value == untuned) return;

                untuned = value;


                if (value)
                {
                    VitaruPlayfield.Gamefield.Remove(this);
                    VitaruPlayfield.VitaruInputManager.BlurredPlayfield.Add(this);
                    CurrentPlayfield = VitaruPlayfield.VitaruInputManager.BlurredPlayfield;
                }
                else
                {
                    VitaruPlayfield.VitaruInputManager.BlurredPlayfield.Remove(this);
                    VitaruPlayfield.Gamefield.Add(this);
                    CurrentPlayfield = VitaruPlayfield.Gamefield;
                }

                if (VitaruPlayfield.Player == this)
                {
                    if (value)
                    {
                        VitaruPlayfield.Remove(VitaruPlayfield.Gamefield);
                        VitaruPlayfield.VitaruInputManager.BlurContainer.Add(VitaruPlayfield.Gamefield);
                        VitaruPlayfield.Gamefield.Margin = 0.8f;
                        VitaruPlayfield.VitaruInputManager.BlurContainer.Remove(VitaruPlayfield.VitaruInputManager.BlurredPlayfield);
                        VitaruPlayfield.Add(VitaruPlayfield.VitaruInputManager.BlurredPlayfield);
                        VitaruPlayfield.VitaruInputManager.BlurredPlayfield.Margin = 1f;
                    }
                    else
                    {
                        VitaruPlayfield.VitaruInputManager.BlurContainer.Remove(VitaruPlayfield.Gamefield);
                        VitaruPlayfield.Add(VitaruPlayfield.Gamefield);
                        VitaruPlayfield.Gamefield.Margin = 1f;
                        VitaruPlayfield.Remove(VitaruPlayfield.VitaruInputManager.BlurredPlayfield);
                        VitaruPlayfield.VitaruInputManager.BlurContainer.Add(VitaruPlayfield.VitaruInputManager.BlurredPlayfield);
                        VitaruPlayfield.VitaruInputManager.BlurredPlayfield.Margin = 0.8f;
                    }
                }
            }
        }

        private bool untuned;

        public DrawableRyukoy(VitaruPlayfield playfield, VitaruNetworkingClientHandler vitaruNetworkingClientHandler) : base(playfield, new Ryukoy(), vitaruNetworkingClientHandler)
        {
            Spell += input => { Untuned = true; };
        }

        protected override void SpellUpdate()
        {
            base.SpellUpdate();

            if (SpellActive)
            {
                int bullets = 0;
                int players = 0;
                int enemies = 0;
                int bosses = 0;

                foreach (Drawable draw in VitaruPlayfield.VitaruInputManager.BlurredPlayfield)
                    switch (draw)
                    {
                        case DrawableBullet bullet:
                            bullets++;
                            break;
                        case DrawableVitaruPlayer player:
                            players++;
                            break;
                        case Enemy enemy:
                            enemies++;
                            break;
                        case DrawableBoss boss:
                            bosses++;
                            break;
                    }

                Energy -= Clock.ElapsedFrameTime / 1000 * (bullets * Ryukoy.BULLET_DRAIN_MULTIPLIER + players * Ryukoy.PLAYER_DRAIN_MULTIPLIER + enemies * Ryukoy.ENEMY_DRAIN_MULTIPLIER + bosses * Ryukoy.BOSS_DRAIN_MULTIPLIER);
            }
            else if (Untuned)
                Untuned = false;
        }

        protected override void SpellDeactivate(VitaruAction action)
        {
            base.SpellDeactivate(action);
            Untuned = false;
        }

        protected override void Pressed(VitaruAction action)
        {
            base.Pressed(action);

            if (action == VitaruAction.Pull)
            {
                restart:
                foreach (Drawable draw in Untuned ? VitaruPlayfield.Gamefield : VitaruPlayfield.VitaruInputManager.BlurredPlayfield)
                    if (draw is ITuneable tunable && draw.Alpha > 0)
                    {
                        Vector2 drawPos = Cursor.ToSpaceOfOtherDrawable(Vector2.Zero, draw);
                        float distance = (float)Math.Sqrt(Math.Pow(drawPos.X, 2) + Math.Pow(drawPos.Y, 2));

                        if (100 >= distance)
                        {
                            tunable.Untuned = !tunable.Untuned;
                            goto restart;
                        }
                    }
            }
        }
    }
}
