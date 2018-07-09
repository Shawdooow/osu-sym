using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.UI;

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
                //Energy -= Clock.ElapsedFrameTime / 1000 * TouhosuPlayer.EnergyDrainRate * 0.25f;

                //abstraction.Value = level;
                //applyToClock(workingBeatmap.Value.Track, setPitch);
            }
            else
            {
                //applyToClock(workingBeatmap.Value.Track, 1);
                //abstraction.Value = 0;
            }
        }

        protected override void SpellDeactivate(VitaruAction action)
        {
            base.SpellDeactivate(action);
            Untuned = false;
        }

        protected override void Pressed(VitaruAction action)
        {
            base.Pressed(action);
        }
    }
}
