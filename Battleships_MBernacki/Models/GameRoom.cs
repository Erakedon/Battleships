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
        private List<int> RoomPlayersKeys { get; set; } = new List<int>();
        public List<string> PlayersNames { get; set; } = new List<string>();
        public List<ShipsMap> Maps { get; set; } = new List<ShipsMap>();
        public string Password { get; set; }
        public bool RequirePassword { get; }
        public short CurrentPlayerTurn { get; }// 0-first player turn | 1-second player turn
        public bool Instantiating = true;

        private static RNGCryptoServiceProvider RngCsp = new RNGCryptoServiceProvider();

        public GameRoom(int roomID, string roomName, string password)
        {
            RoomID = roomID;
            RoomName = roomName;
            //RoomPlayersKeys = new List<int>(2);
            Password = password;
            if (password != "") RequirePassword = false;
            else RequirePassword = true;

            CurrentPlayerTurn = 1;
        }

        public int AddPlayer(string name)
        {
            if(!Instantiating)
            {
                throw new InvalidOperationException("Room is full");
            }

            PlayersNames.Add(name);            

            if(PlayersNames.Count() == 2)
            {
                Instantiating = false;
            }

            
            int playerSecretKey = RngCsp.GetHashCode();
            RoomPlayersKeys.Add(playerSecretKey);

            return playerSecretKey;
        }

        public bool IsInstantiating()
        {
            return Instantiating;
        }

        public bool CheckPassword(string password)
        {
            if (Password == password) return true;
            else return false;
        }

    }
}
