using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Battleships_MBernacki.Models.Interfaces;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class GameRooms : IGameRooms<GameRoom>
    {
        public List<GameRoom> GameRoomsList { get; set; }
        //private static RNGCryptoServiceProvider RngCsp = new RNGCryptoServiceProvider();



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
                //newId = RngCsp.GetHashCode();
                Random random = new System.Random();
                newId = random.GetHashCode();

                GameRoomsList.ForEach((gr) => {
                    if (gr.RoomID == newId) sameId = true;
                });

            } while (sameId);
            return newId;
        }
    }
}
