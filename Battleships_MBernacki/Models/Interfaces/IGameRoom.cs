using System;
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
        List<ShipsMap> Maps { get; set; }
        short MapSize { get; set; }
        short[] ShipList { get; set; }
        //bool RequirePassword { get; }

        int CurrentPlayerTurn { get; set; }// 0-first player turn | 1-second player turn
        PlayerAction LastAction { get; set; }
        bool GameOn { get; set; }//True if both players set their maps

        int AddPlayer(string name);
        //bool SetPassword(string oldPassword, string newPassword);
        //bool CheckPassword(string password);
        int GetPlayerRoomId(int playerkey);
        bool MapsReady();
        bool IsRoomFull();
        bool AddMap(int playerKey, short[][] map, short mapSize, short[] shipList);
        string PlayerShootReq(int playerKey, int x, int y);
        bool IsPlayerTurn(int playerKey);
            


    }
}
