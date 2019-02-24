using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Serilog;
using Tesseract;

namespace ForageBuddy
{
    public static class NameReader
    {
        private const int DutyReportScalar = 8;

        public static string ReadDutyReportName(Bitmap image, ILogger logger)
        {
            var ocrPage = new TesseractEngine("./tessdata", "eng", EngineMode.Default)
                    {DefaultPageSegMode = PageSegMode.SingleWord}
                .Process(ResizeImage(image, image.Width * DutyReportScalar, image.Height * DutyReportScalar));
            
            var name = ocrPage
                .GetText()
                .Replace("\n", "");
            
            logger.Information($"OCR reading: {name} with {ocrPage.GetMeanConfidence()*100}% confidence");

            return name;
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
