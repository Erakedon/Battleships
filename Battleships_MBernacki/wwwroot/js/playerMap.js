class PlayerMap extends ShipsMap {


    constructor(containerId, _mapSize, shipsList) {
        super(containerId, _mapSize, shipsList);

    }

    allShipsPlaced() {
        this.shipsLeft.forEach(s => {
            if (s > 0) return false;
        });
        return true;
    }

    setShip(x, y) {
        console.log(x, y);
        this.mapTokens[x][y] = 1;

        this.findBlank();
        this.updateDOMMap();
    }



}