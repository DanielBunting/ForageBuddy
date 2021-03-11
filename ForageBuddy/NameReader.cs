using ForageBuddy.Tesseract;
using System.Drawing;
using Tesseract;

namespace ForageBuddy
{
    public static class NameReader
    {
        public static string ReadDutyReportName(Bitmap image)
        => new TesseractEngine("./tessdata", "eng", EngineMode.CubeOnly)
            { DefaultPageSegMode = PageSegMode.SingleWord }
            .Process(image.ToPix())
            .GetText()
            .Replace("\n", "");
    }
}
