#region usings

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;
using Sym.Base.Game;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay
{
    public class VitaruHitbox : Hitbox
    {
        public VitaruHitbox(Shape shape = Shape.Circle)
            : base(shape)
        {
        }

        public override bool HitDetect(Hitbox hitbox1, Hitbox hitbox2 = null)
        {
            if (hitbox2 == null)
                hitbox2 = this;

            if (hitbox1.HitDetection && hitbox2.HitDetection)
            {
                if (hitbox1.Shape == Shape.Circle && hitbox2.Shape == Shape.Circle)
                {
                    Vector2 pos = hitbox2.ToSpaceOfOtherDrawable(Vector2.Zero, hitbox1);
                    //Oddly this seems to help but not always
                    pos += new Vector2(hitbox1.Width / 4 + hitbox2.Width / 4);

                    double distance = Math.Sqrt(Math.Pow(pos.X, 2) + Math.Pow(pos.Y, 2));
                    double edgeDistance = distance - (hitbox1.Width / 2 + hitbox2.Width / 2);

                    if (edgeDistance <= 0)
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
                        Container child1 = (Container)drawable;
                        foreach (Drawable drawable1 in hitbox2.Children)
                        {
                            Container child2 = (Container)drawable1;
                            if (child1.ScreenSpaceDrawQuad.AABB.IntersectsWith(child2.ScreenSpaceDrawQuad.AABB))
                                return true;
                        }
                    }
            }
            return false;
        }
    }
}
