using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Serilog;
using Tesseract;

namespace ForageBuddy
{
    public static class NameReader
    {
        private const int DutyReportScalar = 24;

        public static string ReadDutyReportName(Bitmap image, ILogger logger)
        {
            var transformedImage = TransformImage(image, image.Width * DutyReportScalar, image.Height * DutyReportScalar);

            var ocrPage = new TesseractEngine("./tessdata", "eng", EngineMode.Default)
                    {DefaultPageSegMode = PageSegMode.SparseText}
                .Process(transformedImage);

            var name = ocrPage
                .GetText()
                .Replace("\n", "")
                .Replace(" ", "");

            // TODO: Remove this when OCR is VERY consistent.
            // transformedImage.Save($@"C:/tmp/{name}_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.png");
            
            logger.Information($"OCR reading: {name} with {ocrPage.GetMeanConfidence()*100}% confidence");

            return name;
        }

        private static Bitmap TransformImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.GammaCorrected;
                graphics.InterpolationMode = InterpolationMode.Default;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.PixelOffsetMode = PixelOffsetMode.None;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.Tile);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
