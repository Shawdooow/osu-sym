﻿using osu.Game.Rulesets.Vitaru.Mods.Chaptersets.Chapters;
using osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Scarlet.Characters;
using osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Scarlet.Characters.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Scarlet
{
    public class ScarletChapter : TouhosuChapter
    {
        public override string Title => "The Chapter in Scarlet";

        public override TouhosuPlayer[] GetTouhosuPlayers() => new TouhosuPlayer[]
        {
            new Remilia(),
            new Flandre(),
            new Sakuya(),
        };

        public override DrawableTouhosuPlayer GetDrawableTouhosuPlayer(VitaruPlayfield playfield, TouhosuPlayer player)
        {
            switch (player.Name)
            {
                default:
                    return null;

                case "Sakuya Izayoi":
                    return new DrawableSakuya(playfield);
                case "Remilia Scarlet":
                    return new DrawableRemilia(playfield);
                case "Flandre Scarlet":
                    return new DrawableFlandre(playfield);
            }
        }
    }
}