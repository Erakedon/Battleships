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
        private List<int> RoomPlayersKeys { get; set; } = new List<int>(2);
        public List<string> PlayersNames { get; set; } = new List<string>(2);
        public List<ShipsMap> Maps { get; set; } = new List<ShipsMap>(2);
        public short MapSize { get; set; }
        public short[] ShipList { get; set; }
        private string Password { get; set; }
        public bool RequirePassword { get; }

        public int CurrentPlayerTurn { get; set; }// 0-first player turn | 1-second player turn
        public PlayerAction LastAction { get; set; }
        public bool GameOn { get; set; } = false;//True if both players set their maps
        

        public GameRoom(int roomID, string roomName, string password, short mapsize, short[] shipList)
        {
            RoomID = roomID;
            RoomName = roomName;
            MapSize = mapsize;
            ShipList = shipList;
            Password = password;
            if (password == "") RequirePassword = false;
            else RequirePassword = true;

            CurrentPlayerTurn = 1;
        }

        public int AddPlayer(string name)
        {
            if (IsRoomFull())
            {
                throw new InvalidOperationException("Room is full");
            }

            PlayersNames.Add(name);
            
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
            int id = 0;
            id = RoomPlayersKeys.FindIndex(k => k == playerkey);
            return id;
        }

        public bool MapsReady()
        {
            if (Maps[0] == null || Maps[1] == null) return false;

            return true;
        }

        public bool IsRoomFull()
        {
            if (PlayersNames.Count() == 2) return true;
            else return false;
        }

        public bool AddMap(int playerKey,short[][] map, short mapSize, short[] shipList)
        {
            int playerId = GetPlayerRoomId(playerKey);
            //if (playerId == 0) return false;

            ShipsMap shipsMap = new ShipsMap(map, mapSize, shipList);
            if (!shipsMap.ValidateMap()) return false;

            Maps.Add(shipsMap);
            //Maps[playerId] = shipsMap;

            if (Maps.Count == 2) GameOn = true;

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
            }
            LastAction = new PlayerAction() { PlayerId = playerId, Result = result, X = x, Y = y, GameWinner = playerWon };
            CurrentPlayerTurn = (CurrentPlayerTurn + 1) % 2;

            return result;
        }

        public bool IsPlayerTurn(int playerKey)
        {
            return GetPlayerRoomId(playerKey) == CurrentPlayerTurn;
        }

    }
}
