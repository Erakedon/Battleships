﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models.Interfaces
{
    public interface IGameRoom
    {
        int RoomID { get; set; }
        string RoomName { get; set; }
        List<string> PlayersNames { get; set; }
        List<string> PlayerDatabaseId { get; set; }
        //List<ShipsMap> Maps { get; set; }
        short MapSize { get; }
        short[] ShipList { get; }
        //bool RequirePassword { get; }

        int CurrentPlayerTurn { get; set; }// 0-first player turn | 1-second player turn
        PlayerAction LastAction { get; set; }
        bool GameOn { get; set; }//True if both players set their maps
        DateTime DeleteTime { get; set; }

        int AddPlayer(string name, string databaseId);
        //bool SetPassword(string oldPassword, string newPassword);
        //bool CheckPassword(string password);
        int GetPlayerRoomId(int playerkey);
        //bool MapsReady();
        bool IsRoomFull();
        bool AddMap(int playerKey, short[][] map);
        string PlayerShootReq(int playerKey, int x, int y);
        bool IsPlayerTurn(int playerKey);



    }
}
