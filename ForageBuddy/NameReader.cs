using ForageBuddy.Tesseract;
using Microsoft.Extensions.Logging;
using System.Drawing;
using Tesseract;

namespace ForageBuddy
{
    public class NameReader : INameReader
    {
        private readonly ILogger<NameReader> _logger;

        public NameReader(ILogger<NameReader> logger)
        {
            _logger = logger;
        }

        public string ReadDutyReportName(Bitmap image)
        {
            var predition =  new TesseractEngine("./tessdata", "eng", EngineMode.CubeOnly)
            { DefaultPageSegMode = PageSegMode.SingleWord }
              .Process(image.ToPix());

            var name = predition.GetText().Replace("\n", "");
            var confidence = predition.GetMeanConfidence();

            _logger.LogInformation($"Predicted name of {name} with confidence of {confidence*100}%");

            return name;
        }
    }
}
