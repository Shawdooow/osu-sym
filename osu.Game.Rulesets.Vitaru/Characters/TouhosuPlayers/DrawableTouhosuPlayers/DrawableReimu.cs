using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.DrawableTouhosuPlayers
{
    public class DrawableReimu : DrawableTouhosuPlayer
    {
        public DrawableReimu(VitaruPlayfield playfield, VitaruNetworkingClientHandler vitaruNetworkingClientHandler) : base(playfield, new Reimu(), vitaruNetworkingClientHandler)
        {
            Spell += (input) =>
            {

            };
        }

        protected override void SpellUpdate()
        {
            base.SpellUpdate();

            if (SpellActive)
            {
                foreach (Drawable drawable in VitaruPlayfield.GameField.Current)
                    if (drawable is DrawableTouhosuPlayer drawableTouhosuPlayer)
                    {
                        
                    }
            }
        }
    }
}
