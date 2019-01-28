using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Battleships_MBernacki.Models
{
    public class GamestateQuery
    {
        [Required]
        public int RoomID { get; set; }
        [Required]
        public int PlayerKey { get; set; }
    }
}
