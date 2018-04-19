﻿using OpenTK;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public class UsablePatterns
    {
        public static List<Bullet> Wave(double speed, double diameter, double damage, Vector2 position, double startTime, double complexity = 1, double angle = Math.PI / 2, int team = 1)
        {
            List<Bullet> bullets = new List<Bullet>();

            int bulletCount = (int)(complexity * 5);
            double directionModifier = ((Math.PI / 2) / (bulletCount - 1));
            double direction = angle - Math.PI / 4;

            for (int i = 1; i <= bulletCount; i++)
            {
                bullets.Add(new Bullet
                {
                    StartTime = startTime,
                    Position = position,
                    BulletSpeed = (float)speed,
                    BulletAngle = direction,
                    BulletDiameter = (float)diameter,
                    BulletDamage = (float)damage,
                    Team = team,
                });
                direction += directionModifier;
            }

            return bullets;
        }

        public static List<Bullet> Line(double startSpeed, double endSpeed, double diameter, double damage, Vector2 position, double startTime, double complexity = 1, double angle = Math.PI / 2, int team = 1)
        {
            List<Bullet> bullets = new List<Bullet>();

            int bulletCount = (int)(complexity * 3);
            double speedModifier = (endSpeed - startSpeed) / bulletCount;
            double speed = startSpeed;

            for (int i = 1; i <= bulletCount; i++)
            {
                bullets.Add(new Bullet
                {
                    StartTime = startTime,
                    Position = position,
                    BulletSpeed = (float)speed,
                    BulletAngle = angle,
                    BulletDiameter = (float)diameter,
                    BulletDamage = (float)damage,
                    Team = team,
                });
                speed += speedModifier;
            }

            return bullets;
        }

        public static List<Bullet> Triangle(double speed, double diameter, double damage, Vector2 position, double startTime, double complexity = 1, double angle = Math.PI / 2, int team = 1)
        {
            List<Bullet> bullets = new List<Bullet>();

            int bulletCount = (int)(complexity * 3);

            if (bulletCount % 3 != 0)
                bulletCount++;
            if (bulletCount % 3 != 0)
                bulletCount++;

            double directionModifier = ((Math.PI / 4) / (bulletCount - 1));
            double direction = angle;

            for (int i = 1; i <= bulletCount; i++)
            {
                bullets.Add(new Bullet
                {
                    StartTime = startTime,
                    Position = position,
                    BulletSpeed = (float)speed,
                    BulletAngle = direction,
                    BulletDiameter = (float)diameter,
                    BulletDamage = (float)damage,
                    Team = team,
                });
                direction += directionModifier;

                if (i == 1)
                {
                    speed *= 0.9f;
                    direction -= directionModifier + directionModifier / 2;
                }
                if (i == 3)
                {
                    speed *= 0.92f;
                    direction -= directionModifier * 3 + directionModifier / 2;
                }
            }

            return bullets;
        }

        public static List<Bullet> Wedge(double speed, double diameter, double damage, Vector2 position, double startTime, double complexity = 1, double angle = Math.PI / 2, int team = 1)
        {
            List<Bullet> bullets = new List<Bullet>();

            int bulletCount = (int)(complexity * 7);
            int halfOfBullets;

            if (bulletCount % 2 == 0)
            {
                halfOfBullets = bulletCount / 2;
                bulletCount++;
            }
            else
                halfOfBullets = (bulletCount - 1) / 2;

            double directionModifier = ((Math.PI / 2) / (bulletCount - 1));
            double directionModifierModifier = (directionModifier / halfOfBullets);
            double direction = angle - Math.PI / 4;

            double speedModifier = ((speed * 1.5f) - (speed * 0.75f)) / bulletCount;

            speed *= 0.8f;

            for (int i = 1; i <= bulletCount; i++)
            {
                bullets.Add(new Bullet
                {
                    StartTime = startTime,
                    Position = position,
                    BulletSpeed = (float)speed,
                    BulletAngle = direction,
                    BulletDiameter = (float)diameter,
                    BulletDamage = (float)damage,
                    Team = team,
                });

                if (i <= halfOfBullets)
                {
                    speed += speedModifier;
                    direction += directionModifier;
                    directionModifier -= directionModifierModifier;
                }
                else
                {
                    speed -= speedModifier;
                    direction += directionModifier;
                    directionModifier += directionModifierModifier;
                }
            }

            return bullets;
        }

        public static List<Bullet> Circle(double speed, double diameter, double damage, Vector2 position, double startTime, double complexity = 1, int team = 1)
        {
            List<Bullet> bullets = new List<Bullet>();

            int bulletCount = (int)(complexity * 8);
            double directionModifier = ((Math.PI * 2) / bulletCount);
            double direction = Math.PI / 2;

            for (int i = 1; i <= bulletCount; i++)
            {
                bullets.Add(new Bullet
                {
                    StartTime = startTime,
                    Position = position,
                    BulletSpeed = (float)speed,
                    BulletAngle = direction,
                    BulletDiameter = (float)diameter,
                    BulletDamage = (float)damage,
                    Team = team,
                });
                direction += directionModifier;
            }

            return bullets;
        }

        public static List<Bullet> Flower(double speed, double diameter, double damage, Vector2 position, double startTime, double duration, double beatLength = 500, double complexity = 1, int team = 1, int arms = 8)
        {
            List<Bullet> bullets = new List<Bullet>();

            double direction = 0;

            SliderType type = SliderType.CurveRight;

            for (double j = startTime; j <= startTime + duration; j += beatLength / 2)
            {
                for (int i = 1; i <= arms; i++)
                {
                    if (i % 2 == 0)
                        type = SliderType.CurveLeft;
                    else
                        type = SliderType.CurveRight;

                    bullets.Add(new Bullet
                    {
                        StartTime = j,
                        Position = position,
                        BulletSpeed = (float)speed,
                        BulletAngle = direction,
                        BulletDiameter = (float)diameter,
                        BulletDamage = (float)damage,
                        SliderType = type,
                        Curviness = 2,
                        Team = team,
                    });

                    if (i % 2 == 0)
                        direction += Math.PI / 2;

                }
                direction += 0.3d;
            }

            return bullets;
        }
    }
}