﻿using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using Symcol.Core.Graphics.Containers;

namespace Symcol.Core.GameObjects
{
    public class SymcolHitbox : SymcolContainer
    {
        /// <summary>
        /// whether we want to do hit detection
        /// </summary>
        public int Team { get; set; }

        /// <summary>
        /// whether we want to do hit detection
        /// </summary>
        public bool HitDetection { get; set; } = true;

        /// <summary>
        /// the shape of this object (used for hit detection)
        /// </summary>
        public Shape Shape
        {
            get => shape;
            set
            {
                if (value != shape)
                {
                    shape = value;

                    if (value == Shape.Circle)
                        Child = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            CornerRadius = Width / 2
                        };
                    else if (value == Shape.Rectangle)
                        Child = new Container
                        {
                            RelativeSizeAxes = Axes.Both
                        };
                }
            }

        }

        private Shape shape = Shape.Complex;

        public SymcolHitbox(Shape shape = Shape.Circle)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Shape = shape;
        }

        public bool HitDetect(SymcolHitbox hitbox1, SymcolHitbox hitbox2)
        {
            if (hitbox1.HitDetection && hitbox2.HitDetection && hitbox1.Team != hitbox2.Team)
            {
                if (hitbox1.Shape == Shape.Circle && hitbox2.Shape == Shape.Circle)
                {
                    if (hitbox1.ScreenSpaceDrawQuad.AABB.IntersectsWith(hitbox2.ScreenSpaceDrawQuad.AABB))
                        return true;
                }
                else if (hitbox1.Shape == Shape.Circle && hitbox2.Shape == Shape.Rectangle || hitbox1.Shape == Shape.Rectangle && hitbox2.Shape == Shape.Circle)
                {
                    if (hitbox1.ScreenSpaceDrawQuad.AABB.IntersectsWith(hitbox2.ScreenSpaceDrawQuad.AABB))
                        return true;
                }
                else if (hitbox1.Shape == Shape.Rectangle && hitbox2.Shape == Shape.Rectangle)
                {
                    if (hitbox1.ScreenSpaceDrawQuad.AABB.IntersectsWith(hitbox2.ScreenSpaceDrawQuad.AABB))
                        return true;
                }
                else if (hitbox1.Shape == Shape.Complex || hitbox2.Shape == Shape.Complex)
                    foreach (Drawable drawable in hitbox1.Children)
                    {
                        var child1 = (Container)drawable;
                        foreach (Drawable drawable1 in hitbox2.Children)
                        {
                            var child2 = (Container)drawable1;
                            if (child1.ScreenSpaceDrawQuad.AABB.IntersectsWith(child2.ScreenSpaceDrawQuad.AABB))
                                return true;
                        }
                    }
            }
            return false;
        }
    }

    public enum Shape
    {
        Circle,
        Rectangle,
        Complex
    }
}
