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

        private List<ChestGrouping> chestGroups = new List<ChestGrouping>();

        int maxHeight = 0;
        int minHeight = 0; // TODO: Use this for bounding of the bottom of the Duty Report.

        public IEnumerable<PlayerScore> GetPlayerScoresInImage(string pathToFile)
        {

            var usedBitmap = new Bitmap(pathToFile);

            var lockedBitmap = new LockedBitmap(usedBitmap);

            lockedBitmap.LockBits();
            
            // TODO: Implement an earlier check to see if in the Forage puzzle.

            if(lockedBitmap.DoesImageExist(_bottomOfForageLegend))
                maxHeight = lockedBitmap.GetFirstLocation(_bottomOfForageLegend).Y;
            else if (lockedBitmap.DoesImageExist(_topOfDutyReport))
                maxHeight = lockedBitmap.GetFirstLocation(_topOfDutyReport).Y;
            else return new List<PlayerScore>();


            if (lockedBitmap.DoesImageExist(_bottomOfDutyReport))
                minHeight = lockedBitmap.GetFirstLocation(_bottomOfDutyReport).Y; // TODO: Implement a sector to speed up the searches, should remove redundant lookups also.
            else return new List<PlayerScore>();

            // TODO: Make all chest implement IChest or similar - it will allow the composite pattern to assign values.
            var cursedChestLocations = lockedBitmap.GetAllOccurences(_cursedChest).ToList();
            var fetishJarLocations = lockedBitmap.GetAllOccurences(_fetishJar).ToList();
            var boneBoxLocations = lockedBitmap.GetAllOccurences(_boneBox).ToList();

            lockedBitmap.UnlockBits();

            chestGroups = new List<ChestGrouping>();

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

            foreach (var chestGroup in chestGroups)
            {
                var location = new Point(chestGroup.GroupLocation.X + 8, chestGroup.GroupLocation.Y - 45);
                var name = NameReader.ReadDutyReportName(usedBitmap.Clone(new Rectangle(location, new Size(80, 20)), usedBitmap.PixelFormat));
                result.Add(new PlayerScore(name, chestGroup.BoneBox, chestGroup.FetishJar, chestGroup.CursedChest));
            }

            return result;
        }

        private void GroupChests(ChestType chestType, Point chestLocation)
        {
            if (chestLocation.Y < maxHeight) return;

            var matchingGroup = chestGroups.FirstOrDefault(x =>
                x.GroupLocation.Y < chestLocation.Y + 30 && x.GroupLocation.Y > chestLocation.Y - 30);

            if(matchingGroup != null)
                matchingGroup.AddChest(chestType, chestLocation);
            else
                chestGroups.Add(new ChestGrouping(chestType, chestLocation));
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
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

        public void Dispose()
        {
            _bottomOfDutyReport.UnlockBits();
            _bottomOfForageLegend.UnlockBits();
            _topOfDutyReport.UnlockBits();
        }
    }

    enum ChestType
    {
        BoneBox, FetishJar, CursedChest
    }

    class ChestGrouping
    {
        // All values in here can be read only. 

        public Point GroupLocation;
        public int CursedChest = 0;
        public int FetishJar = 0;
        public int BoneBox = 0;

        public ChestGrouping(ChestType initialChest, Point groupLocation)
        {
            this.GroupLocation = groupLocation;
            this.AddChest(initialChest, groupLocation);
        }

        public void AddChest(ChestType typeOfChest, Point chestLocation)
        {
            switch (typeOfChest)
            {
                case ChestType.BoneBox:
                    this.BoneBox++;
                    break;
                case ChestType.FetishJar:
                    this.FetishJar++;
                    break;
                case ChestType.CursedChest:
                    this.CursedChest++;
                    break;
            }

            if (chestLocation.Y < this.GroupLocation.Y) this.GroupLocation.Y = chestLocation.Y;
            if (chestLocation.X < this.GroupLocation.X) this.GroupLocation.X = chestLocation.X;
        }
    }
}
