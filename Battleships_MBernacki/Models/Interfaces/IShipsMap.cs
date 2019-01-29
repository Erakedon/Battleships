using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models.Interfaces
{
    public interface IShipsMap
    {
        short[][] Map { get; set; }// 0 - blank | 1 - ship | -1 - miss | -2 - ship drowned
        short MapSize { get; set; }
        short[] ShipList { get; set; }

        bool ValidateMap();
        string Shoot(int x, int y);


    }
}
