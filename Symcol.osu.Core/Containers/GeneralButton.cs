using System;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input;
using OpenTK;

namespace Symcol.osu.Core.Containers
{
    public class GeneralButton : Sprite
    {
        public Action Action;
        public override bool ReceiveMouseInputAt(Vector2 screenSpacePos) => this.ReceiveMouseInputAt(screenSpacePos);

        protected override bool OnMouseDown(InputState state, MouseDownEventArgs args)
        {
            Action();
            return true;
        }
    }
}
