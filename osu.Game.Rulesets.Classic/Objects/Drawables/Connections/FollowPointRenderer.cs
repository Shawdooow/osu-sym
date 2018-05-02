﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using OpenTK;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Types;

namespace osu.Game.Rulesets.Classic.Objects.Drawables.Connections
{
    public class FollowPointRenderer : ConnectionRenderer<ClassicHitObject>
    {
        private int pointDistance = 32;
        /// <summary>
        /// Determines how much space there is between points.
        /// </summary>
        public int PointDistance
        {
            get { return pointDistance; }
            set
            {
                if (pointDistance == value) return;
                pointDistance = value;
                update();
            }
        }

        private int preEmpt = 800;
        /// <summary>
        /// Follow points to the next hitobject start appearing for this many milliseconds before an hitobject's end time.
        /// </summary>
        public int PreEmpt
        {
            get { return preEmpt; }
            set
            {
                if (preEmpt == value) return;
                preEmpt = value;
                update();
            }
        }

        private IEnumerable<ClassicHitObject> hitObjects;
        public override IEnumerable<ClassicHitObject> HitObjects
        {
            get { return hitObjects; }
            set
            {
                hitObjects = value;
                update();
            }
        }

        private void update()
        {
            Clear();
            if (hitObjects == null)
                return;

            ClassicHitObject prevHitObject = null;
            foreach (var currHitObject in hitObjects)
            {
                if (prevHitObject != null && !currHitObject.NewCombo && !(prevHitObject is Spinner) && !(currHitObject is Spinner))
                {
                    Vector2 startPosition = prevHitObject.EndPosition;
                    Vector2 endPosition = currHitObject.Position;
                    double startTime = (prevHitObject as IHasEndTime)?.EndTime ?? prevHitObject.StartTime;
                    double endTime = currHitObject.StartTime;

                    Vector2 distanceVector = endPosition - startPosition;
                    int distance = (int)distanceVector.Length;
                    float rotation = MathHelper.RadiansToDegrees((float)Math.Atan2(currHitObject.Position.Y - prevHitObject.Position.Y, currHitObject.Position.X - prevHitObject.Position.X));
                    if (prevHitObject is Slider)
                    {
                        Slider slider = prevHitObject as Slider;
                        rotation = MathHelper.RadiansToDegrees((float)Math.Atan2(currHitObject.Position.Y - slider.EndPosition.Y, currHitObject.Position.X - slider.EndPosition.X));
                    }
                    double duration = endTime - startTime;

                    for (int d = (int)(PointDistance * 1.5); d < distance - PointDistance; d += PointDistance)
                    {
                        float fraction = (float)d / distance;
                        Vector2 pointStartPosition = startPosition + (fraction - 0.1f) * distanceVector;
                        Vector2 pointEndPosition = startPosition + fraction * distanceVector;
                        double fadeOutTime = startTime + fraction * duration;
                        double fadeInTime = fadeOutTime - PreEmpt;

                        FollowPoint fp;

                        Add(fp = new FollowPoint
                        {
                            Position = pointStartPosition,
                            Rotation = rotation,
                            Alpha = 0,
                            Scale = new Vector2(1.5f),
                        });

                        using (fp.BeginAbsoluteSequence(fadeInTime))
                        {
                            fp.FadeIn(currHitObject.TimeFadein);
                            fp.ScaleTo(1, currHitObject.TimeFadein, Easing.Out);

                            fp.MoveTo(pointEndPosition, currHitObject.TimeFadein, Easing.Out);

                            fp.Delay(fadeOutTime - fadeInTime).FadeOut(currHitObject.TimeFadein);
                        }

                        fp.Expire(true);
                    }
                }
                prevHitObject = currHitObject;
            }
        }
    }
}