using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Serilog;
using Tesseract;

namespace ForageBuddy
{
    public static class NameReader
    {
        private const int DutyReportScalar = 4;

        private const int BrightnessThreshold = 100;

        public static string ReadDutyReportName(Bitmap image, ILogger logger)
        {
            image.ToBinaryImage();

            var transformedImage = TransformImage(image, image.Width * DutyReportScalar, image.Height * DutyReportScalar);

            var ocrPage = new TesseractEngine("./tessdata", "eng", EngineMode.CubeOnly)
                    {DefaultPageSegMode = PageSegMode.SingleWord}
                .Process(transformedImage);

            var name = ocrPage
                .GetText()
                .Replace("\n", "");
            
            logger.Information($"OCR reading: {name} with {ocrPage.GetMeanConfidence()*100}% confidence");

            return name;
        }

        private static Bitmap TransformImage(Bitmap image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.Clamp);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private static void ToBinaryImage(this Bitmap image)
        {
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                    image.SetPixel(x, y, image.GetPixel(x, y)
                        .ColourToBrightness() > BrightnessThreshold 
                        ? Color.White 
                        : Color.Black);
        }

        private static int ColourToBrightness(this Color colour)
        => (int)(colour.R * 0.3 + colour.G * 0.59 + colour.B * 0.11);
    }
}
