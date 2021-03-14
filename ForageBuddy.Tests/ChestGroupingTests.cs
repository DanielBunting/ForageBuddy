using LockedBitmapUtil;
using LockedBitmapUtil.Extensions;
using NUnit.Framework;
using System.Drawing;

namespace ForageBuddy.Tests
{
    class ChestGroupingTests
    {
        LockedBitmap _lockedBitmap;
        ChestGrouping _chestGrouping;
        Point _initialTopLeft;

        [SetUp]
        public void Setup()
        {
            _lockedBitmap = new Bitmap(10, 10)
                .ToLockedBitmap();
            _initialTopLeft = new Point(500, 500);


            _chestGrouping = new ChestGrouping(ChestType.BoneBox, _initialTopLeft, _lockedBitmap);
        }

        [Test]
        public void OnConstruction_ImagePersisted_IsTheImageInTheObject()
        {
            Assert.AreEqual(_lockedBitmap, _chestGrouping.NameSector);
        }

        [Test]
        public void OnConstruction_LocationPersisted_IsTheInitialLocation()
        {
            Assert.AreEqual(_initialTopLeft, _chestGrouping.GroupLocation);
        }

        [TestCase(0)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(20)]
        public void AddChest_OnNewBoneBox_UpdatesBoneBoxCounts(int chestsAdded)
        {
            for(int x = 0; x < chestsAdded; x++)
                _chestGrouping.AddChest(ChestType.BoneBox, _initialTopLeft);

            Assert.AreEqual(chestsAdded + 1, _chestGrouping.BoneBox);
        }

        [TestCase(0)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(20)]
        public void AddChest_OnNewFetishJar_UpdatesFetishJarCounts(int chestsAdded)
        {
            for (int x = 0; x < chestsAdded; x++)
                _chestGrouping.AddChest(ChestType.FetishJar, _initialTopLeft);

            Assert.AreEqual(chestsAdded, _chestGrouping.FetishJar);
        }

        [TestCase(0)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(20)]
        public void AddChest_OnNewCursedChest_UpdatesCusedChestCounts(int chestsAdded)
        {
            for (int x = 0; x < chestsAdded; x++)
                _chestGrouping.AddChest(ChestType.CursedChest, _initialTopLeft);

            Assert.AreEqual(chestsAdded, _chestGrouping.CursedChest);
        }

        [Test]
        public void AddChest_OnNewLocationWhenFurtherUpOrLeft_UpdatesLocation()
        {
            var point = new Point(50, 50);

            _chestGrouping.AddChest(ChestType.BoneBox, point);

            Assert.AreEqual(point, _chestGrouping.GroupLocation);
        }

        [Test]
        public void AddChest_OnNewLocationWhenLowerOrRight_DoesNotUpdateLocation()
        {
            var point = new Point(600, 600);

            _chestGrouping.AddChest(ChestType.BoneBox, point);

            Assert.AreEqual(_initialTopLeft, _chestGrouping.GroupLocation);
        }
    }
}
