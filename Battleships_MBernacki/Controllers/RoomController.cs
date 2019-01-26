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
        private IGameRooms<GameRoom> _gameRooms { get; set; }

        public RoomController(IGameRooms<GameRoom> gameRooms)
        {
            _gameRooms = gameRooms;
        }

        //private List<GameRoom> RoomList { get; set; } = new List<GameRoom>();



        [HttpPost]
        [Route("[action]")]
        [Produces("application/json")]
        public IActionResult CreateRoom([FromBody] RoomCreation roomCreation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string password;
            if (roomCreation.Password == null) password = "";
            else password = roomCreation.Password;

            GameRoom newGameRoom = new GameRoom(_gameRooms.GenerateRoomId(), roomCreation.RoomName, password);
            int playerRoomKey = newGameRoom.AddPlayer(roomCreation.PlayerName);

            //RoomList.Add(new GameRoom(RoomList.Count(), roomCreation.RoomName, password));
            //var newGameRoom = RoomList.Last<GameRoom>();
            _gameRooms.GameRoomsList.Add(newGameRoom);


            return Ok(new JoinedRoomInfo() { PlayerRoomKey = playerRoomKey, RoomID = newGameRoom.RoomID , OponentName = "", RoomName = newGameRoom.RoomName });
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json")]
        public IActionResult JoinRoom([FromBody] RoomJoin roomJoin)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var gameRoom = _gameRooms.GameRoomsList.Find(r => r.RoomID == roomJoin.RoomId);

            if (gameRoom == null) return NotFound("Game room could not be found");
            if (!gameRoom.Instantiating) return NotFound("Game room is full");
            if (gameRoom.Password != "" && gameRoom.CheckPassword(roomJoin.RoomPassword)) return BadRequest("Password is incorect");

            int playerRoomKey = gameRoom.AddPlayer(roomJoin.PlayerName);//Trajkaczem obłożyć

            return Ok(new JoinedRoomInfo() { PlayerRoomKey = playerRoomKey, RoomID = gameRoom.RoomID, OponentName = "", RoomName = gameRoom.RoomName });
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json")]
        public IActionResult GetRoomList()
        {
            //GameRoom newGameRoom = new GameRoom(123, "Halko", "");
            //newGameRoom.AddPlayer("Kazik");
            //RoomList.Add(newGameRoom);


            List<RoomInfo> avalibleRooms = new List<RoomInfo>();
            _gameRooms.GameRoomsList.ForEach(room =>
            {
                if (room.Instantiating)
                    avalibleRooms.Add(new RoomInfo()
                    {
                        OwnerName = room.PlayersNames[0],
                        RoomName = room.RoomName,
                        RequirePassword = room.RequirePassword
                    });
            });

            return Ok(avalibleRooms);
        }


    }
}