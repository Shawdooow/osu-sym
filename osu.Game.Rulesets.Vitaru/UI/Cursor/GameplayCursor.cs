﻿using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using OpenTK;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Graphics;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers;

namespace osu.Game.Rulesets.Vitaru.UI.Cursor
{
    public class GameplayCursor : CursorContainer
    {
        protected override Drawable CreateCursor() => cursor;

        private readonly VitaruCursor cursor;

        public GameplayCursor(VitaruPlayfield vitaruPlayfield)
        {
            cursor = new VitaruCursor(vitaruPlayfield);
            Masking = false;
        }

        public class VitaruCursor : Container
        {
            private string selectedCharacter = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);
            private readonly Gamemodes currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);

            private readonly VitaruPlayfield vitaruPlayfield;

            private Container lineContainer;
            private Container circleContainer;
            public static CircularContainer CenterCircle;

            private Bindable<double> cursorScale;
            private Bindable<bool> autoCursorScale;
            private Bindable<WorkingBeatmap> beatmap;

            public VitaruCursor(VitaruPlayfield vitaruPlayfield)
            {
                this.vitaruPlayfield = vitaruPlayfield;

                Origin = Anchor.Centre;
                Size = new Vector2(32);
                Masking = false;
            }

            [BackgroundDependencyLoader]
            private void load(OsuConfigManager config, OsuGameBase game, OsuColour osu)
            {
                Children = new Drawable[]
                {
                    new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(Size.X + Size.X / 3.5f),
                        Texture = VitaruRuleset.VitaruTextures.Get("ring")
                    },
                    CenterCircle = new CircularContainer
                    {
                        Masking = true,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(Size.X / 5),
                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    },
                    lineContainer = new Container
                    {
                        Masking = false,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Rotation = 45,

                        Children = new Drawable[]
                        {
                            new Container
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                CornerRadius = Size.X / 12,
                                Size = new Vector2(Size.X / 3, Size.X / 7),
                                Position = new Vector2(Size.X / 3, 0),
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new Container
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                CornerRadius = Size.X / 12,
                                Size = new Vector2(Size.X / 3, Size.X / 7),
                                Position = new Vector2(-1 * Size.X / 3, 0),
                                Rotation = 180,
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new Container
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                CornerRadius = Size.X / 12,
                                Size = new Vector2(Size.X / 3, Size.X / 7),
                                Position = new Vector2(0, Size.X / 3),
                                Rotation = 90,
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new Container
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                CornerRadius = Size.X / 12,
                                Size = new Vector2(Size.X / 3, Size.X / 7),
                                Position = new Vector2(0, -1 * Size.X / 3),
                                Rotation = 270,
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            }
                        }
                    },
                    circleContainer = new Container
                    {
                        Masking = false,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,

                        Children = new Drawable[]
                        {
                            new CircularContainer
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(Size.X / 8),
                                Position = new Vector2(Size.X / 4, 0),
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new CircularContainer
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(Size.X / 8),
                                Position = new Vector2(-1 * Size.X / 4, 0),
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new CircularContainer
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(Size.X / 8),
                                Position = new Vector2(0, Size.X / 4),
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new CircularContainer
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(Size.X / 8),
                                Position = new Vector2(0, -1 * Size.X / 4),
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            }
                        }
                    }
                };

                if (currentGameMode == Gamemodes.Touhosu)
                {
                    //if (currentCharacter == SelectableCharacters.SakuyaIzayoi)// || currentCharacter == PlayableCharacters.AliceMuyart)
                        //speed.Alpha = 0.5f;

                    //if (currentCharacter == PlayableCharacters.KokoroHatano)
                    //combo.Alpha = 0.5f;
                }

                beatmap = game.Beatmap.GetBoundCopy();
                beatmap.ValueChanged += v => calculateScale();

                cursorScale = config.GetBindable<double>(OsuSetting.GameplayCursorSize);
                cursorScale.ValueChanged += v => calculateScale();

                autoCursorScale = config.GetBindable<bool>(OsuSetting.AutoCursorSize);
                autoCursorScale.ValueChanged += v => calculateScale();

                calculateScale();
            }

            private void calculateScale()
            {
                float scale = (float)cursorScale.Value;

                if (autoCursorScale && beatmap.Value != null)
                {
                    // if we have a beatmap available, let's get its circle size to figure out an automatic cursor scale modifier.
                    scale *= (float)(1 - 0.7 * (1 + beatmap.Value.BeatmapInfo.BaseDifficulty.CircleSize - BeatmapDifficulty.DEFAULT_DIFFICULTY) / BeatmapDifficulty.DEFAULT_DIFFICULTY);
                }

                Scale = new Vector2(scale);
            }

            protected override void Update()
            {
                base.Update();

                if (vitaruPlayfield.Player != null )
                {
                    switch (selectedCharacter)
                    {
                        case "SakuyaIzayoi":
                            //speed.Text = ((Sakuya)VitaruPlayfield.Player).SetRate.ToString();
                            break;
                        case "TomajiHakurei":
                            //speed.Text = ((Tomaji)VitaruPlayfield.Player).SetRate.ToString();
                            break;
                    }
                    //combo.Text = VitaruPlayfield.Player.Combo.ToString();
                }
            }
        }
    }
}
