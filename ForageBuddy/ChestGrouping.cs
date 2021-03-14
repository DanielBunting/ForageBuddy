using LockedBitmapUtil;
using System.Drawing;

namespace ForageBuddy
{
    public class ChestGrouping
    {
        public Point GroupLocation;
        public int CursedChest { get; private set; } = 0;
        public int FetishJar { get; private set; } = 0;
        public int BoneBox { get; private set; } = 0;
        public LockedBitmap NameSector { get; private set; }

        public ChestGrouping(ChestType initialChest, Point groupLocation, LockedBitmap nameSector)
        {
            GroupLocation = groupLocation;
            AddChest(initialChest, groupLocation);
            NameSector = nameSector;
        }

        public void AddChest(ChestType typeOfChest, Point chestLocation)
        {
            switch (typeOfChest)
            {
                case ChestType.BoneBox:
                    BoneBox++;
                    break;
                case ChestType.FetishJar:
                    FetishJar++;
                    break;
                case ChestType.CursedChest:
                    CursedChest++;
                    break;
            }

            if (chestLocation.Y < GroupLocation.Y) GroupLocation.Y = chestLocation.Y;
            if (chestLocation.X < GroupLocation.X) GroupLocation.X = chestLocation.X;
        }
    }
}
