class ShipsMap {

    //mapDOMRef;
    //mapTokens;
    //mapSize;

    //shipsLeft;


    constructor(containerId, _mapSize, shipsList) {
        this.mapDOMRef = new Array(mapSize);
        this.generateMap(containerId);
        console.log(this.mapDOMRef);

        this.mapSize = _mapSize;
        this.shipsLeft = shipsList.slice();
        
        this.searchedCells = new Array(this.mapSize)

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

        for (let i = 0; i < mapSize; i++) {

            let row = document.createElement("div");
            row.classList.add('mapRow');

            this.mapDOMRef[i] = new Array(mapSize);

            for (let j = 0; j < mapSize; j++) {
                let cell = document.createElement("div");
                cell.classList.add('cell');
                cell.onclick = () => { this.setShip(i,j) }

                this.mapDOMRef[i][j] = cell;

                row.appendChild(cell);
            }

            document.querySelector(container).appendChild(row)
        }
    }

    findBlank() {

        this.searchedCells = new Array(this.mapSize)
        for (var i = 0; i < this.mapSize; i++) {
            this.searchedCells[i] = new Array(this.mapSize);
            for (var j = 0; j < this.mapSize; j++) {
                this.searchedCells[i][j] = 0;
            }
        }

        let biggestShip = 0;
        this.shipsLeft.forEach((el, index) => {
            if (el > 0) biggestShip = index + 1;
        });
        console.log("biggestShip", biggestShip);
        

        let startOver = false;
        for (var i = 0; i < this.mapSize; i++) {
            for (var j = 0; j < this.mapSize; j++) {
                if (this.mapTokens[i][j] < 0 || this.searchedCells[i][j] == 1) continue;
                if (this.mapTokens[i][j] == 1) {

                    this.markSlants(i, j);

                    let sxNeg = this.findInDirection(i - 1, j, -1, 0);
                    let sxPos = this.findInDirection(i + 1, j, 1, 0);

                    let syNeg = this.findInDirection(i, j - 1, 0, -1);
                    let syPos = this.findInDirection(i, j + 1, 0, 1);

                    //if (sxNeg + sxPos + syNeg + syPos > 0) {
                    //    console.log(sxNeg, sxPos, syNeg,syPos)
                    //}

                    if (sxNeg + sxPos == biggestShip - 1) {
                        this.shipsLeft[biggestShip - 1] -= 1;
                        let x = i + sxPos;
                        if (i + 1 < this.mapSize) this.mapTokens[x + 1][j] = -1;
                        while (x >= i - sxNeg) {
                            this.mapTokens[x][j] = -2;
                            x--;
                        }
                        if (x + 1 > 0) this.mapTokens[x][j] = -1;
                        startOver = true;
                        break;
                    }
                    else if (syNeg + syPos == biggestShip - 1) {
                        this.shipsLeft[biggestShip - 1] -= 1;
                        let y = j + syPos;
                        if (y + 1 < this.mapSize) this.mapTokens[i][y + 1] = -1;
                        while (y >= j - syNeg) {
                            this.mapTokens[i][y] = -2;
                            y--;
                        }
                        if (y + 1 > 0) this.mapTokens[i][y] = -1;
                        startOver = true;
                        break;
                    }

                }

            }
            if (startOver) break;
        }
        if (startOver) this.findBlank();
    }

    markSlants(x, y) {
    //console.log(this);
    //console.log(this.this);
    if (x > 0) {
        if (y > 0) {
            this.searchedCells[x - 1][y - 1] = 1;
            this.mapTokens[x - 1][y - 1] = -1;
        }
        if (y + 1 < this.mapSize) {
            this.searchedCells[x - 1][y + 1] = 1;
            this.mapTokens[x - 1][y + 1] = -1;
        }
    }
    if (x + 1 < this.mapSize) {
        if (y > 0) {
            this.searchedCells[x + 1][y - 1] = 1;
            this.mapTokens[x + 1][y - 1] = -1;
        }
        if (y + 1 < this.mapSize) {
            this.searchedCells[x + 1][y + 1] = 1;
            this.mapTokens[x + 1][y + 1] = -1;
        }
    }
}

    findInDirection(x, y, dirX, dirY) {
    if (x < 0 || x + 1 > this.mapSize || y < 0 || y + 1 > this.mapSize) return 0;

        //if (this.mapTokens[x][y] < 0 || this.searchedCells[x][y] == 1) {
        //if (this.mapTokens[x][y] < 0) {
        //    this.searchedCells[x][y] = 1;
        //return 0;
        //}
    if (this.mapTokens[x][y] == 1) {
        this.searchedCells[x][y] = 1;
        this.markSlants(x, y);


        return 1 + this.findInDirection(x + dirX, y + dirY, dirX, dirY);
    }
        this.searchedCells[x][y] = 1;
        return 0;
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
                        this.mapDOMRef[i][j].classList.remove("ship");
                        this.mapDOMRef[i][j].classList.add("complete");
                        this.mapDOMRef[i][j].onclick = () => { return false };
                        break;
                    case -1:
                        this.mapDOMRef[i][j].onclick = () => { return false };
                        this.mapDOMRef[i][j].classList.add("miss");
                        break;
                    case 1:
                        this.mapDOMRef[i][j].onclick = () => { return false };
                        this.mapDOMRef[i][j].classList.add("ship");
                        break;
                }
            }
        }
    }

    //setShip(x, y) {
    //    console.log(x, y);
    //    this.mapTokens[x][y] = 1;

    //    this.findBlank();
    //    this.updateDOMMap();
    //}

}
