let mapSize = 6;
let shipsList = [0, 3, 2, 1];//Index + 1 is the indicator of ship size

let oponentCells = new Array(mapSize);
let playerCells = new Array(mapSize);

let playerMap;

function initializeGameplay() {
    playerMap = new ShipsMap("#playerMap", mapSize, shipsList);

}


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

function positionShip(x, y) {

}



