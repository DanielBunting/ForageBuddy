using System;
using System.Collections.Generic;
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

        public IEnumerable<PlayerScore> GetPlayerScoresInImage(string pathToFile)
        {
            var maxHeight = 0;
            var minHeight = 0; // TODO: Use this for bounding of the bottom of the Duty Report.

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
                minHeight = lockedBitmap.GetFirstLocation(_bottomOfDutyReport).Y; // TODO: Implement a sector to speed up the searches.
            else return new List<PlayerScore>();

            // TODO: Make all chest implement IChest or similar - it will allow the composite pattern to assign values.
            var cursedChestLocations = lockedBitmap.GetAllOccurences(_cursedChest).ToList();
            var fetishJarLocations = lockedBitmap.GetAllOccurences(_fetishJar).ToList();
            var boneBoxLocations = lockedBitmap.GetAllOccurences(_boneBox).ToList();

            lockedBitmap.UnlockBits();

            var chestGroups = new List<ChestGroup>();

            foreach (var boneBox in boneBoxLocations)
            {
                if (boneBox.Y < maxHeight) continue;

                var matchingGroup = chestGroups.FirstOrDefault(x => x.TopLeft.Y < boneBox.Y + 30 && x.TopLeft.Y > boneBox.Y - 30);

                if (matchingGroup != null)
                {
                    matchingGroup.BBs++;
                    if (boneBox.Y < matchingGroup.TopLeft.Y) matchingGroup.TopLeft.Y = boneBox.Y;
                    if (boneBox.X < matchingGroup.TopLeft.X) matchingGroup.TopLeft.X = boneBox.X;
                }
                else 
                    chestGroups.Add(new ChestGroup(boneBox));
            }

            foreach (var fetishJar in fetishJarLocations)
            {
                if (fetishJar.Y < maxHeight) continue;

                var matchingGroup = chestGroups.FirstOrDefault(x => x.TopLeft.Y < fetishJar.Y + 30 && x.TopLeft.Y > fetishJar.Y - 30);

                if (matchingGroup != null)
                {
                    matchingGroup.FJs++;
                    if (fetishJar.Y < matchingGroup.TopLeft.Y) matchingGroup.TopLeft.Y = fetishJar.Y;
                    if (fetishJar.X < matchingGroup.TopLeft.X) matchingGroup.TopLeft.X = fetishJar.X;
                }
                else
                    chestGroups.Add(new ChestGroup(fetishJar));
            }

            foreach (var cursedChest in cursedChestLocations)
            {
                if (cursedChest.Y < maxHeight) continue;

                var matchingGroup = chestGroups.FirstOrDefault(x => x.TopLeft.Y < cursedChest.Y + 30 && x.TopLeft.Y > cursedChest.Y - 30);

                if (matchingGroup != null)
                {
                    matchingGroup.CCs++;
                    if (cursedChest.Y < matchingGroup.TopLeft.Y) matchingGroup.TopLeft.Y = cursedChest.Y;
                    if (cursedChest.X < matchingGroup.TopLeft.X) matchingGroup.TopLeft.X = cursedChest.X;
                }
                else
                    chestGroups.Add(new ChestGroup(cursedChest));
            }

            var result = new List<PlayerScore>();

            foreach (var chestGroup in chestGroups)
            {
                var scalar = 8;

                var location = new Point(chestGroup.TopLeft.X + 8, chestGroup.TopLeft.Y - 45);

                var rect = new Rectangle(location, new Size(80,20));

                var ocrImage = ResizeImage(usedBitmap.Clone(rect, usedBitmap.PixelFormat), rect.Width * scalar, rect.Height * scalar);

                var name = GetTextFromBitmap(ocrImage);

                result.Add(new PlayerScore(name, chestGroup.BBs, chestGroup.FJs, chestGroup.CCs));
            }

            return result;
        }

        string GetTextFromBitmap(Bitmap img)
        {
            // TODO: Figure out how to not have to create this multiple times.
            var tesseractEngine = new TesseractEngine("./tessdata", "eng", EngineMode.Default)
            {
                DefaultPageSegMode = PageSegMode.SingleWord
            };

            var text = tesseractEngine.Process(img).GetText();

            return $"{text}";
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

    // TODO: Get rid of this. 
    class ChestGroup
    {
        public Point TopLeft;

        public int CCs = 0;
        public int FJs = 0;
        public int BBs = 0;

        public ChestGroup(Point topLeft)
        {
            this.TopLeft = topLeft;
        }
    }
}
