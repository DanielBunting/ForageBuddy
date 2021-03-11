using System.Drawing;
using Tesseract;

namespace ForageBuddy.Tesseract
{
    public static class TesseractExtensions
    {
        private static readonly BitmapToPixConverter bitmapConverter = new BitmapToPixConverter();

        /// <summary>
        /// Converts the specified <paramref name="img"/> to a Pix.
        /// </summary>
        /// <param name="img">The source image to be converted.</param>
        /// <returns>The converted bitmap image as a <see cref="Pix"/>.</returns>
        public static Pix ToPix(this Bitmap img)
        {
            return bitmapConverter.Convert(img);
        }

        public static PixColor ToPixColor(this Color color)
        {
            return new PixColor(color.R, color.G, color.B, color.A);
        }
    }
}
