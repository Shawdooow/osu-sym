using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using System;

namespace osu.Game.Screens.Symcol.CasterBible.Pieces
{
    public class UTCClock : Container
    {
        public int TextSize
        {
            get { return textSize;  }
            set
            {
                if (value != textSize)
                {
                    textSize = value;

                    hour.TextSize = value;
                    minutes.TextSize = value;
                    seconds.TextSize = value;
                }
            }
        }

        private int textSize = 20;

        private OsuSpriteText hour;
        private OsuSpriteText minutes;
        private OsuSpriteText seconds;

        public UTCClock()
        {
            Origin = Anchor.Centre;

            Children = new Drawable[]
            {
                hour = new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Position = new Vector2(-28, 0),

                    TextSize = textSize,
                    Text = "66"
                },
                new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Position = new Vector2(-14, 0),

                    TextSize = textSize,
                    Text = ":"
                },
                minutes = new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,

                    TextSize = textSize,
                    Text = "66"
                },
                new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Position = new Vector2(14, 0),

                    TextSize = textSize,
                    Text = ":"
                },
                seconds = new OsuSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Position = new Vector2(28, 0),

                    TextSize = textSize,
                    Text = "66"
                }
            };
        }

        protected override void Update()
        {
            base.Update();

            DateTime time = DateTime.UtcNow;

            hour.Text = time.Hour.ToString();
            minutes.Text = time.Minute.ToString();
            seconds.Text = time.Second.ToString();
        }
    }
}
