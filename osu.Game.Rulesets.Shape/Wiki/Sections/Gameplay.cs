﻿using osu.Framework.Allocation;
using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Shape.Wiki.Sections
{
    public class Gameplay : WikiSection
    {
        public override string Title => "Gameplay";

        [BackgroundDependencyLoader]
        private void load()
        {
            Content.Add(new WikiSubSectionHeader("Shapes"));
            Content.Add(new WikiParagraph("Shape has four Hitobject currently implemented (Circles, Xs, Squares and Triangles). " +
                "Each is bound to a Hitsound type:\n\n" +
                "Circle - Normal\n" +
                "X - Clap\n" +
                "Square - Whistle\n" +
                "Triangle - Finish"));
        }
    }
}