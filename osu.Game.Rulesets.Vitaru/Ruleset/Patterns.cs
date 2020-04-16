#region usings

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osuTK;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset
{
    public static class Patterns
    {
        public static List<Projectile> Wave(float speed, float diameter, float damage, Vector2 position, double startTime, int team, float complexity = 1, float angle = (float)Math.PI / 2)
        {
            List<Projectile> projectiles = new List<Projectile>();

            int bulletCount = (int)(complexity * 5);
            float directionModifier = (float)Math.PI / 2 / (bulletCount - 1);
            float direction = angle - (float)Math.PI / 4;

            for (int i = 1; i <= bulletCount; i++)
            {
                projectiles.Add(new Bullet
                {
                    PatternStartTime = startTime,
                    Position = position,
                    Speed = speed,
                    Angle = direction,
                    Distance = 1000,
                    SliderType = SliderType.Straight,
                    //SpeedEasing = Easing.OutSine,
                    Diameter = i % 2 == 1 ? diameter : diameter * 1.5f,
                    Damage = i % 2 == 1 ? damage : damage * 0.8f,
                    Team = team,
                });
                direction += directionModifier;
            }

            return projectiles;
        }

        public static List<Projectile> Line(float startSpeed, float endSpeed, float diameter, float damage, Vector2 position, double startTime, int team, float complexity = 1, float angle = (float)Math.PI / 2)
        {
            List<Projectile> projectiles = new List<Projectile>();

            int bulletCount = (int)(complexity * 3);
            float speedModifier = (endSpeed - startSpeed) / bulletCount;
            float speed = startSpeed;

            for (int i = 1; i <= bulletCount; i++)
            {
                projectiles.Add(new Bullet
                {
                    PatternStartTime = startTime,
                    Position = position,
                    Speed = speed,
                    Angle = angle,
                    Distance = 1000,
                    SliderType = SliderType.Straight,
                    SpeedEasing = Easing.OutQuad,
                    Diameter = diameter,
                    Damage = damage,
                    Team = team,
                });
                speed += speedModifier;
            }

            return projectiles;
        }

        public static List<Projectile> Triangle(float speed, float diameter, float damage, Vector2 position, double startTime, int team, float complexity = 1, float angle = (float)Math.PI / 2)
        {
            List<Projectile> projectiles = new List<Projectile>();

            int bulletCount = (int)(complexity * 3);

            if (bulletCount % 3 != 0)
                bulletCount++;
            if (bulletCount % 3 != 0)
                bulletCount++;

            float directionModifier = (float)Math.PI / 4 / (bulletCount - 1);
            float direction = angle;

            for (int i = 1; i <= bulletCount; i++)
            {
                projectiles.Add(new Bullet
                {
                    PatternStartTime = startTime,
                    Position = position,
                    Speed = speed,
                    Angle = direction,
                    Distance = 1000,
                    SliderType = SliderType.Straight,
                    SpeedEasing = Easing.OutQuad,
                    Diameter = diameter,
                    Damage = damage,
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

            return projectiles;
        }

        public static List<Projectile> Wedge(float speed, float diameter, float damage, Vector2 position, double startTime, int team, float complexity = 1, float angle = (float)Math.PI / 2)
        {
            List<Projectile> projectiles = new List<Projectile>();

            int bulletCount = (int)(complexity * 7);

            if (bulletCount % 2 == 0)
                bulletCount++;

            float directionModifier = (float)Math.PI / 2 / (bulletCount - 1);
            float direction = angle - (float)Math.PI / 4;

            float speedModifier = (speed * 1.5f - speed * 0.75f) / bulletCount;

            for (int i = 1; i <= bulletCount; i++)
            {
                projectiles.Add(new Bullet
                {
                    PatternStartTime = startTime,
                    Position = position,
                    Speed = speed,
                    Angle = i % 2 == 0 ? angle - direction : angle + direction,
                    Distance = 1000,
                    SliderType = SliderType.Straight,
                    SpeedEasing = Easing.OutSine,
                    Diameter = diameter,
                    Damage = damage,
                    Team = team,
                });

                if (i % 2 == 0)
                {
                    speed -= speedModifier;
                    direction -= directionModifier;
                }
            }

            return projectiles;
        }

        public static List<Projectile> Circle(float speed, float diameter, float damage, Vector2 position, double startTime, int team, float complexity = 1)
        {
            List<Projectile> projectiles = new List<Projectile>();

            int bulletCount = (int)(complexity * 12);
            float directionModifier = (float)Math.PI * 2 / bulletCount;
            float direction = (float)Math.PI / 2;

            for (int i = 1; i <= bulletCount; i++)
            {
                projectiles.Add(new Bullet
                {
                    PatternStartTime = startTime,
                    Position = position,
                    Speed = speed,
                    Angle = direction,
                    Distance = 1000,
                    SliderType = SliderType.Straight,
                    SpeedEasing = Easing.OutCubic,
                    Diameter = i % 2 == 1 ? diameter : diameter * 1.5f,
                    Damage = i % 2 == 1 ? damage : damage * 0.8f,
                    Team = team,
                });
                direction += directionModifier;
            }

            return projectiles;
        }

        public static List<Projectile> Swipe(float speed, float diameter, float damage, double startTime, int team, float complexity = 1)
        {
            List<Projectile> projectiles = new List<Projectile>();

            const float dist = 800;
            int bulletCount = (int)(complexity * 4);
            float directionModifier = (float)Math.PI / bulletCount;
            float direction = (float)Math.PI / 8f;

            for (int i = 1; i <= bulletCount; i++)
            {
                Vector2 offset = new Vector2((float)Math.Cos(direction) * (-dist / 2), (float)Math.Sin(direction) * (-dist / 2));

                projectiles.Add(new Bullet
                {
                    ObeyBoundries = false,
                    ShootPlayer = true,
                    Shape = Shape.Triangle,
                    PatternStartTime = startTime,
                    Position = offset,
                    Speed = speed,
                    Angle = direction,
                    Distance = dist,
                    SliderType = SliderType.Target,
                    SpeedEasing = Easing.InOutQuint,
                    Diameter = diameter,
                    Damage = damage,
                    Team = team,
                });
                direction += directionModifier;
            }

            return projectiles;
        }

        public static List<Projectile> Cross(float speed, float diameter, float damage, double startTime, int team, float complexity = 1)
        {
            List<Projectile> projectiles = new List<Projectile>();

            const float dist = 600;
            int bulletCount = (int)(complexity * 4);
            float directionModifier = (float)Math.PI * 2 / bulletCount;
            float direction = (float)Math.PI / 4;

            for (int i = 1; i <= bulletCount; i++)
            {
                Vector2 offset = new Vector2((float)Math.Cos(direction) * (-dist / 2), (float)Math.Sin(direction) * (-dist / 2));

                projectiles.Add(new Bullet
                {
                    ObeyBoundries = false,
                    ShootPlayer = true,
                    Shape = Shape.Triangle,
                    PatternStartTime = startTime,
                    Position = offset,
                    Speed = speed,
                    Angle = direction,
                    Distance = dist,
                    SliderType = SliderType.Target,
                    SpeedEasing = Easing.InOutQuint,
                    Diameter = diameter,
                    Damage = damage,
                    Team = team,
                });
                direction += directionModifier;
            }

            return projectiles;
        }

        public static List<Projectile> Flower(float speed, float diameter, float damage, Vector2 position, double startTime, double duration, int team, double beatLength = 500, float complexity = 1, int arms = 16)
        {
            List<Projectile> projectiles = new List<Projectile>();

            float direction = 0;

            for (int i = 1; i <= 4; i++)
            {
                projectiles.Add(new Laser
                {
                    StartTime = startTime,
                    EndTime = startTime + duration,
                    Position = position,
                    Angle = 90 * i + 45,
                    Size = new Vector2(diameter, diameter * 32),
                    Damage = damage * 2,
                    Team = team,
                });
            }

            for (double j = startTime; j <= startTime + duration; j += beatLength / 2)
            {
                for (int i = 1; i <= arms; i++)
                {
                    SliderType type = i % 2 == 0 ? SliderType.CurveLeft : SliderType.CurveRight;

                    projectiles.Add(new Bullet
                    {
                        PatternStartTime = j,
                        Position = position,
                        Speed = speed,
                        Angle = direction,
                        Diameter = diameter,
                        Damage = damage,
                        Distance = 600,
                        SpeedEasing = Easing.OutCubic,
                        SliderType = type,
                        Curviness = 2,
                        Team = team,
                    });

                    if (i % 2 == 0)
                        direction += (float)Math.PI / (arms / 4f);

                }
                direction += (float)Math.PI / (arms / 2f);
            }

            return projectiles;
        }
    }
}

/*
── █───▄▀█▀▀█▀▄▄───▐█──────▄▀█▀▀█▀▄▄
──█───▀─▐▌──▐▌─▀▀──▐█─────▀─▐▌──▐▌─█▀
─▐▌──────▀▄▄▀──────▐█▄▄──────▀▄▄▀──▐▌
─█────────────────────▀█────────────█
▐█─────────────────────█▌───────────█
▐█─────────────────────█▌───────────█
─█───────────────█▄───▄█────────────█
─▐▌───────────────▀███▀────────────▐▌
──█──────────▀▄───────────▄▀───────█
───█───────────▀▄▄▄▄▄▄▄▄▄▀────────█

             .  .
             |\_|\
             | a_a\
             | | "]
         ____| '-\___
        /.----.___.-'\
       //        _    \
      //   .-. (~v~) /|
     |'|  /\:  .--  / \
    // |-/  \_/____/\/~|
   |/  \ |  []_|_|_] \ |
   | \  | \ |___   _\ ]_}
   | |  '-' /   '.'  |
   | |     /    /|:  |
   | |     |   / |:  /\
   | |     /  /  |  /  \
   | |    |  /  /  |    \
   \ |    |/\/  |/|/\    \
    \|\ |\|  |  | / /\/\__\
     \ \| | /   | |__
         / |   |____)
          |_/

    ,------.
    `-____-'        ,-----------.
     ,i--i.         |           |
    / @  @ \       /  Woo Hoo!  |
   | -.__.- | ___-'             J
    \.    ,/ """"""""""""""""""'
    ,\""""/.
  ,'  `--'  `.
 (_,i'    `i._)
    |   o  |
    |  ,.  |
    | |  | |
    `-'  `-'

           .--'''''''''--.
        .'      .---.      '.
       /    .-----------.    \
      /        .-----.        \
      |       .-.   .-.       |
      |      /   \ /   \      |
       \    | .-. | .-. |    /
        '-._| | | | | | |_.-'
            | '-' | '-' |
             \___/ \___/
          _.-'  /   \  `-._
        .' _.--|     |--._ '.
        ' _...-|     |-..._ '
               |     |
               '.___.'
                 | |
                _| |_
               /\( )/\
              /  ` '  \
             | |     | |
             '-'     '-'
             | |     | |
             | |     | |
             | |-----| |
          .`/  |     | |/`.
          |    |     |    |
          '._.'| .-. |'._.'
                \ | /
                | | |
                | | |
                | | |
               /| | |\
             .'_| | |_`.
             `. | | | .'
          .    /  |  \    .
         /o`.-'  / \  `-.`o\
        /o  o\ .'   `. /o  o\
        `.___.'       `.___.'

                   __   __
                 .'  '.'  `.
              _.-|  o | o  |-._
            .~   `.__.'.__.'^  ~.
          .~     ^  /   \  ^     ~.
          \-._^   ^|     |    ^_.-/
          `\  `-._  \___/ ^_.-' /'
            `\_   `--...--'   /'
               `-.._______..-'      /\  /\
                  __/   \__         | |/ /_
                .'^   ^    `.      .'   `__\
              .'    ^     ^  `.__.'^ .\ \
             .' ^ .    ^   .    ^  .'  \/
            /    /        ^ \'.__.'
           |  ^ /|   ^      |
            \   \|^      ^  |  
             `\^ |        ^ |
               `~|    ^     |
                 |  ^     ^ |
                 \^         /
                  `.    ^ .'
             jgs   : ^    ; 
           .-~~~~~~   |  ^ ~~~~~~-.
          /   ^     ^ |    ^       \
          \^     ^   / \  ^     ^  /
           `~~~~~~~~'   `~~~~~~~~~'
*/
