using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models.Interfaces
{
    public interface IShipsMap
    {
        bool ValidateMap();
        string Shoot(int x, int y);
    }
}
