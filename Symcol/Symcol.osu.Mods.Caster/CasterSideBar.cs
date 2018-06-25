using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using OpenTK.Graphics;
using Symcol.Core.Graphics.Containers;

namespace Symcol.osu.Mods.Caster
{
    public class CasterSideBar : SymcolContainer
    {
        public CasterSideBar()
        {
            RelativeSizeAxes = Axes.Both;
            Width = 0.18f;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f,
                },
            };
        }
    }
}
