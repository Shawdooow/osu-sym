// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using OpenTK;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Game.Rulesets.Classic.UI;

namespace osu.Game.Rulesets.Classic.Objects.Drawables.Pieces
{
    public class RingPiece : Container
    {
        public override bool HandleMouseInput => false;
        public override bool HandleKeyboardInput => false;

        private readonly Sprite ring;

        public RingPiece()
        {
            Size = new Vector2(128);

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Children = new Drawable[]
            {
                ring = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
                ring.Texture = ClassicSkinElement.LoadSkinElement(@"hitcircleoverlay", storage);
        }
    }
}
