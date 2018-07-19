using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Game.Configuration;
using Symcol.Core.Graphics.Sprites;
using Bitmap = System.Drawing.Bitmap;
// ReSharper disable InconsistentNaming

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Abilities
{
    public class ScreenSnap : SymcolSprite
    {
        private GameHost host;

        private readonly Box area;

        private static int img_count;
        private int imgCount;

        private static ResourceStore<byte[]> img_resources;
        private static TextureStore img_textures;

        public ScreenSnap(Box area)
        {
            Alpha = 0;
            AlwaysPresent = true;
            this.area = area;
        }

        [BackgroundDependencyLoader]
        private void load(OsuGame osu, OsuConfigManager config, GameHost host, Storage storage)
        {
            this.host = host;

            Bindable<ScreenshotFormat> screenshotFormat = config.GetBindable<ScreenshotFormat>(OsuSetting.ScreenshotFormat);

            try
            {
                Rectangle rect = new Rectangle(new Point((int)area.DrawSize.X, (int)area.DrawSize.Y), new Size((int)area.ToScreenSpace(Vector2.Zero).X, (int)area.ToScreenSpace(Vector2.Zero).Y));

                Bitmap bitmap = snapshot(rect);

                switch (screenshotFormat.Value)
                {
                    case ScreenshotFormat.Png:
                        bitmap.Save(storage.GetStream("vitaru\\temp\\snapshot" + img_count, FileAccess.Write, FileMode.Create), ImageFormat.Png);
                        imgCount = img_count;
                        img_count++;
                        break;
                    case ScreenshotFormat.Jpg:
                        bitmap.Save(storage.GetStream("vitaru\\temp\\snapshot" + img_count, FileAccess.Write, FileMode.Create), ImageFormat.Jpeg);
                        imgCount = img_count;
                        img_count++;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(screenshotFormat));
                }

                if (img_resources == null)
                {
                    img_resources = new ResourceStore<byte[]>(new StorageBackedResourceStore(storage.GetStorageForDirectory("vitaru\\temp")));
                    img_textures = new TextureStore(new RawTextureLoaderStore(img_resources));
                }
            }
            catch (Exception e) { Logger.Error(e, "Failed to take ScreenSnap!", LoggingTarget.Runtime); }
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Texture = img_textures?.Get("snapshot" + imgCount);
        }

        /// <summary>
        /// FROM: osu.Framework.Platform.GameHost
        /// </summary>
        private Bitmap snapshot(Rectangle rectangle)
        {
            Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height);
            BitmapData data = bitmap.LockBits(rectangle, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            host.DrawThread.Scheduler.Add(() =>
            {
                if (GraphicsContext.CurrentContext == null)
                    throw new GraphicsContextMissingException();

                OpenTK.Graphics.OpenGL.GL.ReadPixels(0, 0, rectangle.Width, rectangle.Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, data.Scan0);
            });

            bitmap.UnlockBits(data);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return bitmap;
        }
    }
}
