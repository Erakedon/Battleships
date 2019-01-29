class OponentMap extends ShipsMap {


    constructor(containerId, _mapSize, shipsList, gameplayRef) {
        super(containerId, _mapSize, shipsList);
        this.gameplay = gameplayRef;
        this.playerTurn = false;
    }

    enableMove() {
        this.playerTurn = true;
    }

    shoot(_x,_y) {
        if (!this.playerTurn) return;

        let shootInfo = {
            roomID: this.gameplay.roomData.roomID,
            playerKey: this.gameplay.roomData.playerRoomKey,
            x: _x,
            y: _y
        }
        console.log("shootInfo", shootInfo)

        var options = {};
        options.url = "/api/room/Shoot";
        options.type = "POST";
        options.contentType = "application/json";
        options.data = JSON.stringify(shootInfo);
        options.dataType = "json";
        options.success = (data) => {
            console.log("shooting result recieved", data);

            switch (data.lastAction.result) {
                case "win":
                    break;
                case "hit":
                    this.mapTokens[data.lastAction.x][data.lastAction.y] = 1;
                    this.mapDOMRef[data.lastAction.x][data.lastAction.y].classList.add("ship");
                    break;
                case "miss":
                    this.mapTokens[data.lastAction.x][data.lastAction.y] = -1;
                    this.mapDOMRef[data.lastAction.x][data.lastAction.y].classList.add("miss");
                    break

                default:
                    break;
            }
            this.findBlank();


            this.gameplay.watchForTurn();
        };
        options.error = (err) => {
            console.log(err);
        };
        $.ajax(options);

    }


    setMapForGameplay() {
        for (let i = 0; i < this.mapSize; i++) {
            for (let j = 0; j < this.mapSize; j++) {

                this.mapDOMRef[i][j].onclick = () => { this.shoot(i, j) };
              
            }
        }

    }

}