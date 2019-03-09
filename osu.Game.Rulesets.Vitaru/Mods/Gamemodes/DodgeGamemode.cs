using osuTK;

namespace osu.Game.Rulesets.Vitaru.Mods.Gamemodes
{
    public class DodgeGamemode : VitaruGamemode
    {
        public override string Name => "Dodge";

        public override Vector2 PlayfieldAspectRatio => new Vector2(6, 3);

        public override Vector2 PlayfieldSize => new Vector2(512 + 256, 384);

        public override Vector2 PlayerStartingPosition => PlayfieldSize / 2;

        public override Vector2 ClusterOffset => new Vector2(128, 0);

        public override string Description => "Completly changes how vitaru is played. " +
                                                      "The Dodge gamemode changes the playfield to a much shorter rectangle and send bullets your way from all directions "
                                                      + "while also taking away your ability to shoot!";
    }
}
