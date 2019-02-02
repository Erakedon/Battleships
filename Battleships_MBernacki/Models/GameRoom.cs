using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Battleships_MBernacki.Models.Interfaces;

namespace Battleships_MBernacki.Models
{
    public class GameRoom : IGameRoom
    {
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        private List<int> RoomPlayersKeys { get; set; }
        public List<string> PlayersNames { get; set; }
        public List<string> PlayerDatabaseId { get; set; }
        private ShipsMap[] Maps { get; set; }
        public short MapSize { get; }
        public short[] ShipList { get; }
        //private string Password { get; set; }
        //public bool RequirePassword { get; }

        public int CurrentPlayerTurn { get; set; }// 0-first player turn | 1-second player turn
        public PlayerAction LastAction { get; set; }
        public bool GameOn { get; set; } = false;//True if both players set their maps
        public DateTime DeleteTime { get; set; }


        public GameRoom(int roomID, string roomName, string password, short mapsize, short[] shipList)
        {
            RoomID = roomID;
            RoomName = roomName;
            MapSize = mapsize;
            ShipList = shipList;
            //Password = password;
            //if (password == "") RequirePassword = false;
            //else RequirePassword = true;

            CurrentPlayerTurn = 0;

            RoomPlayersKeys = new List<int>(2);
            PlayersNames = new List<string>(2);
            PlayerDatabaseId = new List<string>(2);
            Maps = new ShipsMap[2];
        }

        public int AddPlayer(string name, string databaseId)
        {
            if (IsRoomFull())
            {
                throw new InvalidOperationException("Room is full");
            }

            PlayersNames.Add(name);
            PlayerDatabaseId.Add(databaseId);
            
            Random random = new System.Random();
            int playerSecretKey = random.GetHashCode();
            RoomPlayersKeys.Add(playerSecretKey);

            return playerSecretKey;
        }

        //public bool SetPassword(string oldPassword, string newPassword)
        //{
        //    if (Password == "" || Password == oldPassword) Password = newPassword;
        //    else return false;

        //    return true;
        //}

        //public bool CheckPassword(string password)
        //{
        //    if (Password == password) return true;
        //    else return false;
        //}

        public int GetPlayerRoomId(int playerkey)
        {
            int id = -1;
            id = RoomPlayersKeys.FindIndex(k => k == playerkey);
            return id;
        }

        //public bool MapsReady()
        //{
        //    if (Maps.Count() == 2) return true;

        //    return false;
        //}

        public bool IsRoomFull()
        {
            if (PlayersNames.Count() == 2) return true;
            else return false;
        }

        public bool AddMap(int playerKey,short[][] map)
        {
            int playerId = GetPlayerRoomId(playerKey);
            if (playerId == -1) return false;

            if (GameOn) return false;

            ShipsMap shipsMap = new ShipsMap(map, MapSize, ShipList);
            if (!shipsMap.ValidateMap()) return false;

            //Maps.Insert(playerId, shipsMap);

            //if (Maps.Count == 2) GameOn = true;

            Maps[playerId] = shipsMap;

            if (Maps[0] != null && Maps[1] != null) GameOn = true;

            return true;
        }

        public string PlayerShootReq(int playerKey, int x, int y)
        {
            if (!GameOn) return "Game have not started";
            if (x < 0 || y < 0 || x > MapSize || y > MapSize) return "Wrong Coordinates";

            int playerId = GetPlayerRoomId(playerKey);

            if (playerId != CurrentPlayerTurn) return "Not player turn";

            string result = Maps[(playerId + 1) % 2].Shoot(x, y);
            bool playerWon = false;
            if (result == "win")
            {
                GameOn = false;
                playerWon = true;
                GameResolve();
            }
            LastAction = new PlayerAction() { PlayerId = playerId, PlayerName = PlayersNames[playerId], Result = result, X = x, Y = y, GameWinner = playerWon };
            CurrentPlayerTurn = (CurrentPlayerTurn + 1) % 2;

            return result;
        }

        public bool IsPlayerTurn(int playerKey)
        {
            return GetPlayerRoomId(playerKey) == CurrentPlayerTurn;
        }

        private void GameResolve()
        {
            DeleteTime = DateTime.Now.AddMinutes(1);
        }

    }
}
