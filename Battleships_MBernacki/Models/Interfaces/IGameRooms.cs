using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public interface IGameRooms<T>
    {
        List<T> GameRoomsList { get; set; }

        int GenerateRoomId();
        void RemoveFinishedRooms();
    }
}
