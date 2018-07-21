// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Game.Rulesets.Classic.UI;
using OpenTK;

namespace osu.Game.Rulesets.Classic.Objects.Drawables.Pieces
{
    public class NumberPiece : Container
    {
        public override bool HandleMouseInput => false;
        public override bool HandleKeyboardInput => false;

        private readonly Sprite number;
        private int numberValue;

        public int Text
        {
            get { return numberValue; }
            set { numberValue = value; }
        }

        public NumberPiece()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Children = new Drawable[]
            {
                number = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1,
                    Scale = new Vector2(0.75f)
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            number.Texture = numberValue <= 9 ? ClassicSkinElement.LoadSkinElement(@"default-" + numberValue, storage) : ClassicSkinElement.LoadSkinElement(@"default-" + 0, storage);
        }
    }
}
