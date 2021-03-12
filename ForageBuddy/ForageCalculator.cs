using System;
using System.Collections.Generic;
using LockedBitmapUtil.Extensions;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ForageBuddy
{
    class ForageCalculator : IForageCalculator
    {
        private readonly IImageParser _imageParser;
        private readonly IScreenshotService _screenshotService;
        private readonly ILogger<ForageCalculator> _logger;

        public ForageCalculator(IImageParser imageParser, IScreenshotService screenshotService, ILogger<ForageCalculator> logger)
        {
            _imageParser = imageParser;
            _screenshotService = screenshotService;
            _logger = logger;
        }

        public List<string> CalculateScores(IntPtr clientHandle)
        {
            _logger.LogInformation("Calculating scores..");

            var sceenshot = _screenshotService
                .CaptureWindow(clientHandle)
                .ToLockedBitmap();

            return _imageParser
                .GetPlayerScoresInImage(sceenshot)
                .OrderBy(x => x.TotalScore)
                .Select(x => x.PlayerScoreString()).ToList();
        }
    }
}
