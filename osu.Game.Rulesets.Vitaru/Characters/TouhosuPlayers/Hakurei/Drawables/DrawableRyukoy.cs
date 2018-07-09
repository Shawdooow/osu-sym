using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Hakurei.Drawables
{
    public class DrawableRyukoy : DrawableTouhosuPlayer
    {
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
