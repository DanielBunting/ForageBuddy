using LockedBitmapUtil;
using LockedBitmapUtil.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ForageBuddy.Tests
{
    class ForageCalculatorTests
    {
        private IForageCalculator _forageCalculator;

        private Mock<IImageParser> _imageParser;
        private Mock<IScreenshotService> _screenshotService;
        private Mock<ILogger<ForageCalculator>> _logger;

        [SetUp]
        public void Setup()
        {
            _imageParser = new Mock<IImageParser>();
            _screenshotService = new Mock<IScreenshotService>();
            _logger = new Mock<ILogger<ForageCalculator>>();

            _imageParser
                .Setup(x => x.GetPlayerScoresInImage(It.IsAny<LockedBitmap>()))
                .Returns(new List<PlayerScore>() { new PlayerScore("test", 1, 1, 1)});

            _screenshotService
                .Setup(x => x.CaptureWindow(It.IsAny<IntPtr>()))
                .Returns(new Bitmap(10, 10));

            _forageCalculator = new ForageCalculator(_imageParser.Object, _screenshotService.Object, _logger.Object);
        }

        [Test]
        public void CalculateScores_OnRunning_CallsGetScreenshotWithCorrectParams()
        {
            // Arrange

            // Act
            _forageCalculator.CalculateScores(IntPtr.Zero);

            // Assert
            _screenshotService.Verify(x => x.CaptureWindow(IntPtr.Zero));
        }

        [Test]
        public void CalculateScore_OnRunning_CallsImageParserWithCorrectParams()
        {
            // Arrange

            // Act
            _forageCalculator.CalculateScores(IntPtr.Zero);

            // Assert
            _imageParser.Verify(x => x.GetPlayerScoresInImage(It.IsAny<LockedBitmap>()));
        }
    }
}
