using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class MapSend
    {
        public short[,] Map { get; set; }// 0 - blank | 1 - ship | -1 - ship drowned
        public int RoomID { get; set; }
        public int PlayerKey { get; set; }


    }
}
