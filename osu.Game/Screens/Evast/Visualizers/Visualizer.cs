// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Threading;
using osu.Framework.Localisation;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics.UserInterface;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Beatmaps;
using osu.Game.Screens.Evast.MusicVisualizers;
using System.Threading.Tasks;

namespace osu.Game.Screens.Evast.Visualizers
{
    public class Visualizer : BeatmapScreen

    {

        protected override bool HideOverlaysOnEnter => true;

        private LocalisationEngine localisation;

        private BeatmapSprite beatmapSprite;

        private BeatmapManager beatmaps;

        private SpriteText title, artist;

        private FillFlowContainer musicControls;

        private IconButton playButton;


        [BackgroundDependencyLoader]
        private void load()
        {
            this.beatmaps = beatmaps;
            this.localisation = localisation;
            Padding = new MarginPadding { Top = 0 };
            Children = new Drawable[]
            {
                new ParallaxContainer
                {
                    ParallaxAmount = -0.02f,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new SpaceParticlesContainer(),
                    }
                },
                
                new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,

                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Spacing = new Vector2(0,0),
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            Margin = new MarginPadding {Top = 20},
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
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
                                }
                                ,
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
                                        Colour = Color4.Black.Opacity(0.2f),
                                        Offset = new Vector2(0, 4),
                                        Radius = 16
                                    },
                                    Child = beatmapSprite = new BeatmapSprite
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        FillMode = FillMode.Fill,
                                    }
                                }
                            }
                        },

                        title = new SpriteText
                                {
                                    Margin = new MarginPadding {Top = 10},
                                    Origin = Anchor.TopCentre,
                                    Anchor = Anchor.TopCentre,
                                    Position = new Vector2(0, 0),
                                    TextSize = 60,
                                    Shadow = true,
                                    Colour = Color4.White,
                                    Text = @"IMAGE -MATERIAL-",
                                    Font = @"Exo2.0-Light"
                                },
                        artist = new SpriteText
                                {
                                    Margin = new MarginPadding {Top = 5},
                                    Origin = Anchor.TopCentre,
                                    Anchor = Anchor.TopCentre,
                                    Position = new Vector2(0, 0),
                                    TextSize = 36,
                                    Shadow = true,
                                    Colour = Color4.White,
                                    Text = @"Tatsh",
                                    Font = @"Exo2.0-LightItalic"
                                },

                        musicControls = new FillFlowContainer
                        {
                            Margin = new MarginPadding {Top = 30},
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            AutoSizeAxes = Axes.Both,
                            Masking = false,
                            CornerRadius = 16,
                            Children = new Drawable[]
                            {
                                playButton = new IconButton
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Scale = new Vector2(2.4f),
                                    IconScale = new Vector2(1.6f),
                                    CornerRadius = 15,
                                    
                                    HoverColour = Color4.Transparent,
                                    FlashColour = Color4.White.Opacity(0f),
                                    
                                    // Action = play,
                                    Icon = FontAwesome.fa_play_circle_o,
                                    
                                }
                             
                            }

                        }
                    },

                }
            };
        }

        protected override void OnBeatmapChange(WorkingBeatmap beatmap)
        {
            base.OnBeatmapChange(beatmap);
            beatmapSprite.UpdateTexture(beatmap);

            if (beatmap?.Beatmap == null)
            {
                title.Current = null;
                title.Text = @"Nothing is playing";

                artist.Current = null;
                artist.Text = @"Nothing is playing";
            }
            /*else
            {
                BeatmapMetadata metadata = beatmap.Metadata;
                title.Current = localisation.GetUnicodePreference(metadata.TitleUnicode, metadata.Title);
                artist.Current = localisation.GetUnicodePreference(metadata.TitleUnicode, metadata.Artist);
            }*/

        }

        private class BeatmapSprite : Sprite
        {
            public void UpdateTexture(WorkingBeatmap beatmap)
            {
                if (beatmap.Background != null)
                    Texture = beatmap.Background;
                else
                {
                    void texture(TextureStore background)
                    {
                        Texture = background.Get(@"Backgrounds/bg1");
                    }
                }
                
                

            }
        }
    }
}
