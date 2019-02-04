
function submitCreateRoom() {
    let nameInput = document.querySelector("#roomNameInput"); 
    let errLabel = document.querySelector("#roomNameErrorLabel");

    errLabel.textContent = "";

    if (nameInput.value.length < 1) {
        errLabel.textContent += "Room name cannot be blank";
    }
    else {
        createNewRoom(nameInput.value);
    }
    
}

function togglePopUp(popUpId) {
    let popUpEl = document.querySelector(popUpId);

    if (popUpEl.style.display == "none") popUpEl.style.display = "flex";
    else popUpEl.style.display = "none"
}

function createNewRoom(roomname) {

    let roomCreationData =
    {
        roomName: roomname,
        mapSize: 7,
        shipsList: [4,1,1,1]

    }
    
    var options = {};
    options.url = "/api/room/CreateRoom";
    options.type = "POST";
    options.contentType = "application/json";
    options.data = JSON.stringify(roomCreationData);
    options.dataType = "json";
    options.success = (data) => {
        console.log("Room succesfully created!", data);
        enterRoom(data);
    };
    options.error = () => {
        console.log("Server error");
    };
    $.ajax(options);    
}

function joinRoom(roomId) {

    let roomJoinData =
    {
        roomId: roomId
    }

    var options = {};
    options.url = "/api/room/JoinRoom";
    options.type = "POST";
    options.contentType = "application/json";
    options.data = JSON.stringify(roomJoinData);
    options.dataType = "json";
    options.success = (data) => {
        console.log("Room joined!", data);
        enterRoom(data);
    };
    options.error = (err) => {
        console.log(err);
        window.location.href = ('/home/lobby/' + roomData.roomID);
    };
    $.ajax(options);
}

function enterRoom(roomData) {

    localStorage.setItem(roomData.roomID, JSON.stringify(roomData));

    window.location.href = ('/home/gameroom/' + roomData.roomID);
}

function updateRoomList(divToPassList) {


    let container = document.querySelector(divToPassList);
    console.log("divToPassList: ",divToPassList);
    while (container.firstChild) {
        container.removeChild(container.firstChild);
    }

        var options = {};
        options.url = "/api/room/GetRoomList";
        options.type = "GET";
        options.dataType = "json";
        options.success = function (data) {

            data.forEach(function (element) {
                console.log(element);

                let roomDiv = document.createElement("div");
                roomDiv.classList.add("roomInfo");

                let roomName = document.createElement("div");
                roomName.classList.add("roomName");
                roomName.textContent = element.roomName;

                let playerName = document.createElement("div");
                playerName.classList.add("playerName");
                playerName.textContent = element.ownerName;


                roomDiv.appendChild(roomName);
                roomDiv.appendChild(playerName);

                if (element.requirePassword) {
                    let icon = document.createElement("i");
                    icon.classList.add("fas", "fa-lock");
                    roomDiv.appendChild(icon);
                    roomDiv.onclick = () => { togglePopUp(RoomPasswordPopUp) }
                }
                else {
                    roomDiv.onclick = () => { joinRoom(element.roomID); };
                }

                document.querySelector(divToPassList).appendChild(roomDiv);
            });
        };
        options.error = function () {
            $("#msg").html("Error while calling the Web API!");
        };
        $.ajax(options);
}