using osu.Framework.Allocation;
using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using System;

namespace osu.Game.Rulesets.Classic.UI
{
    public class ClassicUi : Container
    {
        private Container scoreBar;
        private Sprite scoreBarBG;
        private Container scoreBarBar;
        private Sprite scoreBarBarSprite;
        private Sprite scoreBarMarker;

        public static double BreakStartTime = -10000;

        public static double CurrentHealth = 1;

        public ClassicUi()
        {
            AlwaysPresent = true;
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            CurrentHealth = 1;

            Children = new Drawable[]
            {
                scoreBar = new Container
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,

                    Children = new Drawable[]
                    {
                        scoreBarBG = new Sprite
                        {
                            Depth = 1,
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Texture = ClassicSkinElement.LoadSkinElement("scorebar-bg", storage)
                        },
                        scoreBarBar = new Container
                        {
                            Position = new Vector2(12 , 13),
                            Depth = 0,
                            Masking = true,
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Children = new Drawable[]
                            {
                                scoreBarBarSprite = new Sprite
                                {
                                    Texture = ClassicSkinElement.LoadSkinElement("scorebar-colour", storage)
                                }
                            }
                        },
                        scoreBarMarker = new Sprite
                        {
                            Depth = -1,
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.Centre,
                            Texture = ClassicSkinElement.LoadSkinElement("scorebar-marker", storage),
                            Position = new Vector2(50, 13 + 4),
                        }
                    }
                }
            };
            scoreBarBar.Size = scoreBarBarSprite.Size;
        }

        protected override void Update()
        {
            base.Update();

            float drainAmount = 10;

            if (CurrentHealth > 1)
                CurrentHealth = 1;

            if (BreakStartTime > Time.Current)
                CurrentHealth = Math.Max(CurrentHealth - ((float)Clock.ElapsedFrameTime / (1000 * drainAmount)), 0);

            scoreBarBar.ResizeTo(new Vector2(scoreBarBarSprite.Size.X * (float)CurrentHealth, scoreBarBarSprite.Size.Y));
            scoreBarMarker.MoveTo(new Vector2(scoreBarBarSprite.Size.X * (float)CurrentHealth + 10, scoreBarMarker.Position.Y));
        }
    }
}
