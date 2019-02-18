using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace ForageBuddy
{
    // TODO: Clean up this class and break it up. 

    public class ImageParser : IDisposable
    {
        public ImageParser()
        {
            _topOfDutyReport.LockBits();
            _bottomOfForageLegend.LockBits();
            _bottomOfDutyReport.LockBits();

            _cursedChest.LockBits();
            _fetishJar.LockBits();
            _boneBox.LockBits();
        }

        private readonly LockedBitmap _topOfDutyReport = new LockedBitmap(Properties.Resources.TopOfDutyReport);
        private readonly LockedBitmap _bottomOfForageLegend = new LockedBitmap(Properties.Resources.BottomOfForageLegend);
        private readonly LockedBitmap _bottomOfDutyReport = new LockedBitmap(Properties.Resources.BottomOfDutyReport);

        private readonly LockedBitmap _cursedChest = new LockedBitmap(Properties.Resources.CursedChest);
        private readonly LockedBitmap _fetishJar = new LockedBitmap(Properties.Resources.FetishJar);
        private readonly LockedBitmap _boneBox = new LockedBitmap(Properties.Resources.BoneBox);

        private List<ChestGrouping> _chestGroups = new List<ChestGrouping>();

        private int _maxHeight = 0;

        public IEnumerable<PlayerScore> GetPlayerScoresInImage(string pathToFile)
        {
            var bitmapImage = new Bitmap(pathToFile);

            var lockedBitmapImage = new LockedBitmap(bitmapImage);

            lockedBitmapImage.LockBits();
            
            // TODO: Implement an earlier check to see if in the Forage puzzle.

            if(lockedBitmapImage.DoesImageExist(_bottomOfForageLegend))
                _maxHeight = lockedBitmapImage.GetFirstLocation(_bottomOfForageLegend).Y;
            else if (lockedBitmapImage.DoesImageExist(_topOfDutyReport))
                _maxHeight = lockedBitmapImage.GetFirstLocation(_topOfDutyReport).Y;
            else return new List<PlayerScore>();

            var cursedChestLocations = lockedBitmapImage.GetAllOccurences(_cursedChest).ToList();
            var fetishJarLocations = lockedBitmapImage.GetAllOccurences(_fetishJar).ToList();
            var boneBoxLocations = lockedBitmapImage.GetAllOccurences(_boneBox).ToList();

            lockedBitmapImage.UnlockBits();

            _chestGroups = new List<ChestGrouping>();

            foreach (var boneBoxLocation in boneBoxLocations)
            {
                GroupChests(ChestType.BoneBox, boneBoxLocation);
            }

            foreach (var fetishJarLocation in fetishJarLocations)
            {
                GroupChests(ChestType.FetishJar, fetishJarLocation);
            }

            foreach (var cursedChestLocation in cursedChestLocations)
            {
                GroupChests(ChestType.CursedChest, cursedChestLocation);
            }

            var result = new List<PlayerScore>();

            foreach (var chestGroup in _chestGroups)
            {
                var location = new Point(chestGroup.GroupLocation.X + 8, chestGroup.GroupLocation.Y - 45);
                var name = NameReader.ReadDutyReportName(bitmapImage.Clone(new Rectangle(location, new Size(80, 20)), bitmapImage.PixelFormat));
                result.Add(new PlayerScore(name, chestGroup.BoneBox, chestGroup.FetishJar, chestGroup.CursedChest));
            }

            return result;
        }

        private void GroupChests(ChestType chestType, Point chestLocation)
        {
            if (chestLocation.Y < _maxHeight + 45) return;

            var matchingGroup = _chestGroups.FirstOrDefault(x =>
                x.GroupLocation.Y < chestLocation.Y + 30 && x.GroupLocation.Y > chestLocation.Y - 30);

            if(matchingGroup != null)
                matchingGroup.AddChest(chestType, chestLocation);
            else
                _chestGroups.Add(new ChestGrouping(chestType, chestLocation));
        }

        public void Dispose()
        {
            _bottomOfDutyReport.UnlockBits();
            _bottomOfForageLegend.UnlockBits();
            _topOfDutyReport.UnlockBits();
        }
    }
}
