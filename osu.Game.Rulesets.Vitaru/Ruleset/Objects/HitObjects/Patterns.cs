using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects
{
    public class Patterns
    {
        public static List<Projectile> Wave(double speed, double diameter, double damage, Vector2 position, double startTime, int team, double complexity = 1, double angle = Math.PI / 2)
        {
            List<Projectile> projectiles = new List<Projectile>();

            int bulletCount = (int)(complexity * 5);
            double directionModifier = Math.PI / 2 / (bulletCount - 1);
            double direction = angle - Math.PI / 4;

            for (int i = 1; i <= bulletCount; i++)
            {
                projectiles.Add(new Bullet
                {
                    PatternStartTime = startTime,
                    Position = position,
                    Speed = (float)speed,
                    Angle = direction,
                    SliderType = SliderType.Straight,
                    //SpeedEasing = Easing.OutSine,
                    Diameter = i % 2 == 1 ? (float)diameter : (float)diameter * 1.5f,
                    Damage = i % 2 == 1 ? (float)damage : (float)damage * 0.8f,
                    Team = team,
                });
                direction += directionModifier;
            }

            return projectiles;
        }

        public static List<Projectile> Line(double startSpeed, double endSpeed, double diameter, double damage, Vector2 position, double startTime, int team, double complexity = 1, double angle = Math.PI / 2)
        {
            List<Projectile> projectiles = new List<Projectile>();

            int bulletCount = (int)(complexity * 3);
            double speedModifier = (endSpeed - startSpeed) / bulletCount;
            double speed = startSpeed;

            for (int i = 1; i <= bulletCount; i++)
            {
                projectiles.Add(new Bullet
                {
                    PatternStartTime = startTime,
                    Position = position,
                    Speed = (float)speed,
                    Angle = angle,
                    SliderType = SliderType.Straight,
                    SpeedEasing = Easing.OutQuad,
                    Diameter = (float)diameter,
                    Damage = (float)damage,
                    Team = team,
                });
                speed += speedModifier;
            }

            return projectiles;
        }

        public static List<Projectile> Triangle(double speed, double diameter, double damage, Vector2 position, double startTime, int team, double complexity = 1, double angle = Math.PI / 2)
        {
            List<Projectile> projectiles = new List<Projectile>();

            int bulletCount = (int)(complexity * 3);

            if (bulletCount % 3 != 0)
                bulletCount++;
            if (bulletCount % 3 != 0)
                bulletCount++;

            double directionModifier = Math.PI / 4 / (bulletCount - 1);
            double direction = angle;

            for (int i = 1; i <= bulletCount; i++)
            {
                projectiles.Add(new Bullet
                {
                    PatternStartTime = startTime,
                    Position = position,
                    Speed = (float)speed,
                    Angle = direction,
                    SliderType = SliderType.Straight,
                    SpeedEasing = Easing.OutQuad,
                    Diameter = (float)diameter,
                    Damage = (float)damage,
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

        public static List<Projectile> Wedge(double speed, double diameter, double damage, Vector2 position, double startTime, int team, double complexity = 1, double angle = Math.PI / 2)
        {
            List<Projectile> projectiles = new List<Projectile>();

            int bulletCount = (int)(complexity * 7);

            if (bulletCount % 2 == 0)
                bulletCount++;

            double directionModifier = Math.PI / 2 / (bulletCount - 1);
            double direction = angle - Math.PI / 4;

            double speedModifier = (speed * 1.5f - speed * 0.75f) / bulletCount;

            for (int i = 1; i <= bulletCount; i++)
            {
                projectiles.Add(new Bullet
                {
                    PatternStartTime = startTime,
                    Position = position,
                    Speed = (float)speed,
                    Angle = i % 2 == 0 ? angle - direction : angle + direction,
                    SliderType = SliderType.Straight,
                    SpeedEasing = Easing.OutSine,
                    Diameter = (float)diameter,
                    Damage = (float)damage,
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

        public static List<Projectile> Circle(double speed, double diameter, double damage, Vector2 position, double startTime, int team, double complexity = 1)
        {
            List<Projectile> projectiles = new List<Projectile>();

            int bulletCount = (int)(complexity * 12);
            double directionModifier = Math.PI * 2 / bulletCount;
            double direction = Math.PI / 2;

            for (int i = 1; i <= bulletCount; i++)
            {
                projectiles.Add(new Bullet
                {
                    PatternStartTime = startTime,
                    Position = position,
                    Speed = (float)speed,
                    Angle = direction,
                    SliderType = SliderType.Straight,
                    SpeedEasing = Easing.OutCubic,
                    Diameter = i % 2 == 1 ? (float)diameter : (float)diameter * 1.5f,
                    Damage = i % 2 == 1 ? (float)damage : (float)damage * 0.8f,
                    Team = team,
                });
                direction += directionModifier;
            }

            return projectiles;
        }

        public static List<Projectile> Cross(double speed, double diameter, double damage, double startTime, int team, double complexity = 1)
        {
            List<Projectile> projectiles = new List<Projectile>();

            int bulletCount = (int)(complexity * 4);
            double directionModifier = Math.PI * 2 / bulletCount;
            double direction = Math.PI / 4;

            for (int i = 1; i <= bulletCount; i++)
            {
                Vector2 offset = new Vector2((float)Math.Cos(direction) * -200, (float)Math.Sin(direction) * -200);

                projectiles.Add(new Bullet
                {
                    ObeyBoundries = false,
                    ShootPlayer = true,
                    PatternStartTime = startTime,
                    Position = offset,
                    Speed = (float)speed,
                    Angle = direction,
                    SliderType = SliderType.Target,
                    SpeedEasing = Easing.InOutExpo,
                    Diameter = diameter,
                    Damage = damage,
                    Team = team,
                });
                direction += directionModifier;
            }

            return projectiles;
        }

        public static List<Projectile> Flower(double speed, double diameter, double damage, Vector2 position, double startTime, double duration, int team, double beatLength = 500, double complexity = 1, int arms = 16)
        {
            List<Projectile> projectiles = new List<Projectile>();

            double direction = 0;

            for (int i = 1; i <= 4; i++)
            {
                projectiles.Add(new Laser
                {
                    StartTime = startTime,
                    EndTime = startTime + duration,
                    Position = position,
                    Angle = 90 * i + 45,
                    Size = new Vector2((float)diameter, (float)diameter * 32),
                    Damage = (float)damage * 2,
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
                        Speed = (float)speed,
                        Angle = direction,
                        Diameter = (float)diameter,
                        Damage = (float)damage,
                        SpeedEasing = Easing.OutCubic,
                        SliderType = type,
                        Curviness = 2,
                        Team = team,
                    });

                    if (i % 2 == 0)
                        direction += Math.PI / (arms / 4f);

                }
                direction += Math.PI / (arms / 2f);
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
