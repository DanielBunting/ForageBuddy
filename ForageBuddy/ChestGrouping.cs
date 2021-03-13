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
            this.GroupLocation = groupLocation;
            this.AddChest(initialChest, groupLocation);
            this.NameSector = nameSector;
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
