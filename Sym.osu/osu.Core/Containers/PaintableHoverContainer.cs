﻿#region usings

using osu.Game.Graphics.Containers;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Core.Containers
{
    public class PaintableHoverContainer : OsuHoverContainer
    {
        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => Action != null && base.ReceivePositionalInputAt(screenSpacePos); 

        public new Color4 IdleColour
        {
            get => base.IdleColour;
            set
            {
                idle = value;
                base.IdleColour = value;
            }
        }

        public new Color4 HoverColour
        {
            get => base.HoverColour;
            set
            {
                hover = value;
                base.HoverColour = value;
            }
        }

        //Fight the BackgroundDependencyLoader!
        protected override void LoadComplete()
        {
            base.LoadComplete();
            base.HoverColour = hover;
            base.IdleColour = idle;
        }

        private Color4 hover, idle;
    }
}
