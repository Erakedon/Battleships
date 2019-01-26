using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleships_MBernacki.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Battleships_MBernacki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private List<GameRoom> RoomList { get; set; } = new List<GameRoom>();

        //[HttpGet]
        //[Produces("application/json")]
        //public IActionResult CreateRoom([FromBody] RoomCreation roomCreation)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    string password;
        //    if (roomCreation == null) password = "";
        //    else password = roomCreation.Password;

        //    GameRoom newGameRoom = new GameRoom(RoomList.Count(), roomCreation.RoomName, password);
        //    RoomList.Add(newGameRoom);
        //    int playerRoomKey = newGameRoom.AddPlayer(roomCreation.PlayerName);

        //    return Ok(new JoinedRoomInfo() { PlayerRoomKey = playerRoomKey, OponentName = "", RoomName = newGameRoom.RoomName });
        //}

        //[HttpGet]
        //[Produces("application/json")]
        //public IActionResult JoinRoom([FromBody] RoomJoin roomJoin)
        //{
        //    if (!ModelState.IsValid) return BadRequest(ModelState);

        //    GameRoom gameRoom = RoomList.Find(r => r.RoomID == roomJoin.RoomId);

        //    if (gameRoom == null) return NotFound("Game room could not be found");
        //    if (!gameRoom.Instantiating) return NotFound("Game room is full");
        //    if (gameRoom.Password != "" && gameRoom.CheckPassword(roomJoin.RoomPassword)) return BadRequest("Password is incorect");

        //    int playerRoomKey = gameRoom.AddPlayer(roomJoin.PlayerName);

        //    return Ok(new JoinedRoomInfo() { PlayerRoomKey = playerRoomKey, OponentName = "", RoomName = gameRoom.RoomName });
        //}

        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetRoomList()
        {
            GameRoom newGameRoom = new GameRoom(123, "Halko", "");
            newGameRoom.AddPlayer("Kazik");
            RoomList.Add(newGameRoom);


            List<RoomInfo> avalibleRooms = new List<RoomInfo>();
            RoomList.ForEach(room =>
            {
                if (room.Instantiating)
                    avalibleRooms.Add(new RoomInfo()
                    {
                        OwnerName = room.PlayersNames[0],
                        RoomName = room.RoomName,
                        RequirePassword = room.RequirePassword
                    });
            });

            return Ok(avalibleRooms.ToArray());
        }


    }
}