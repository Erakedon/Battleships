let Gameplay = {

    initializeGameplay(roomId) {

        this.gameStateDiv = document.querySelector("#gamestate");
        this.gameRoomNameDiv = document.querySelector("#roomName");
        this.oponentNameDiv = document.querySelector("#oponentName");


        this.roomData = JSON.parse(localStorage.getItem(roomId));
        console.log(this.roomData);
        if (!this.roomData) {
            joinRoom(roomId);
            //this.initializeGameplay(roomId);
            return;
        }

        this.gameRoomNameDiv.innerText = this.roomData.roomName;

        this.mapSize = this.roomData.mapSize,
            this.shipsList = this.roomData.shipsList, //Index + 1 is the indicator of ship size

        this.playerMap = new PlayerMap("#playerMap", this.mapSize, this.shipsList,this);
        this.playerMap.generateMap();
        this.gameStateDiv.innerText = "Place your Ships";
        this.playerMap.makeFieldsClickable();

        this.oponentMap = new OponentMap("#oponentMap", this.mapSize, this.shipsList,this);
        this.oponentMap.generateMap();
        this.oponentMap.hideMap();

        this.shipsListDiv = document.querySelector("#shipsList");

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

                    if (data.lastAction) {


                        switch (data.lastAction.result) {
                            case "hit":
                                this.playerMap.mapTokens[data.lastAction.x][data.lastAction.y] = -2;
                                break;
                            case "miss":
                                this.playerMap.mapTokens[data.lastAction.x][data.lastAction.y] = -1;
                                break;
                            default:
                        }
                        this.playerMap.updateDOMMap();
                    }


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
    },


    updateShipsListDiv(actualShipsLeft) {
        this.shipsListDiv.innerText = "";
        actualShipsLeft.forEach((s,i) => {
            this.shipsListDiv.innerText += s + " x ";
            for (var j = 0; j < (i + 1); j++) {
                this.shipsListDiv.innerText += "■";
            }
        });
    },

    hideShipsListDiv() {
        this.shipsListDiv.style.display = "none";
    }

}