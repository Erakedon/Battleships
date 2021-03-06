﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class MapSend
    {
        [Required]
        public short[][] Map { get; set; }// 0 - blank | 1 - ship | -1 - ship drowned
        [Required]
        public int RoomID { get; set; }
        [Required]
        public int PlayerKey { get; set; }
    }
}
