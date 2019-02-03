let Gameplay = {

    //mapSize = 6,
    //shipsList = [0, 2, 1, 1], //Index + 1 is the indicator of ship size

    //roomData,

    //playerMap,
    //oponentMap,

    initializeGameplay(roomId) {

        this.gameStateDiv = document.querySelector("#gamestate");
        this.gameRoomNameDiv = document.querySelector("#roomName");
        this.oponentNameDiv = document.querySelector("#oponentName");


        this.roomData = JSON.parse(localStorage.getItem(roomId));
        console.log(this.roomData);

        this.gameRoomNameDiv.innerText = this.roomData.roomName;

        this.mapSize = this.roomData.mapSize,
            this.shipsList = this.roomData.shipsList, //Index + 1 is the indicator of ship size

        this.playerMap = new PlayerMap("#playerMap", this.mapSize, this.shipsList,this);
        this.playerMap.generateMap();
        this.gameStateDiv.innerText = "Place your Ships";
        this.playerMap.makeFieldsClickable();

        this.oponentMap = new OponentMap("#oponentMap", this.mapSize, this.shipsList,this);
        this.oponentMap.generateMap();

    },

    watchForTurn() {

        let query = {
            roomID: this.roomData.roomID,
            playerKey: this.roomData.playerRoomKey
        }
        console.log("query", query)

        var options = {};
        options.url = "/api/room/Gamestate";
        options.type = "POST";
        options.contentType = "application/json";
        options.data = JSON.stringify(query);
        options.dataType = "json";
        options.success = (data) => {
            console.log("room state recieved", data);

            if (data.oponentName == "") this.oponentNameDiv.innerText = "Waiting for oponent";
            else this.oponentNameDiv.innerText = "Oponent: " + data.oponentName;

            console.log(data);
            if (data.lastAction && data.lastAction.result == "win") {
                this.gameStateDiv.innerText = data.lastAction.playerName + " have won!";
            }
            else if (data.gameOn) {
                if (data.askingPlayerTurn) {
                    this.gameStateDiv.innerText = "Your Turn!";
                    this.oponentMap.enableMove();
                }
                else {
                    this.gameStateDiv.innerText = "Waiting for oponent";
                    setTimeout(() => { this.watchForTurn() }, 2000);
                }

            }
            else {
                this.gameStateDiv.innerText = "Waiting for oponent";
                setTimeout(() => { this.watchForTurn() }, 2000);
            }

        };
        options.error = (err) => {
            console.log(err);
        };
        $.ajax(options);
    }


}



//let mapSize = 6;
//let shipsList = [0, 2, 1, 1];//Index + 1 is the indicator of ship size

//let oponentCells = new Array(mapSize);
//let playerCells = new Array(mapSize);

//let playerMap;

//function initializeGameplay() {
//    playerMap = new PlayerMap("#playerMap", mapSize, shipsList);

//}


//function generateMap(parent, allegiance) {
//    let arrayReference;
//    if (allegiance == "oponent") arrayReference = oponentCells;
//    else arrayReference = playerCells;
    
//    for (var i = 0; i < mapSize; i++) {

//        let row = document.createElement("div");
//        row.classList.add('mapRow');

//        arrayReference[i] = new Array(mapSize);

//        for (var j = 0; j < mapSize; j++) {
//            let cell = document.createElement("div");
//            cell.classList.add('cell');

//            arrayReference[i][j] = cell;

//            row.appendChild(cell);
//        }

//        document.querySelector(parent).appendChild(row)
//    }

//    console.log(oponentCells);
//    console.log(playerCells);

//}

