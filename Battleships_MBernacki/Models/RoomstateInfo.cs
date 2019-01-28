using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class RoomstateInfo
    {
        public int RoomID { get; set; }
        public bool AskingPlayerTurn { get; set; }
        public PlayerAction LastAction { get; set; }
        public bool GameOn { get; set; }

    }
}
