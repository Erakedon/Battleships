using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleships_MBernacki.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Battleships_MBernacki.Areas.Identity.Data;

namespace Battleships_MBernacki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private IGameRooms<GameRoom> _gameRooms { get; set; }
        private readonly UserManager<Battleships_MBernackiUser> _userManager;

        public RoomController(
            IGameRooms<GameRoom> gameRooms,
            UserManager<Battleships_MBernackiUser> userManager)
        {
            _gameRooms = gameRooms;
            _userManager = userManager;
        }

        private Task<Battleships_MBernackiUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);



        [HttpPost]
        [Route("[action]")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> CreateRoom([FromBody] RoomCreation roomCreation)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            short mapSize = 6;
            short[] shipList = new short[] { 0, 2, 0, 0 };


            if (roomCreation.MapSize != null && roomCreation.MapSize >= 3 && roomCreation.MapSize <= 8)
                mapSize = roomCreation.MapSize;

            bool flag = true;

            if (roomCreation.ShipsList.Length != 4)
                flag = false;
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (roomCreation.ShipsList[i] < 0 || roomCreation.ShipsList[i] >= (6 - i))
                        flag = false;
                }
            }

            if (flag)
                shipList = roomCreation.ShipsList;


            GameRoom newGameRoom = new GameRoom(_gameRooms.GenerateRoomId(), roomCreation.RoomName,mapSize,shipList);

            Battleships_MBernackiUser user = await GetCurrentUserAsync();

            int playerRoomKey = newGameRoom.AddPlayer(user.UserName, user.Id);

            //RoomList.Add(new GameRoom(RoomList.Count(), roomCreation.RoomName, password));
            //var newGameRoom = RoomList.Last<GameRoom>();
            _gameRooms.GameRoomsList.Add(newGameRoom);


            return Ok(new JoinedRoomInfo() {
                PlayerRoomKey = playerRoomKey,
                RoomID = newGameRoom.RoomID ,
                OponentName = "",
                RoomName = newGameRoom.RoomName,
                ShipsList = shipList,
                MapSize = mapSize });
        }

        [HttpPost]
        [Route("[action]")]
        [Produces("application/json")]
        [Authorize]
        public async Task<IActionResult> JoinRoom([FromBody] RoomJoin roomJoin)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var gameRoom = _gameRooms.GameRoomsList.Find(r => r.RoomID == roomJoin.RoomId);

            if (gameRoom == null) return NotFound("Game room could not be found");
            if (gameRoom.IsRoomFull()) return NotFound("Game room is full");
            //if (gameRoom.RequirePassword  && gameRoom.CheckPassword(roomJoin.RoomPassword)) return BadRequest("Password is incorect");
            //int playerRoomKey;

            Battleships_MBernackiUser user = await GetCurrentUserAsync();


            int playerRoomKey = gameRoom.AddPlayer(user.UserName, user.Id);

            return Ok(new JoinedRoomInfo() {
                PlayerRoomKey = playerRoomKey,
                RoomID = gameRoom.RoomID,
                OponentName = gameRoom.PlayersNames[0],
                RoomName = gameRoom.RoomName,
                ShipsList = gameRoom.ShipList,
                MapSize = gameRoom.MapSize
            });
        }

        [HttpGet]
        [Route("[action]")]
        [Produces("application/json")]
        //[Authorize]
        public IActionResult GetRoomList()
        {
            List<RoomInfo> avalibleRooms = new List<RoomInfo>();
            _gameRooms.RemoveFinishedRooms();
            _gameRooms.GameRoomsList.ForEach(room =>
            {
                if (!room.IsRoomFull())
                    avalibleRooms.Add(new RoomInfo()
                    {
                        RoomID = room.RoomID,
                        OwnerName = room.PlayersNames[0],
                        RoomName = room.RoomName,
                    });
            });

            return Ok(avalibleRooms);
        }

        [HttpPost]
        [Route("[action]")]
        [Produces("application/json")]
        [Authorize]
        public IActionResult PostMap([FromBody] MapSend mapInfo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var gameRoom = _gameRooms.GameRoomsList.Find(r => r.RoomID == mapInfo.RoomID);

            if(gameRoom == null) return BadRequest("Wrong Room Id");

            var playerRoomId = gameRoom.GetPlayerRoomId(mapInfo.PlayerKey);
            if (playerRoomId != 0 && playerRoomId != 1) return BadRequest("Wrong Player Key");
            if(gameRoom.GameOn) return BadRequest("Map is already set");
            //var shipsMap = new ShipsMap(mapInfo.Map,gameRoom.MapSize,gameRoom.ShipList);
            bool isMapOk = gameRoom.AddMap(mapInfo.PlayerKey, mapInfo.Map);
            if(!isMapOk) return BadRequest("Map is not correct");


            //gameRoom.Maps[playerRoomId] = new ShipsMap(mapInfo.Map);
            string oponentName = "";
            if (gameRoom.IsRoomFull()) oponentName = gameRoom.PlayersNames[(playerRoomId + 1) % 2];

            return Ok(new RoomstateInfo()
            {
                RoomID = gameRoom.RoomID,
                OponentName = oponentName,
                AskingPlayerTurn = gameRoom.IsPlayerTurn(mapInfo.PlayerKey),
                LastAction = gameRoom.LastAction,
                GameOn = gameRoom.GameOn
            });
        }

        [HttpPost]
        [Route("[action]")]
        [Produces("application/json")]
        [Authorize]
        public IActionResult Gamestate([FromBody] GamestateQuery query)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var gameRoom = _gameRooms.GameRoomsList.Find(r => r.RoomID == query.RoomID);

            if (gameRoom == null) return BadRequest("Wrong Room Id");

            var playerRoomId = gameRoom.GetPlayerRoomId(query.PlayerKey);
            if (playerRoomId != 0 && playerRoomId != 1) return BadRequest("Wrong Player Key");

            string oponentName = "";
            if (gameRoom.IsRoomFull()) oponentName = gameRoom.PlayersNames[(playerRoomId + 1) % 2];

            return Ok(new RoomstateInfo() {
                RoomID = gameRoom.RoomID,
                OponentName = oponentName,
                AskingPlayerTurn = gameRoom.IsPlayerTurn(query.PlayerKey),
                LastAction = gameRoom.LastAction,
                GameOn = gameRoom.GameOn
            });
        }

        [HttpPost]
        [Route("[action]")]
        [Produces("application/json")]
        [Authorize]
        public IActionResult Shoot([FromBody] PlayerShootInfo shootInfo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var gameRoom = _gameRooms.GameRoomsList.Find(r => r.RoomID == shootInfo.RoomID);

            if (gameRoom == null) return BadRequest("Wrong Room Id");

            var playerRoomId = gameRoom.GetPlayerRoomId(shootInfo.PlayerKey);
            if (playerRoomId != 0 && playerRoomId != 1) return BadRequest("Wrong Player Key");

            if(!gameRoom.GameOn) return BadRequest("Players did not sets theirs maps yet");

            string result = gameRoom.PlayerShootReq(shootInfo.PlayerKey, shootInfo.X, shootInfo.Y);

            //if (result == "hit")
            //{
            //    _gameRooms.ResolveRoom(gameRoom.RoomID);
            //}

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