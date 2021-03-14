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
            
        }

        [Test]
        public void AddChest_OnNewBoneBox_UpdatesBoneBoxCounts()
        {
            
        }

        [Test]
        public void AddChest_OnNewFetishJar_UpdatesFetishJarCounts()
        {

        }

        [Test]
        public void AddChest_OnNewCursedChest_UpdatesCusedChestCounts()
        {

        }

        [Test]
        public void AccChest_OnNewLocationWhenFurtherUpOrLeft_UpdatesLocation()
        { 
        }

        [Test]
        public void AccChest_OnNewLocationWhenLowerOrRight_DoesNotUpdateLocation()
        {
        }
    }
}
