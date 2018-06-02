// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Game.Beatmaps;
using osu.Game.Screens.Evast;
using osu.Game.Screens.Evast.Visualizers;
using osu.Game.Screens.Evast.MusicVisualizers;
using osu.Game.Screens.KoziLord.EvastModded.Visualizers;

namespace osu.Game.Screens.KoziLord.EvastModded.MusicPlayer
{

    //TODO: Everything, adding media buttons and showing the names, and of course entry animation.
    public class FullscreenPlayer : BeatmapScreen
    {
        private BeatmapSprite beatmapSprite;

        public SpriteText Title;
        public SpriteText Artist;

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new SpaceParticlesContainer(),
                new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,

                    Children = new Drawable[]
                    {
                        new Container
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Size = new Vector2(350),
                            Children = new Drawable[]
                            {
                                new CircularVisualizer
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    DegreeValue = 180,
                                    BarsAmount = 100,
                                    CircleSize = 348,
                                    BarWidth = 2,
                                },
                                new CircularVisualizer
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    DegreeValue = 180,
                                    BarsAmount = 100,
                                    CircleSize = 348,
                                    BarWidth = 2,
                                    Rotation = 180,
                                },
                                new CircularContainer
                                {
                                   Anchor = Anchor.Centre,
                                   Origin = Anchor.Centre,
                                   Size = new Vector2(350),
                                   Masking = true,
                                   EdgeEffect = new EdgeEffectParameters
                                   {
                                       Type = EdgeEffectType.Shadow,
                                       Colour = Color4.Black.Opacity(0.18f),
                                       Offset = new Vector2(0, 2),
                                       Radius = 24,
                                    },
                                   Child = beatmapSprite = new BeatmapSprite
                                   {
                                       RelativeSizeAxes = Axes.Both,
                                       Anchor = Anchor.Centre,
                                       Origin = Anchor.Centre,
                                       FillMode = FillMode.Fill,
                                   }
                                }

                            },

                        },
                        Title = new SpriteText
                        {
                            Margin = new MarginPadding{Top = 20},
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Text = @"Song Name",
                            Font = @"Exo2.0-Medium",
                            TextSize = 56,
                            Shadow = true
                            
                        },
                        Artist = new SpriteText
                        {
                            Margin = new MarginPadding{Top = 10},
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Text = @"Artist",
                            Font = @"Exo2.0-MediumItalic",
                            TextSize = 36,
                            Shadow = true
                        }
                    }
                },
              
            };
        }

        protected override void OnBeatmapChange(WorkingBeatmap beatmap)
        {
            base.OnBeatmapChange(beatmap);
            beatmapSprite.UpdateTexture(beatmap);
        }

        private class BeatmapSprite : Sprite
        {
            public void UpdateTexture(WorkingBeatmap beatmap)
            {
                if (beatmap.Background != null)
                    Texture = beatmap.Background;
            }
        }
    }
}
