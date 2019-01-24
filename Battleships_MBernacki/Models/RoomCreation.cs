using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class RoomCreation
    {
        [Required]
        public string RoomName { get; set; }
        [Required]
        public string PlayerName { get; set; }
        public string Password { get; set; }
    }
}
