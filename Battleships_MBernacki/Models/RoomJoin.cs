using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class RoomJoin
    {
        [Required]
        public int RoomId { get; set; }
        //public string RoomPassword { get; set; }
    }
}
