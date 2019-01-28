using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Battleships_MBernacki.Models
{
    public class GameRoom
    {
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        private List<int> RoomPlayersKeys { get; set; } = new List<int>(2);
        public List<string> PlayersNames { get; set; } = new List<string>(2);
        public List<ShipsMap> Maps { get; set; } = new List<ShipsMap>(2);
        public string Password { get; set; }
        public bool RequirePassword { get; }
        public short CurrentPlayerTurn { get; }// 0-first player turn | 1-second player turn
        //public bool Instantiating = true;//Waiting for second player
        //public bool GameStart = false;//True if both players set their maps

        //private static RNGCryptoServiceProvider RngCsp = new RNGCryptoServiceProvider();

        public GameRoom(int roomID, string roomName, string password)
        {
            RoomID = roomID;
            RoomName = roomName;
            //RoomPlayersKeys = new List<int>(2);
            Password = password;
            if (password == "") RequirePassword = false;
            else RequirePassword = true;

            CurrentPlayerTurn = 1;
        }

        public int AddPlayer(string name)
        {
            //if(IsRoomFull())
            //{
            //    throw new InvalidOperationException("Room is full");
            //}

            PlayersNames.Add(name);

            //int playerSecretKey = RngCsp.GetHashCode();
            Random random = new System.Random();
            int playerSecretKey = random.GetHashCode();
            RoomPlayersKeys.Add(playerSecretKey);

            return playerSecretKey;
        }

        public bool CheckPassword(string password)
        {
            if (Password == password) return true;
            else return false;
        }

        public int GetPlayerRoomId(int playerkey)
        {
            int id = RoomPlayersKeys.Find(k => k == playerkey);
            return id;
        }

        public bool MapsReady()
        {
            if (Maps[0] == null || Maps[1] == null) return false;

            return true;
        }

        public bool IsRoomFull()
        {
            //if (PlayersNames[0] == null || PlayersNames[1] == null) return false;

            if (PlayersNames.Count() == 2) return true;
            else return false;
        }

    }
}
