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

        Mock<IShipsMap> _playerOneMap;
        Mock<IShipsMap> _playerTwoMap;
        Mock<IShipsMap> _playerThreeMap;

        [SetUp]
        public void Setup()
        {
            _gameRoom = new GameRoom(123456, "Example Name", "", 6, new short[] { 0, 2, 2, 1 });

            _playerOneMap = new Mock<IShipsMap>();
            _playerTwoMap = new Mock<IShipsMap>();
            _playerThreeMap = new Mock<IShipsMap>();


        }

        [Test]
        public void AddPlayer_PassesCorrectName_MethodReturnsNonNegativeInt()
        {
            int result = _gameRoom.AddPlayer("Nickname");

            Assert.Positive(result);

        }

        [Test]
        public void AddPlayer_AddsTwoPlayers_ChecksIfOutputIntsAreNotTheSame()
        {
            int result1 = _gameRoom.AddPlayer("Nickname1");
            int result2 = _gameRoom.AddPlayer("Nickname2");

            Assert.AreNotEqual(result1, result2);

        }

        [Test]
        public void AddPlayer_AddsThreePlayers_ThrowsExceptionAtThirdTime()
        {
            int result1 = _gameRoom.AddPlayer("Nickname1");
            int result2 = _gameRoom.AddPlayer("Nickname2");
            try
            {
                int result3 = _gameRoom.AddPlayer("Nickname3");
            }
            catch
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetPlayerRoomId_AddsTwoPlayers_ChecksIfRoomIdOfFirstEqualsZero()
        {
            int id1 = _gameRoom.AddPlayer("Nickname1");
            int id2 = _gameRoom.AddPlayer("Nickname2");

            int result = _gameRoom.GetPlayerRoomId(id1);

            Assert.AreEqual(0, result);

        }

        [Test]
        public void GetPlayerRoomId_AddsTwoPlayers_ChecksIfRoomIdOfSecondEqualsOne()
        {
            int id1 = _gameRoom.AddPlayer("Nickname1");
            int id2 = _gameRoom.AddPlayer("Nickname2");

            int result = _gameRoom.GetPlayerRoomId(id2);

            Assert.AreEqual(1, result);

        }

        [Test]
        public void MapsReady_NoMapArePassed_ReturnFalse()
        {
            bool result = _gameRoom.MapsReady();

            Assert.False(result);

        }


        [Test]
        public void MapsReady_SingleMapIsPassed_ReturnFalse()
        {



            bool result = _gameRoom.MapsReady();

            Assert.False(result);

        }



    }
}
