using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Battleships_MBernacki.Models.Interfaces;
using System.Threading.Tasks;
using System.Timers;

namespace Battleships_MBernacki.Models
{
    public class GameRooms : IGameRooms<GameRoom>
    {
        public List<GameRoom> GameRoomsList { get; set; }
        private Queue<Timer> Timers;


        public GameRooms()
        {
            GameRoomsList = new List<GameRoom>();
        }

        public int GenerateRoomId()
        {
            int newId;
            bool sameId;
            do
            {
                sameId = false;
                Random random = new System.Random();
                newId = random.GetHashCode();

                GameRoomsList.ForEach((gr) => {
                    if (gr.RoomID == newId) sameId = true;
                });

            } while (sameId);
            return newId;
        }


        public void RemoveFinishedRooms()
        {
            for (int i = 0; i < GameRoomsList.Count(); i++)
            {
                if (GameRoomsList[i].DeleteTime > DateTime.MinValue && GameRoomsList[i].DeleteTime < DateTime.Now)
                    GameRoomsList.RemoveAt(i);
            }
        }       
    }
}
