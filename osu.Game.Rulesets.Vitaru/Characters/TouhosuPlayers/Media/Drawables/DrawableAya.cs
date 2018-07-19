using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.UI;
using OpenTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Media.Drawables
{
    public class DrawableAya : DrawableTouhosuPlayer
    {
        private GameHost host;

        public DrawableAya(VitaruPlayfield playfield, VitaruNetworkingClientHandler vitaruNetworkingClientHandler)
            : base(playfield, new Aya(), vitaruNetworkingClientHandler)
        {
            Spell += action =>
            {
                using (Bitmap bitmap = snapshot(new Rectangle(new Point(host.Window.ClientRectangle.X, host.Window.ClientRectangle.Y), new Size(host.Window.ClientSize.Width, host.Window.ClientSize.Height))));
                {

                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            this.host = host;
        }

        /// <summary>
        /// FROM: osu.Framework.Platform.GameHost
        /// </summary>
        private Bitmap snapshot(Rectangle rectangle)
        {
            var bitmap = new Bitmap(rectangle.Width, rectangle.Height);
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
