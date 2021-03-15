using LockedBitmapUtil.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;

namespace ForageBuddy.Tests
{
    public class ImageParserTests
    {
        private ImageParser _imageParser;

        private Mock<INameReader> _nameReader;

        private Mock<ILogger<ImageParser>> _logger;

        [SetUp]
        public void Setup()
        {
            _nameReader = new Mock<INameReader>();
            _logger = new Mock<ILogger<ImageParser>>();

            _imageParser = new ImageParser(_nameReader.Object, _logger.Object);
        }

        [Test]
        public void GetPlayerScoresInImage_WithoutToppingImage_ReturnsEmptyList()
        {
            var lockedBitmap = new Bitmap(50, 50)
                .ToLockedBitmap();

            var result = _imageParser.GetPlayerScoresInImage(lockedBitmap);

            Assert.AreEqual(new List<PlayerScore>(), result);
        }

        [Test]
        public void GetPlayerScoresInImage_GivenImage_CalculatesScoreForEachPerson()
        {
            _nameReader.Setup(x => x.ReadDutyReportName(It.IsAny<Bitmap>()))
                .Returns("Test");

            _imageParser
                .GetPlayerScoresInImage(new Bitmap(Properties.Resources.ImageParser_EndToEndTest).ToLockedBitmap());

            _nameReader.Verify(x => x.ReadDutyReportName(It.IsAny<Bitmap>()), Times.Exactly(7));
        }

    }
}