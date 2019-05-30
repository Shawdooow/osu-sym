using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Inlaws.Drawables
{
    public class DrawableAlice : DrawableTouhosuPlayer
    {
        public DrawableAlice(VitaruPlayfield playfield) : base(playfield, new Alice())
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
                foreach (Drawable drawable in VitaruPlayfield)
                    if (drawable is DrawableTouhosuPlayer drawableTouhosuPlayer)
                    {
                        
                    }
            }
        }
    }
}
