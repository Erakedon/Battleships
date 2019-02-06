using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class PlayerAction
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }

        public string Result { get; set; }
        public bool GameWinner { get; set; }
    }
}
