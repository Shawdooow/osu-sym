using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Abilities;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.UI;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Media.Drawables
{
    public class DrawableAya : DrawableTouhosuPlayer
    {
        public DrawableAya(VitaruPlayfield playfield, VitaruNetworkingClientHandler vitaruNetworkingClientHandler)
            : base(playfield, new Aya(), vitaruNetworkingClientHandler)
        {
            Spell += action =>
            {
                Add(new ScreenSnap((Box)playfield.BorderContainer.Child));
            };
        }
    }
}
