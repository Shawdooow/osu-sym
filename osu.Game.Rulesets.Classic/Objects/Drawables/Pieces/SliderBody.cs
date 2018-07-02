// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.OpenGL.Textures;
using osu.Framework.Graphics.Lines;
using osu.Framework.Graphics.Textures;
using osu.Framework.MathUtils;
using osu.Game.Configuration;
using OpenTK;
using OpenTK.Graphics;
using Symcol.Core.Graphics.Containers;

namespace osu.Game.Rulesets.Classic.Objects.Drawables.Pieces
{
    public class SliderBody : SymcolContainer, ISliderProgress
    {
        private readonly Path path;

        public float PathWidth
        {
            get { return path.PathWidth; }
            set { path.PathWidth = value; }
        }

        public double? SnakedStart { get; private set; }
        public double? SnakedEnd { get; private set; }

        private Color4 accentColour;
        /// <summary>
        /// Used to colour the path.
        /// </summary>
        public Color4 AccentColour
        {
            get { return accentColour; }
            set
            {
                if (accentColour == value)
                    return;
                accentColour = value;

                if (LoadState == LoadState.Ready)
                    Schedule(reloadTexture);
            }
        }

        private int textureWidth => (int)PathWidth * 2;

        private readonly Slider slider;
        public SliderBody(Slider s)
        {
            slider = s;

            Add(path = new Path
            {
                Blending = BlendingMode.None,
            });
        }

        public void SetRange(double p0, double p1)
        {
            if (p0 > p1)
                MathHelper.Swap(ref p0, ref p1);

            if (updateSnaking(p0, p1))
            {
                // Autosizing does not give us the desired behaviour here.
                // We want the container to have the same size as the slider,
                // and to be positioned such that the slider head is at (0,0).
                path.Position = -path.PositionInBoundingBox(slider.PositionAt(0) - currentCurve[0]);
            }
        }

        private Bindable<bool> snakingIn;

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config)
        {
            snakingIn = config.GetBindable<bool>(OsuSetting.SnakingInSliders);

            reloadTexture();
        }

        private void reloadTexture()
        {
            var texture = new Texture(textureWidth, 1);

            //initialise background
            var raw = new RawTexture(textureWidth, 1);
            var bytes = raw.Data;

            const float aa_portion = 0.02f;
            const float border_portion = 0.128f;
            const float gradient_portion = 1 - border_portion;

            const float opacity_at_centre = 0.75f;
            const float opacity_at_edge = 0.75f;

            for (int i = 0; i < textureWidth; i++)
            {
                float progress = (float)i / (textureWidth - 1);

                if (progress <= border_portion)
                {
                    bytes[i * 4] = 255;
                    bytes[i * 4 + 1] = 255;
                    bytes[i * 4 + 2] = 255;
                    bytes[i * 4 + 3] = (byte)(Math.Min(progress / aa_portion, 1) * 255);
                }
                else
                {
                    progress -= border_portion;

                    float r = (float)Interpolation.ApplyEasing(Easing.None, AccentColour.R - (AccentColour.R - Math.Min(AccentColour.R * 2f, 1)) * progress / gradient_portion) * 255;
                    float g = (float)Interpolation.ApplyEasing(Easing.None, AccentColour.G - (AccentColour.G - Math.Min(AccentColour.G * 2f, 1)) * progress / gradient_portion) * 255;
                    float b = (float)Interpolation.ApplyEasing(Easing.None, AccentColour.B - (AccentColour.B - Math.Min(AccentColour.B * 2f, 1)) * progress / gradient_portion) * 255;

                    bytes[i * 4] = (byte)r;
                    bytes[i * 4 + 1] = (byte)g;
                    bytes[i * 4 + 2] = (byte)b;
                    bytes[i * 4 + 3] = (byte)((opacity_at_edge - (opacity_at_edge - opacity_at_centre) * progress / gradient_portion) * (AccentColour.A * 255));
                }
            }

            texture.SetData(new TextureUpload(raw));
            path.Texture = texture;
        }

        private readonly List<Vector2> currentCurve = new List<Vector2>();
        private bool updateSnaking(double p0, double p1)
        {
            if (SnakedStart == p0 && SnakedEnd == p1) return false;

            SnakedStart = p0;
            SnakedEnd = p1;

            slider.Curve.GetPathToProgress(currentCurve, p0, p1);

            path.ClearVertices();
            foreach (Vector2 p in currentCurve)
                path.AddVertex(p - currentCurve[0]);

            return true;
        }

        public void UpdateProgress(double progress, int repeat)
        {
            double start = 0;
            double end = snakingIn ? MathHelper.Clamp((Time.Current - (slider.StartTime - slider.TimePreempt)) / slider.TimeFadein, 0, 1) : 1;

            if (repeat >= slider.RepeatCount - 1)
            {
                if (Math.Min(repeat, slider.RepeatCount - 1) % 2 == 1)
                {
                    start = 0;
                    end = 1;
                }
            }

            SetRange(start, end);
        }
    }
}
