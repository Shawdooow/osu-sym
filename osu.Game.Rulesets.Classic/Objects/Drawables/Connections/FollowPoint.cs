// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Game.Rulesets.Classic.UI;

namespace osu.Game.Rulesets.Classic.Objects.Drawables.Connections
{
    public class FollowPoint : Sprite
    {
        public override bool HandleMouseInput => false;
        public override bool HandleKeyboardInput => false;

        private const float width = 8;

        public FollowPoint()
        {
            Origin = Anchor.Centre;
            //FillMode = FillMode.Fill;
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            Texture = ClassicSkinElement.LoadSkinElement("followpoint", storage);
        }
    }
}
