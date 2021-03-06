﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class JoinedRoomInfo
    {
        public string RoomName { get; set; }
        public int RoomID { get; set; }
        public string OponentName { get; set; }
        public int PlayerRoomKey { get; set; }
        public short[] ShipsList { get; set; }
        public short MapSize { get; set; }
    }
}
