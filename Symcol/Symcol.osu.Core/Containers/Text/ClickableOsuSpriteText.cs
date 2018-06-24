using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Game.Graphics.Sprites;
using OpenTK.Graphics;
using Symcol.osu.Core.Wiki.OverlayPieces;

namespace Symcol.osu.Core.Containers.Text
{
    public class ClickableOsuSpriteText : OsuSpriteText, IHasTooltip
    {
        public string TooltipText => Tooltip;

        public string Tooltip = "";

        public new Color4 Colour
        {
            get => HoverContainer.IdleColour;
            set => HoverContainer.IdleColour = value;
        }

        public Action Action
        {
            get { return HoverContainer.Action; }
            set { HoverContainer.Action = value; }
        }

        protected readonly PaintableHoverContainer HoverContainer;

        public override bool HandleKeyboardInput => HoverContainer.Action != null;
        public override bool HandleMouseInput => HoverContainer.Action != null;

        protected override Container<Drawable> Content => HoverContainer ?? (Container<Drawable>)this;

        public override IEnumerable<Drawable> FlowingChildren => Children;

        public ClickableOsuSpriteText()
        {
            base.Colour = Color4.White;

            AddInternal(HoverContainer = new PaintableHoverContainer
            {
                AutoSizeAxes = Axes.Both,
            });
        }
    }
}
