using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;

namespace Symcol.osu.Core.Containers.Text
{
    public class ClickableOsuSpriteText : OsuSpriteText, IHasTooltip
    {
        public string TooltipText => Tooltip;

        public virtual string Tooltip => "";

        public Action Action
        {
            get { return content.Action; }
            set { content.Action = value; }
        }

        private readonly OsuHoverContainer content;

        public override bool HandleKeyboardInput => content.Action != null;
        public override bool HandleMouseInput => content.Action != null;

        protected override Container<Drawable> Content => content ?? (Container<Drawable>)this;

        public override IEnumerable<Drawable> FlowingChildren => Children;

        public ClickableOsuSpriteText()
        {
            AddInternal(content = new OsuHoverContainer
            {
                AutoSizeAxes = Axes.Both,
            });
        }
    }
}
