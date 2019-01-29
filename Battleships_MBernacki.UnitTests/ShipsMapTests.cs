using Battleships_MBernacki.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships_MBernacki.UnitTests
{
    [TestFixture]
    class ShipsMapTests
    {
        //ShipsMapTests _shipsMap;

        //short _mapSize;
        //short[] _shipList;

        //short[][] _properMap;
        //short[][] _badMap1;

        //short[][] _map2x2;
        //short[][] _map3x3;
        //short[][] _map4x4;
        //short[][] _map5x5;

        //[SetUp]
        //public void Setup()
        //{
        //    _map2x2 = new short[][] { new short[] { 0, 0 },
        //                              new short[] { 0, 0 } };

        //}

        readonly short[][][] map =
        {
            new short[][] { new short[] { 1, 0 },
                            new short[] { 1, 0 } },

            new short[][] { new short[] { 1, 0, 1 },
                            new short[] { 1, 0, 1 },
                            new short[] { 0, 0, 1 }},

            new short[][] { new short[] { 1, 0, 1, 1 },
                            new short[] { 1, 0, 0, 0 },
                            new short[] { 1, 0, 1, 0 },
                            new short[] { 1, 0, 1, 0 }}

        };

        static short[] mapSizes =
        {
            2, 3, 4
        };

        static short[][] shipsLists =
        {
            new short[] { 0, 1, 0, 0},
            new short[] { 0, 1, 1, 0},
            new short[] { 0, 2, 0, 1}
        };


        //[Test]
        ////[TestCaseSource("maps", "mapSizes", "shipsLists")]
        ////[TestCase(arg1: _map2x2, arg2: _mapSize, arg3: _shipList)]
        //[TestCase(
        //    new short[][] { new short[] { 1, 0 }, new short[] { 1, 0 } },
        //    2, 
        //    new short[] { 0, 1, 0, 0 })]
        //public void ValidateMap_SetsProperMapMapSizeAndShipList(short[][] map, short mapSize, short[] shipsList)
        //{

        //}

        //[Test]
        //public void Test(
        //[Values(map[0],map[1],map[2])]    short[][] map, 
        //    short mapSize, 
        //    short[] shipsList)
        //{

        //}


    }
}
