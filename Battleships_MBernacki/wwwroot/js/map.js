class ShipsMap {

    mapDOMRef;
    mapTokens;
    mapSize;

    shipsLeft;


    constructor(containerId, _mapSize, shipsList) {
        this.mapDOMRef = new Array(mapSize);
        this.generateMap(containerId);
        console.log(mapDOMRef);

        this.mapSize = _mapSize;
        this.shipsLeft = shipsList.slice();

        this.mapTokens = new Array(_mapSize);
        for (var i = 0; i < _mapSize; i++) {
            this.mapTokens[i] = new Array(_mapSize);
            for (var j = 0; j < _mapSize; j++) {
                this.mapTokens[i][j] = 0;
            }
        }

    }


    generateMap(container) {
        //let arrayReference;
        //if (allegiance == "oponent") arrayReference = oponentCells;
        //else arrayReference = playerCells;

        for (var i = 0; i < mapSize; i++) {

            let row = document.createElement("div");
            row.classList.add('mapRow');

            mapDOMRef[i] = new Array(mapSize);

            for (var j = 0; j < mapSize; j++) {
                let cell = document.createElement("div");
                cell.classList.add('cell');

                mapDOMRef[i][j] = cell;

                row.appendChild(cell);
            }

            document.querySelector(container).appendChild(row)
        }
    }

    findBlank() {

        let searchedCells = new Array(this.mapSize)
        for (var i = 0; i < this.mapSize; i++) {
            searchedCells[i] = new Array(this.mapSize);
            for (var j = 0; j < this.mapSize; j++) {
                searchedCells[i][j] = 0;
            }
        }

        let biggestShip = 0;
        this.shipsLeft.forEach((el, index) => {
            if (el != 0) biggestShip = index + 1;
        });

        function markSlants(x, y) {
            if (x > 0) {
                if (y > 0) {
                    searchedCells[x - 1][y - 1] = 1;
                    this.mapTokens[x - 1][y - 1] == -1;
                }
                if (y < this.mapSize) {
                    searchedCells[x - 1][y + 1] = 1;
                    this.mapTokens[x - 1][y + 1] == -1;
                }
            }
            if (x < this.mapSize) {
                if (y > 0) {
                    searchedCells[x - 1][y - 1] = 1;
                    this.mapTokens[x - 1][y - 1] == -1;
                }
                if (y < this.mapSize) {
                    searchedCells[x - 1][y + 1] = 1;
                    this.mapTokens[x - 1][y + 1] == -1;
                }
            }
        }
        
        function findInDirection(x, y, dirX, dirY) {
            if (x < 0 || x > this.mapSize || y < 0 || y > this.mapSize) return 0;

            if (this.mapTokens[x][y] < 0 || searchedCells[i][j] == 1) {
                searchedCells[i][j] = 1;
                return 0;
            }
            if (this.mapTokens[x][y] == 1) {
                searchedCells[i][j] = 1;
                markSlants(x, y);


                return 1 + findInDirection(x + dirX, y + dirY, dirX, dirY);
            }
        }

        let startOver = false;
        for (var i = 0; i < this.mapSize; i++) {
            for (var j = 0; j < this.mapSize; j++) {
                if (this.mapTokens[i][j] < 0 || searchedCells[i][j] == 1) continue;
                if (this.mapTokens[i][j] == 1) {

                    markSlants(i, j);

                    let sxNeg = findInDirection(i - 1, j, -1, 0);
                    let sxPos = findInDirection(i + 1, j, 1, 0);

                    let syNeg = findInDirection(i, j - 1, 0, -1);
                    let syPos = findInDirection(i, j + 1, 0, 1);

                    if (sxNeg + sxPos == biggestShip) {
                        let x = i + sxPos;
                        while (x >= i - sxNeg) {
                            this.mapTokens[x][j] = -2;
                            x--;
                        }
                        startOver = true;
                        break;
                    }
                    else if (syNeg + syPos == biggestShip) {
                        let y = i + syPos;
                        while (y >= i - syNeg) {
                            this.mapTokens[i][y] = -2;
                            y--;
                        }
                        startOver = true;
                        break;
                    }

                }

            }
            if (startOver) break;
        }
        if (startOver) this.findBlank();
    }


    updateDOMMap() {
        //this.mapDOMRef.forEach((row,i) => {
        //    row.forEach((cell,j) => {

        //    });
        //})

        for (var i = 0; i < this.mapSize; i++) {
            for (var j = 0; j < this.mapSize; j++) {
                switch (this.mapTokens[i][j]) {
                    case -2:
                        this.mapDOMRef.classList.remove("ship");
                        this.mapDOMRef.classList.add("complete");
                        break;
                    case -1:
                        this.mapDOMRef.classList.add("miss");
                        break;
                    case 1:
                        this.mapDOMRef.classList.add("ship");
                        break;
                }
            }
        }
    }

    setShip(x,y) {
        this.mapTokens[x][y] = 1;

        this.findBlank();
        this.updateDOMMap();
    }

}
