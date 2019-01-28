using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class ShipsMap
    {
        public short[,] Map { get; set; }// 0 - blank | 1 - ship | -1 - miss | -2 - ship drowned

        public ShipsMap(short[,] map)
        {
            Map = map;
        }
    }
}
