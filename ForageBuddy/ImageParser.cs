using LockedBitmapUtil;
using LockedBitmapUtil.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ForageBuddy
{
    public class ImageParser : IImageParser
    {
        const int Scalar = 8;
        const int BrightnessThreshold = 100;

        private readonly LockedBitmap _topOfDutyReport = new LockedBitmap(Properties.Resources.TopOfDutyReport);
        private readonly LockedBitmap _bottomOfForageLegend = new LockedBitmap(Properties.Resources.BottomOfForageLegend);
        private readonly LockedBitmap _bottomOfDutyReport = new LockedBitmap(Properties.Resources.BottomOfDutyReport);

        private readonly LockedBitmap _cursedChest = new LockedBitmap(Properties.Resources.CursedChest);
        private readonly LockedBitmap _fetishJar = new LockedBitmap(Properties.Resources.FetishJar);
        private readonly LockedBitmap _boneBox = new LockedBitmap(Properties.Resources.BoneBox);

        private readonly INameReader nameReader;
        private readonly ILogger<ImageParser> _logger;

        public ImageParser(INameReader nameReader, ILogger<ImageParser> logger)
        {
            _topOfDutyReport.LockBits();
            _bottomOfForageLegend.LockBits();
            _bottomOfDutyReport.LockBits();

            _cursedChest.LockBits();
            _fetishJar.LockBits();
            _boneBox.LockBits();
            this.nameReader = nameReader;
            _logger = logger;
        }

        private int _maxHeight = 0;

        // TODO: Break this out to it's own service?
        private List<ChestGrouping> _chestGroups = new List<ChestGrouping>();

        public IEnumerable<PlayerScore> GetPlayerScoresInImage(LockedBitmap screenshot)
        {
            if (screenshot.DoesImageExist(_bottomOfForageLegend, out var bottomLocation))
                _maxHeight = bottomLocation.Y;
            else if (screenshot.DoesImageExist(_topOfDutyReport, out var topLocation))
                _maxHeight = topLocation.Y;
            else
            {
                _logger.LogWarning($"No duty report found.");
                return new List<PlayerScore>();
            }

            _logger.LogInformation($"Top of search area set to {_maxHeight}");

            var cursedChestLocations = screenshot.GetAllOccurences(_cursedChest).ToList();
            var fetishJarLocations = screenshot.GetAllOccurences(_fetishJar).ToList();
            var boneBoxLocations = screenshot.GetAllOccurences(_boneBox).ToList();

            _logger.LogInformation($"Chests found: Bone Boxes - {boneBoxLocations.Count} "  
                + $", Fetish Jars -  {fetishJarLocations.Count}, Cursed Chests - {cursedChestLocations.Count}");

            _chestGroups = new List<ChestGrouping>();

            foreach (var boneBoxLocation in boneBoxLocations)
                GroupChests(ChestType.BoneBox, boneBoxLocation, screenshot);

            foreach (var fetishJarLocation in fetishJarLocations)
                GroupChests(ChestType.FetishJar, fetishJarLocation, screenshot);

            foreach (var cursedChestLocation in cursedChestLocations)
                GroupChests(ChestType.CursedChest, cursedChestLocation, screenshot);

            return _chestGroups
                .AsParallel()
                .Select(x =>
                    new PlayerScore(nameReader.ReadDutyReportName(
                        x.NameSector
                        .Resize(x.NameSector.Width * Scalar, x.NameSector.Height * Scalar)
                        .ToBinaryImage(BrightnessThreshold, Color.White, Color.Black)
                        .ToBitmap()),
                        x.BoneBox,
                        x.FetishJar,
                        x.CursedChest))
                .ToList();
        }

        private void GroupChests(ChestType chestType, Point chestLocation, LockedBitmap screenshot)
        {
            if (chestLocation.Y < _maxHeight + 45) return;

            var matchingGroup = _chestGroups.FirstOrDefault(x =>
                x.GroupLocation.Y < chestLocation.Y + 30 && x.GroupLocation.Y > chestLocation.Y - 30);

            if (matchingGroup != null)
                matchingGroup.AddChest(chestType, chestLocation);
            else
                _chestGroups.Add(new ChestGrouping(
                    chestType,
                    chestLocation,
                    screenshot.Crop(chestLocation.Y + 3, chestLocation.Y - 45, 90, 20)));
        }
    }
}
