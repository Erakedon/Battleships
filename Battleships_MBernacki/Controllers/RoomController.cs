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

            short[] shipList = new short[] { 0, 2, 1, 1 };//Index + 1 is the indicator of ship size
            GameRoom newGameRoom = new GameRoom(_gameRooms.GenerateRoomId(), roomCreation.RoomName, password,6,shipList);
            int playerRoomKey = newGameRoom.AddPlayer(roomCreation.PlayerName);

            //RoomList.Add(new GameRoom(RoomList.Count(), roomCreation.RoomName, password));
            //var newGameRoom = RoomList.Last<GameRoom>();
            _gameRooms.GameRoomsList.Add(newGameRoom);


            return Ok(new JoinedRoomInfo() { PlayerRoomKey = playerRoomKey, RoomID = newGameRoom.RoomID , OponentName = "", RoomName = newGameRoom.RoomName });
        }

        [HttpPost]
        [Route("[action]")]
        [Produces("application/json")]
        public IActionResult JoinRoom([FromBody] RoomJoin roomJoin)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var gameRoom = _gameRooms.GameRoomsList.Find(r => r.RoomID == roomJoin.RoomId);

            if (gameRoom == null) return NotFound("Game room could not be found");
            if (gameRoom.IsRoomFull()) return NotFound("Game room is full");
            if (gameRoom.Password != "" && gameRoom.CheckPassword(roomJoin.RoomPassword)) return BadRequest("Password is incorect");

            int playerRoomKey = gameRoom.AddPlayer(roomJoin.PlayerName);//Trajkaczem obłożyć

            return Ok(new JoinedRoomInfo() { PlayerRoomKey = playerRoomKey, RoomID = gameRoom.RoomID, OponentName = "", RoomName = gameRoom.RoomName });
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json")]
        public IActionResult GetRoomList()
        {
            List<RoomInfo> avalibleRooms = new List<RoomInfo>();
            _gameRooms.GameRoomsList.ForEach(room =>
            {
                if (!room.IsRoomFull())
                    avalibleRooms.Add(new RoomInfo()
                    {
                        RoomID = room.RoomID,
                        OwnerName = room.PlayersNames[0],
                        RoomName = room.RoomName,
                        RequirePassword = room.RequirePassword
                    });
            });

            return Ok(avalibleRooms);
        }

        [HttpPost]
        [Route("[action]")]
        [Produces("application/json")]
        public IActionResult PostMap([FromBody] MapSend mapInfo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var gameRoom = _gameRooms.GameRoomsList.Find(r => r.RoomID == mapInfo.RoomID);

            if(gameRoom == null) return BadRequest("Wrong Room Id");

            var playerRoomId = gameRoom.GetPlayerRoomId(mapInfo.PlayerKey);
            if (playerRoomId != 0 && playerRoomId != 1) return BadRequest("Wrong Player Key");

            //var shipsMap = new ShipsMap(mapInfo.Map,gameRoom.MapSize,gameRoom.ShipList);
            bool isMapOk = gameRoom.AddMap(mapInfo.PlayerKey, mapInfo.Map, gameRoom.MapSize, gameRoom.ShipList);
            if(!isMapOk) return BadRequest("Map is not correct");


            //gameRoom.Maps[playerRoomId] = new ShipsMap(mapInfo.Map);


            return Ok(new RoomstateInfo()
            {
                RoomID = gameRoom.RoomID,
                OponentName = gameRoom.PlayersNames[(playerRoomId + 1) % 2],
                AskingPlayerTurn = gameRoom.IsPlayerTurn(mapInfo.PlayerKey),
                LastAction = gameRoom.LastAction,
                GameOn = gameRoom.GameOn
            });
        }

        [HttpPost]
        [Route("[action]")]
        [Produces("application/json")]
        public IActionResult Gamestate([FromBody] GamestateQuery query)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var gameRoom = _gameRooms.GameRoomsList.Find(r => r.RoomID == query.RoomID);

            if (gameRoom == null) return BadRequest("Wrong Room Id");

            var playerRoomId = gameRoom.GetPlayerRoomId(query.PlayerKey);
            if (playerRoomId != 0 && playerRoomId != 1) return BadRequest("Wrong Player Key");
                       

            return Ok(new RoomstateInfo() {
                RoomID = gameRoom.RoomID,
                OponentName = gameRoom.PlayersNames[(playerRoomId + 1) % 2],
                AskingPlayerTurn = gameRoom.IsPlayerTurn(query.PlayerKey),
                LastAction = gameRoom.LastAction,
                GameOn = gameRoom.GameOn
            });
        }

        [HttpPost]
        [Route("[action]")]
        [Produces("application/json")]
        public IActionResult Shoot([FromBody] PlayerShootInfo shootInfo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var gameRoom = _gameRooms.GameRoomsList.Find(r => r.RoomID == shootInfo.RoomID);

            if (gameRoom == null) return BadRequest("Wrong Room Id");

            var playerRoomId = gameRoom.GetPlayerRoomId(shootInfo.PlayerKey);
            if (playerRoomId != 0 && playerRoomId != 1) return BadRequest("Wrong Player Key");

            string result = gameRoom.PlayerShootReq(shootInfo.PlayerKey, shootInfo.X, shootInfo.Y);

            return Ok(new RoomstateInfo()
            {
                RoomID = gameRoom.RoomID,
                OponentName = gameRoom.PlayersNames[(playerRoomId + 1) % 2],
                AskingPlayerTurn = gameRoom.IsPlayerTurn(shootInfo.PlayerKey),
                LastAction = gameRoom.LastAction,
                GameOn = gameRoom.GameOn
            });
        }




    }
}