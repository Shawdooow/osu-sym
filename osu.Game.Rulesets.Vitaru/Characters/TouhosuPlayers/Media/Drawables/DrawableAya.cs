using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Abilities;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.UI;

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
            };
        }

        protected override void Update()
        {
            base.Update();
            camera.Position = Cursor.Position;
        }
    }
}
