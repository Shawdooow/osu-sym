﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;

namespace Symcol.osu.Mods.Evast.CursorTracers
{
    public class CrossCursorTracer : Container, IRequireHighFrequencyMousePosition
    {
        private const int thickness = 3;

        private double delay;
        public double Delay
        {
            set { delay = value; }
            get { return delay; }
        }

        private readonly Container horizontal;
        private readonly Container vertical;

        public CrossCursorTracer()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                horizontal = new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Height = thickness,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                },
                vertical = new Container
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = thickness,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                }
            };
        }

        protected override bool OnMouseMove(InputState state)
        {
            horizontal.MoveToY(state.Mouse.Position.Y, delay, Easing.Out);
            vertical.MoveToX(state.Mouse.Position.X, delay, Easing.Out);

            return base.OnMouseMove(state);
        }
    }
}