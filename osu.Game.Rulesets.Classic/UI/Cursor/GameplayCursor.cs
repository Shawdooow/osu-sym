// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Bindings;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using OpenTK;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Game.Rulesets.Classic.Settings;

namespace osu.Game.Rulesets.Classic.UI.Cursor
{
    public class GameplayCursor : CursorContainer, IKeyBindingHandler<ClassicAction>
    {
        protected override Drawable CreateCursor() => new ClassicCursor();

        public static Vector2 CursorPosition;

        public GameplayCursor()
        {
            Add(new CursorTrail { Depth = 1 });
        }

        private int downCount;

        public class ClassicCursor : Container
        {
            private Bindable<double> cursorScale;
            private Bindable<bool> autoCursorScale;
            private Bindable<WorkingBeatmap> beatmap;

            public ClassicCursor()
            {
                Origin = Anchor.Centre;
                Size = new Vector2(42);
            }

            private Sprite cursor;
            private Sprite cursorMiddle;

            [BackgroundDependencyLoader]
            private void load(OsuConfigManager config, OsuGameBase game, Storage storage)
            {
                Children = new Drawable[]
                {
                    cursor = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Depth = 0,
                    },
                    cursorMiddle = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Depth = -1,
                    }
                };

                cursorMiddle.Texture = ClassicSkinElement.LoadSkinElement(@"cursormiddle", storage);
                cursor.Texture = ClassicSkinElement.LoadSkinElement(@"cursor", storage);

                beatmap = game.Beatmap.GetBoundCopy();
                beatmap.ValueChanged += v => calculateScale();

                cursorScale = config.GetBindable<double>(OsuSetting.GameplayCursorSize);
                cursorScale.ValueChanged += v => calculateScale();

                autoCursorScale = config.GetBindable<bool>(OsuSetting.AutoCursorSize);
                autoCursorScale.ValueChanged += v => calculateScale();

                calculateScale();
            }

            protected override void Update()
            {
                base.Update();
                cursor.RotateTo((float)((Clock.CurrentTime / 1000) * 90) / 2);
                CursorPosition = Position;
            }

            private void calculateScale()
            {
                float scale = (float)cursorScale.Value;

                if (autoCursorScale && beatmap.Value != null)
                {
                    // if we have a beatmap available, let's get its circle size to figure out an automatic cursor scale modifier.
                    scale *= (float)(1 - 0.7 * (1 + beatmap.Value.BeatmapInfo.BaseDifficulty.CircleSize - BeatmapDifficulty.DEFAULT_DIFFICULTY) / BeatmapDifficulty.DEFAULT_DIFFICULTY);
                }

                cursor.Scale = new Vector2(scale);
                cursorMiddle.Scale = new Vector2(scale);
            }
        }

        public bool OnPressed(ClassicAction action)
        {
            switch (action)
            {
                case ClassicAction.LeftButton:
                case ClassicAction.RightButton:
                    downCount++;
                    ActiveCursor.ScaleTo(1).ScaleTo(1.2f, 100, Easing.OutQuad);
                    break;
            }

            return false;
        }

        public bool OnReleased(ClassicAction action)
        {
            switch (action)
            {
                case ClassicAction.LeftButton:
                case ClassicAction.RightButton:
                    if (--downCount == 0)
                        ActiveCursor.ScaleTo(1, 200, Easing.OutQuad);
                    break;
            }

            return false;
        }
    }
}
