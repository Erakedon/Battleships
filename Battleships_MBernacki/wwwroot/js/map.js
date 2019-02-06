class ShipsMap {

    constructor(containerId, _mapSize, shipsList) {
        this.mapDOMRef = new Array(_mapSize);
        this.containerId = containerId;

        this.generateMap(this.containerId);

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
        console.log(this.containerId);

        for (let i = 0; i < this.mapSize; i++) {

            let row = document.createElement("div");
            row.classList.add('mapRow');

            this.mapDOMRef[i] = new Array(this.mapSize);

            for (let j = 0; j < this.mapSize; j++) {
                let cell = document.createElement("div");
                cell.classList.add('cell');

                this.mapDOMRef[i][j] = cell;

                row.appendChild(cell);
            }
            document.querySelector(this.containerId).appendChild(row)
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

                    if (sxNeg + sxPos + syNeg + syPos > 0) {
                        console.log(sxNeg, sxPos, syNeg,syPos)
                    }

                    if (sxNeg + sxPos == biggestShip - 1) {
                        this.shipsLeft[biggestShip - 1] -= 1;
                        let x = i + sxPos;
                        if (x + 1 < this.mapSize) this.mapTokens[x + 1][j] = -1;
                        while (x >= i - sxNeg) {
                            this.mapTokens[x][j] = -2;
                            x--;
                        }
                        if (x + 1 > 0) this.mapTokens[x][j] = -1;
                        startOver = true;
                    }
                    if (syNeg + syPos == biggestShip - 1) {
                        if (!startOver) this.shipsLeft[biggestShip - 1] -= 1;
                        let y = j + syPos;
                        if (y + 1 < this.mapSize) this.mapTokens[i][y + 1] = -1;
                        while (y >= j - syNeg) {
                            this.mapTokens[i][y] = -2;
                            y--;
                        }
                        if (y + 1 > 0) this.mapTokens[i][y] = -1;
                        startOver = true;
                    }
                    if (startOver) break;
                }
            }
            if (startOver) break;
        }
        if (startOver) this.findBlank();
    }

    markSlants(x, y) {
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

    if (this.mapTokens[x][y] == 1) {
        this.searchedCells[x][y] = 1;
        this.markSlants(x, y);


        return 1 + this.findInDirection(x + dirX, y + dirY, dirX, dirY);
    }
        this.searchedCells[x][y] = 1;
        return 0;
}


    updateDOMMap() {

        for (var i = 0; i < this.mapSize; i++) {
            for (var j = 0; j < this.mapSize; j++) {
                this.mapDOMRef[i][j].classList.remove("ship", "miss", "complete");
                switch (this.mapTokens[i][j]) {
                    case -2:
                        this.mapDOMRef[i][j].classList.add("complete");
                        break;
                    case -1:
                        this.mapDOMRef[i][j].onclick = () => { return false };
                        this.mapDOMRef[i][j].classList.add("miss");
                        break;
                    case 1:
                        this.mapDOMRef[i][j].classList.add("ship");
                        break;
                }
            }
        }
    }

}
