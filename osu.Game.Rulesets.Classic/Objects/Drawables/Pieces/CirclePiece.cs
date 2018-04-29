// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Bindings;
using osu.Framework.Platform;
using osu.Game.Rulesets.Classic.Settings;
using osu.Game.Rulesets.Classic.UI;
using OpenTK;

namespace osu.Game.Rulesets.Classic.Objects.Drawables.Pieces
{
    public class CirclePiece : Container, IKeyBindingHandler<ClassicAction>
    {
        private readonly Bindable<string> skin = ClassicSettings.ClassicConfigManager.GetBindable<string>(ClassicSetting.Skin);

        private readonly Sprite disc;

        public Func<bool> Hit;

        public CirclePiece()
        {
            AlwaysPresent = true;

            Size = new Vector2((float)ClassicHitObject.OBJECT_RADIUS * 2);
            Masking = true;
            CornerRadius = Size.X / 2;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Children = new Drawable[]
            {
                disc = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            disc.Texture = ClassicSkinElement.LoadSkinElement(@"hitcircle", storage);
        }

        public bool OnPressed(ClassicAction action)
        {
            switch (action)
            {
                case ClassicAction.LeftButton:
                case ClassicAction.RightButton:
                    return IsHovered && (Hit?.Invoke() ?? false);

            }
            return false;
        }

        public bool OnReleased(ClassicAction action) => false;
    }
}
