using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class RoomInfo
    {
        public string RoomName { get; set; }
        public string[] PlayersNames { get; set; }
        public bool RequirePassword { get; set; }
    }
}
