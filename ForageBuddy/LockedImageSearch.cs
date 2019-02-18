using System.Collections.Generic;
using System.Drawing;

namespace ForageBuddy
{
    public static class LockedImageSearch
    {
        public static bool DoesImageExist(this LockedBitmap lockedHaystack, LockedBitmap lockedNeedle)
        {
            for (int hayX = 0; hayX < lockedHaystack.Width; hayX++)
            {
                for (int hayY = 0; hayY < lockedHaystack.Height; hayY++)
                {
                    for (int needleX = 0; needleX < lockedNeedle.Width; needleX++)
                    {
                        for (int needleY = 0; needleY < lockedNeedle.Height; needleY++)
                        {
                            int xLoc = hayX + needleX;
                            int yLoc = hayY + needleY;

                            if (lockedHaystack.GetPixel(xLoc, yLoc) != lockedNeedle.GetPixel(needleX, needleY)) goto NotFound;
                        }
                    }
                    return true;
                    NotFound:;
                }
            }

            return false;
        }

        public static bool ImageFirstLocation(this LockedBitmap lockedHaystack, LockedBitmap lockedNeedle, Point innerSearch, Point outerSearch) // Could replace with a search rectangle
        {
            for (int hayX = 0; hayX < lockedHaystack.Width && hayX < outerSearch.X; hayX++)
            {
                for (int hayY = 0; hayY < lockedHaystack.Height && hayY < outerSearch.Y; hayY++)
                {
                    for (int needleX = 0; needleX < lockedNeedle.Width; needleX++)
                    {
                        for (int needleY = 0; needleY < lockedNeedle.Height; needleY++)
                        {
                            int xLoc = hayX + needleX;
                            int yLoc = hayY + needleY;

                            if (lockedHaystack.GetPixel(xLoc, yLoc) != lockedNeedle.GetPixel(needleX, needleY)) goto NotFound;
                        }
                    }
                    return true;
                    NotFound:;
                }
            }

            return false;
        }

        public static Point GetFirstLocation(this LockedBitmap lockedHaystack, LockedBitmap lockedNeedle)
        {
            for (int hayX = 0; hayX < lockedHaystack.Width; hayX++)
            {
                for (int hayY = 0; hayY < lockedHaystack.Height; hayY++)
                {
                    for (int needleX = 0; needleX < lockedNeedle.Width; needleX++)
                    {
                        for (int needleY = 0; needleY < lockedNeedle.Height; needleY++)
                        {
                            int xLoc = hayX + needleX;
                            int yLoc = hayY + needleY;

                            if (lockedHaystack.GetPixel(xLoc, yLoc) != lockedNeedle.GetPixel(needleX, needleY)) goto NotFound;
                        }
                    }
                    return new Point(hayX, hayY);
                    NotFound:;
                }
            }

            return Point.Empty;
        }

        public static IEnumerable<Point> GetAllOccurences(this LockedBitmap lockedHaystack, LockedBitmap lockedNeedle)
        {
            var pointsFound = new List<Point>();

            for (int hayX = 0; hayX < lockedHaystack.Width; hayX++)
            {
                for (int hayY = 0; hayY < lockedHaystack.Height; hayY++)
                {
                    for (int needleX = 0; needleX < lockedNeedle.Width; needleX++)
                    {
                        for (int needleY = 0; needleY < lockedNeedle.Height; needleY++)
                        {
                            int xLoc = hayX + needleX;
                            int yLoc = hayY + needleY;


                            var needleColour = lockedNeedle.GetPixel(needleX, needleY);
                            var haystackColour = lockedHaystack.GetPixel(xLoc, yLoc);
                            
                            if (lockedHaystack.GetPixel(xLoc, yLoc) != lockedNeedle.GetPixel(needleX, needleY)) goto NotFound;
                        }
                    }
                    pointsFound.Add(new Point(hayX, hayY));
                    NotFound:;
                }
            }

            return pointsFound;
        }

        public static IEnumerable<Point> GetAllOccurences(this LockedBitmap lockedHaystack, IEnumerable<LockedBitmap> lockedNeedles)
        {
            var pointsFound = new List<Point>();
            
            foreach (var lockedNeedle in lockedNeedles)
            {
                pointsFound.AddRange(GetAllOccurences(lockedNeedle, lockedHaystack));
            }

            return pointsFound;
        }
    }
}
