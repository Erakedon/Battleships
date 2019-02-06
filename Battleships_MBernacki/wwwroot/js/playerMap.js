class PlayerMap extends ShipsMap {

    constructor(containerId, _mapSize, shipsList, gameplayRef) {
        super(containerId, _mapSize, shipsList);
        this.shipsList = shipsList;
        this.gameplay = gameplayRef;
    }

    makeFieldsClickable() {
        console.log(this.mapDOMRef);
        for (let i = 0; i < this.mapSize; i++) {
            for (let j = 0; j < this.mapSize; j++) {
                this.mapDOMRef[i][j].onclick = () => { this.setShip(i, j) };
            }
        }
    }

    allShipsPlaced() {
        let result = true;
        this.shipsLeft.forEach(s => {
            console.log("ships left",s);
            if (s != 0) result = false;
        });
        return result;
    }

    setShip(x, y) {
        console.log(x, y);
        this.mapTokens[x][y] = 1;

        this.findBlank();
        this.updateDOMMap();
        this.gameplay.updateShipsListDiv(this.shipsLeft);
        this.mapDOMRef[x][y].onclick = () => { this.removeShip(x, y) };
        if (this.allShipsPlaced()) {
            console.log("All ships placed!");
            document.querySelector(this.containerId).classList.remove("clickable");
            document.querySelector(this.gameplay.oponentMap.containerId).classList.add("clickable");
            this.setMapForGameplay();
            this.gameplay.hideShipsListDiv();
            this.gameplay.oponentMap.showMap();
            this.sendMap();
        }
    }

    removeShip(x, y) {
        console.log("rmvShp",x, y);
        this.mapTokens[x][y] = 0;

        this.removeMiss();
        this.renewCompleted();
        this.shipsLeft = this.shipsList.slice();


        this.findBlank();
        this.updateDOMMap();
        this.gameplay.updateShipsListDiv(this.shipsLeft);
        this.mapDOMRef[x][y].onclick = () => { this.setShip(x, y) };
        this.makeBlankClickable();
    }

    removeMiss() {
        for (var i = 0; i < this.mapSize; i++) {
            for (var j = 0; j < this.mapSize; j++) {
                if (this.mapTokens[i][j] == -1) {
                    this.mapTokens[i][j] = 0;
                }
            }
        }
    }

    makeBlankClickable() {
        for (let i = 0; i < this.mapSize; i++) {
            for (let j = 0; j < this.mapSize; j++) {
                if (this.mapTokens[i][j] == 0) {
                    this.mapDOMRef[i][j].onclick = () => { this.setShip(i, j) };
                }
            }
        }
    }

    renewCompleted() {
        for (var i = 0; i < this.mapSize; i++) {
            for (var j = 0; j < this.mapSize; j++) {
                if (this.mapTokens[i][j] == -2) this.mapTokens[i][j] = 1;
            }
        }
    }

    setMapForGameplay() {
        for (let i = 0; i < this.mapSize; i++) {
            for (let j = 0; j < this.mapSize; j++) {

                this.mapDOMRef[i][j].onclick = () => { return 0 };
                this.mapDOMRef[i][j].classList.remove("ship", "miss", "complete");

                if (this.mapTokens[i][j] == -2) {
                    this.mapTokens[i][j] = 1;
                    this.mapDOMRef[i][j].classList.add("ship");
                }
                else if (this.mapTokens[i][j] == -1) {
                    this.mapTokens[i][j] = 0;
                }
            }
        }                       
    }

    sendMap() {

        let mapData = {
            map: this.mapTokens,
            roomID: this.gameplay.roomData.roomID,
            playerKey: this.gameplay.roomData.playerRoomKey
        }
        console.log("mapData", mapData)

        var options = {};
        options.url = "/api/room/PostMap";
        options.type = "POST";
        options.contentType = "application/json";
        options.data = JSON.stringify(mapData);
        options.dataType = "json";
        options.success = (data) => {
            console.log("Map Posted!", data);
            this.gameplay.watchForTurn();
            this.gameplay.oponentMap.setMapForGameplay();
        };
        options.error = (err) => {
            console.log(err);
            window.location.href = ('/home/lobby/' + roomData.roomID);
        };
        $.ajax(options);
    }
}