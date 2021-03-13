using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using System.Drawing;
using System.Collections.Generic;
using LockedBitmapUtil.Extensions;

namespace ForageBuddy.Tests
{
    class NameReaderTests
    {
        private NameReader _nameReader;
        private Mock<ILogger<NameReader>> _logger;

        private List<(Bitmap image, string expectedName)> _testImages;

        const int scalar = 8;
        const int BrightnessThreshold = 100;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<NameReader>>();
            _nameReader = new NameReader(_logger.Object);

            _testImages = new List<(Bitmap image, string expectedName)>()
            {
                (Properties.Resources.NameReader_Annapurna_1, "Annapurna"),
                (Properties.Resources.NameReader_Jaxxa_1, "Jaxxa"),
                (Properties.Resources.NameReader_Raffaella_1, "Raffaella"),
                //(Properties.Resources.NameReader_Theredwench_1, "Theredwench"), // TODO: Currently broken.
                //(Properties.Resources.NameReader_Yeetmcskeet_1, "Yeetmcskeet")  // TODO: Currently broken
            };
        }


        [Test]
        public void ReadDutyReportName_WhenGivenImageOfName_ReturnsCorrectName()
        {
            foreach (var (image, expectedName) in _testImages)
            {
                // Arange
                var transformed = image
                    .ToLockedBitmap()
                    .Resize(image.Width * scalar, image.Height * scalar)
                    .ToBinaryImage(BrightnessThreshold, Color.White, Color.Black)
                    .ToBitmap();

                // Act
                var recievedName = _nameReader.ReadDutyReportName(transformed);

                // Assert
                Assert.AreEqual(expectedName, recievedName);
            }
        }
    }
}
