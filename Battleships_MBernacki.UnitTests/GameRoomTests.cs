using Moq;
using Battleships_MBernacki.Models;
using Battleships_MBernacki.Models.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battleships_MBernacki.UnitTests
{
    [TestFixture]
    class GameRoomTests
    {
        GameRoom _gameRoom;
        
        short _mapSize;
        short[] _shipsList;

        short[][] _properMap1;
        short[][] _properMap2;
        short[][] _properMap3;

        short[][] _badMap1;


        [SetUp]
        public void Setup()
        {
            _mapSize = 3;
            _shipsList = new short[] { 2, 1, 0, 0 };

            _gameRoom = new GameRoom(123456, "Example Name", "", _mapSize, _shipsList );
            
            _properMap1 = new short[][] { new short[]{ 0, 1, 1 },
                                          new short[]{ 0, 0, 0 },
                                          new short[]{ 1, 0, 1 } };

            _properMap2 = new short[][] { new short[]{ 1, 0, 1 },
                                          new short[]{ 0, 0, 1 },
                                          new short[]{ 1, 0, 0 } };

            _properMap3 = new short[][] { new short[]{ 1, 0, 1 },
                                          new short[]{ 0, 0, 0 },
                                          new short[]{ 1, 1, 0 } };

            _badMap1 = new short[][] { new short[]{ 1, 0, 1 },
                                       new short[]{ 0, 1, 0 },
                                       new short[]{ 1, 1, 0 } };


        }

        [Test]
        public void AddPlayer_PassesCorrectName_MethodReturnsNonNegativeInt()
        {
            int result = _gameRoom.AddPlayer("Nickname","testId");

            Assert.Positive(result);
        }

        [Test]
        public void AddPlayer_AddsTwoPlayers_OutputsAreNotTheSame()
        {
            int result1 = _gameRoom.AddPlayer("Nickname1", "testId");
            int result2 = _gameRoom.AddPlayer("Nickname2", "testId");

            Assert.AreNotEqual(result1, result2);
        }

        [Test]
        public void AddPlayer_AddsThreePlayers_ThrowsExceptionAtThirdTime()
        {
            int result1 = _gameRoom.AddPlayer("Nickname1", "testId");
            int result2 = _gameRoom.AddPlayer("Nickname2", "testId");
            try
            {
                int result3 = _gameRoom.AddPlayer("Nickname3", "testId");
            }
            catch
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetPlayerRoomId_AddsTwoPlayersAndGetsIdOfFirst_RoomIdOfFirstEqualsZero()
        {
            int id1 = _gameRoom.AddPlayer("Nickname1", "testId");
            int id2 = _gameRoom.AddPlayer("Nickname2", "testId");

            int result = _gameRoom.GetPlayerRoomId(id1);

            Assert.AreEqual(0, result);
        }

        [Test]
        public void GetPlayerRoomId_AddsTwoPlayersAndGetsIdOfSecond_RoomIdOfSecondEqualsOne()
        {
            int id1 = _gameRoom.AddPlayer("Nickname1", "testId");
            int id2 = _gameRoom.AddPlayer("Nickname2", "testId");

            int result = _gameRoom.GetPlayerRoomId(id2);

            Assert.AreEqual(1, result);
        }

        [Test]
        public void GetPlayerRoomId_AddsOnePlayerPassesRandomId_ReturnsMinusOne(
            [Random(1000000, 9999999, 5)] int id)
        {
            _gameRoom.AddPlayer("Nickname1", "testId");            

            int result = _gameRoom.GetPlayerRoomId(id);

            Assert.AreEqual(-1, result);
        }

        [Test]
        public void GetPlayerRoomId_AddsTwoPlayersPassesRandomId_ReturnsMinusOne(
            [Random(1000000, 9999999, 5)] int id)
        {
            _gameRoom.AddPlayer("Nickname1", "testId");
            _gameRoom.AddPlayer("Nickname2", "testId");

            int result = _gameRoom.GetPlayerRoomId(id);

            Assert.AreEqual(-1, result);
        }

        [Test]
        public void IsRoomFull_AddNoPlayer_ReturnFalse()
        {
            bool result = _gameRoom.IsRoomFull();

            Assert.False(result);
        }

        [Test]
        public void IsRoomFull_AddOnePlayer_ReturnFalse()
        {
            int id1 = _gameRoom.AddPlayer("Nickname1", "testId");

            bool result = _gameRoom.IsRoomFull();

            Assert.False(result);
        }

        [Test]
        public void IsRoomFull_AddTwoPlayers_ReturnFalse()
        {
            int id1 = _gameRoom.AddPlayer("Nickname1", "testId");
            int id2 = _gameRoom.AddPlayer("Nickname2", "testId");

            bool result = _gameRoom.IsRoomFull();

            Assert.True(result);
        }

        [Test]
        public void AddMap_PassesRandomPlayerIdAndCorrectMap_ReturnsFalse(
            [Random(1000000, 9999999, 5)] int id)
        {
            bool result = _gameRoom.AddMap(id, _properMap1);
            Assert.False(result);
        }

        [Test]
        public void AddMap_AddsPlayerPassesRandomPlayerIdAndCorrectMap_ReturnsFalse(
            [Random(1000000, 9999999, 5)] int id)
        {
            _gameRoom.AddPlayer("Nickname1", "testId");
            bool result = _gameRoom.AddMap(id, _properMap1);
            Assert.False(result);
        }

        [Test]
        public void AddMap_AddsTwoPlayersPassesRandomPlayerIdAndCorrectMap_ReturnsFalse(
            [Random(1000000, 9999999, 5)] int id)
        {
            _gameRoom.AddPlayer("Nickname1", "testId");
            _gameRoom.AddPlayer("Nickname1", "testId");

            bool result = _gameRoom.AddMap(id, _properMap1);
            Assert.False(result);
        }

        [Test]
        public void AddMap_AddsPlayerPassesCorrectIdAndBadMap_ReturnsFalse()
        {
            int id = _gameRoom.AddPlayer("Nickname1", "testId");
            bool result = _gameRoom.AddMap(id, _badMap1);
            Assert.False(result);
        }

        [Test]
        public void AddMap_AddsPlayerPassesCorrectIdAndCorrectMap_ReturnsTrue()
        {
            int id = _gameRoom.AddPlayer("Nickname1", "testId");
            bool result = _gameRoom.AddMap(id, _properMap1);
            Assert.True(result);
        }

        [Test]
        public void AddMap_OnePlayerAddsMap_GameOnValueIsFalse()
        {
            int id = _gameRoom.AddPlayer("Nickname1", "testId");
            _gameRoom.AddMap(id, _properMap1);

            bool result = _gameRoom.GameOn;

            Assert.False(result);
        }

        [Test]
        public void AddMap_TwoPlayersCorrectMaps_GameOnValueIsTrue()
        {
            int id1 = _gameRoom.AddPlayer("Nickname1", "testId");
            _gameRoom.AddMap(id1, _properMap1);
            int id2 = _gameRoom.AddPlayer("Nickname2", "testId");
            _gameRoom.AddMap(id2, _properMap2);

            bool result = _gameRoom.GameOn;

            Assert.True(result);
        }

        [Test]
        public void PlayerShootReq_MapsNotSetPassesProperValues_ReturnsProperString(
            [Values(0, 1, 2)] int x,
            [Values(0, 1, 2)] int y)
        {
            int id = _gameRoom.AddPlayer("Nickname1", "testId");

            string result = _gameRoom.PlayerShootReq(id, x, y);

            Assert.AreEqual("Game have not started", result);
        }


        [Test]
        public void PlayerShootReq_SetsSingleMapSetPassesProperValues_ReturnsProperString(
            [Values(0, 1, 2)] int x,
            [Values(0, 1, 2)] int y)
        {
            int id = _gameRoom.AddPlayer("Nickname1", "testId");
            _gameRoom.AddMap(id, _properMap1);

            string result = _gameRoom.PlayerShootReq(id, x, y);

            Assert.AreEqual("Game have not started", result);
        }

        [Test]
        public void PlayerShootReq_MapsSetAndPassesOutOfBoundValues_ReturnsProperString(
            [Values(-5, -1, 5, 12)] int x,
            [Values(-5, -1, 5, 12)] int y)
        {
            int id1 = _gameRoom.AddPlayer("Nickname1", "testId");
            int id2 = _gameRoom.AddPlayer("Nickname2", "testId");
            _gameRoom.AddMap(id1, _properMap1);
            _gameRoom.AddMap(id2, _properMap2);

            string result = _gameRoom.PlayerShootReq(id1, x, y);

            Assert.AreEqual("Wrong Coordinates", result);
        }

        [Test]
        public void PlayerShootReq_MapsSetAndPassesSecondPlayerIdAndProperCoordinates_ReturnsProperString(
            [Values(0, 1, 2)] int x,
            [Values(0, 1, 2)] int y)
        {
            int id1 = _gameRoom.AddPlayer("Nickname1", "testId");
            int id2 = _gameRoom.AddPlayer("Nickname2", "testId");
            _gameRoom.AddMap(id1, _properMap1);
            _gameRoom.AddMap(id2, _properMap2);

            string result = _gameRoom.PlayerShootReq(id2, x, y);

            Assert.AreEqual("Not player turn", result);
        }

        [Test]
        public void PlayerShootReq_MapsSetAndPassesProperValues_ValuesSavedInLastActionObject(
            [Values(0, 1, 2)] int x,
            [Values(0, 1, 2)] int y)
        {
            int id1 = _gameRoom.AddPlayer("Nickname1", "testId");
            int id2 = _gameRoom.AddPlayer("Nickname2", "testId");
            _gameRoom.AddMap(id1, _properMap1);
            _gameRoom.AddMap(id2, _properMap2);

            string result = _gameRoom.PlayerShootReq(id1, x, y);

            Assert.Multiple(() => {
                Assert.AreEqual(x, _gameRoom.LastAction.X);
                Assert.AreEqual(y, _gameRoom.LastAction.Y);
            });
        }



    }
}
