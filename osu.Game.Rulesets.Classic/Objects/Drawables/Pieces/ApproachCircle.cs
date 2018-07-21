// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Game.Rulesets.Classic.UI;

namespace osu.Game.Rulesets.Classic.Objects.Drawables.Pieces
{
    public class ApproachCircle : Container
    {
        public override bool HandleMouseInput => false;
        public override bool HandleKeyboardInput => false;

        private readonly Sprite approachCircle;

        public ApproachCircle()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            AutoSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                approachCircle = new Sprite()
            };
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            approachCircle.Texture = ClassicSkinElement.LoadSkinElement("approachcircle", storage);
        }
    }
}
