using Battleships_MBernacki.Models;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Battleships_MBernacki.UnitTests
{

    [TestFixture]
    class ShipsMapTests
    {
        readonly short[][][] maps =
        {
            new short[][] { new short[] { 1, 0 },
                            new short[] { 1, 0 } },

            new short[][] { new short[] { 1, 0, 1 },
                            new short[] { 1, 0, 1 },
                            new short[] { 0, 0, 1 }},

            new short[][] { new short[] { 1, 0, 1, 1 },
                            new short[] { 1, 0, 0, 0 },
                            new short[] { 1, 0, 0, 0 },
                            new short[] { 0, 0, 1, 1 }},

            new short[][] { new short[] { 1, 0, 1, 1, 1 },
                            new short[] { 1, 0, 0, 0, 0 },
                            new short[] { 1, 0, 1, 1, 1 },
                            new short[] { 0, 0, 0, 0, 0 },
                            new short[] { 0, 1, 1, 1, 1 }}

        };

        static short[] mapSizes = new short[]
        {
            2, 3, 4, 5
        };

        static short[][] shipsLists =
        {
            new short[] { 0, 1, 0, 0},
            new short[] { 0, 1, 1, 0},
            new short[] { 0, 2, 1, 0},
            new short[] { 0, 0, 3, 1}
        };

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ValidateMap_SetsProperMapMapSizeAndShipList_ReturnsTrue(int id)
        {
            ShipsMap shipsMap = new ShipsMap(maps[id], mapSizes[id], shipsLists[id]);
            bool result = shipsMap.ValidateMap();
            Assert.True(result);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void ValidateMap_SetsProperMapAndMapSizeAndWrongShipList_ReturnsFalse(int id)
        {
            short[] wrongShipList = { 3, 3, 3, 3 };

            ShipsMap shipsMap = new ShipsMap(maps[id], mapSizes[id], wrongShipList);
            bool result = shipsMap.ValidateMap();
            Assert.False(result);
        }

        [Test]
        public void ValidateMap_SetsProperMapAndShipListAndWrongMapSize_ReturnsFalse(
           [Values(0,1,2,3)] int id, [Values(-5, 1, 0, 20)] short mapSize)
        {

            ShipsMap shipsMap = new ShipsMap(maps[id], mapSize, shipsLists[id]);
            bool result = shipsMap.ValidateMap();
            Assert.False(result);
        }

        [Test]
        [TestCase(0,1,0)]
        [TestCase(1,2,2)]
        public void Shoot_SetsCoordinatesOnShip_ReturnHitString(int id, int x, int y)
        {

            ShipsMap shipsMap = new ShipsMap(maps[id], mapSizes[id], shipsLists[id]);
            shipsMap.ValidateMap();
            string result = shipsMap.Shoot(x,y);
            Assert.AreEqual("hit",result);
        }

        [Test]
        [TestCase(0, 1, 1)]
        [TestCase(1, 2, 0)]
        public void Shoot_SetsCoordinatesOnBlank_ReturnMissString(int id, int x, int y)
        {

            ShipsMap shipsMap = new ShipsMap(maps[id], mapSizes[id], shipsLists[id]);
            shipsMap.ValidateMap();
            string result = shipsMap.Shoot(x, y);
            Assert.AreEqual("miss", result);
        }




    }
}
