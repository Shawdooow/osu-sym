using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Inlaws.Drawables
{
    public class DrawableAlice : DrawableTouhosuPlayer
    {
        public DrawableAlice(VitaruPlayfield playfield) : base(playfield, new Alice())
        {
        }

        protected override void SpellUpdate()
        {
            base.SpellUpdate();

            if (SpellActive)
            {
                foreach (Drawable drawable in VitaruPlayfield.Children)
                    if (drawable is DrawableTouhosuPlayer drawableTouhosuPlayer)
                    {
                        
                    }
            }
        }
    }
}
