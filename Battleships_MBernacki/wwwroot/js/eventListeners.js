

function submitNickname() {

    let input = document.querySelector("#nicknameInput");
    let errLabel = document.querySelector("#nicknameErrorLabel");

    errLabel.textContent = "";

    if (input.value.length < 3) {
        errLabel.textContent += "Nickname mus have at least 3 haracters";
        return;
    }

    let userNickname = input.value;

    localStorage.setItem('userNickname', JSON.stringify(userNickname));

    console.log("Nickname submitted: " + userNickname);

    //window.location.href = '@Url.Action("Lobby", "Home")' + '/';
    window.location.href = ('/home/lobby/');
}

function submitCreateRoom() {
    let nameInput = document.querySelector("#roomNameInput"); 
    let passwordInput = document.querySelector("#roomPasswordInput"); 
    let errLabel = document.querySelector("#roomNameErrorLabel");

    errLabel.textContent = "";

    if (nameInput.value.length < 1) {
        errLabel.textContent += "Room name cannot be blank";
    }
    else {
        let password = passwordInput.value || "";
        createNewRoom(nameInput.value, password);
        //window.location.href = ('/home/gameroom/' + data.roomID);
    }
    
}

function togglePopUp(popUpId) {
    let popUpEl = document.querySelector(popUpId);

    if (popUpEl.style.display == "none") popUpEl.style.display = "flex";
    else popUpEl.style.display = "none"
}

function checkIfNicknameSet() {
    let userNickname = JSON.parse(localStorage.getItem('userNickname'));


    //if (!userNickname) window.location.href = ('/home/index/');
}

function createNewRoom(roomname, roomPassword) {
    let userNickname = JSON.parse(localStorage.getItem('userNickname'));
    if (!userNickname) window.location.href = ('/home/index/');

    let roomCreationData =
    {
        roomName: roomname,
        playername: userNickname,
        password: roomPassword
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

function joinRoom(roomId, roomPassword) {
    console.log("joinRoom");
    let userNickname = JSON.parse(localStorage.getItem('userNickname'));
    if (!userNickname) window.location.href = ('/home/index/');

    let roomJoinData =
    {
        roomId: roomId,
        playername: userNickname,
        password: roomPassword
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




                //let roomDiv = "<div class='roomInfo'><div class='roomName'>"
                //    + element.roomName
                //    + "</div><div class='playerName'>"
                //    + element.ownerName + "</div>";
                ////roomDiv += element.requirePassword ? "<i class='fas fa-lock'></i></div>" : "<div></div></div>";
                //if (element.requirePassword) {
                //    roomDiv += "<i class='fas fa-lock'></i>";
                //    roomDiv.onclick = () => { togglePopUp(RoomPasswordPopUp) }
                //}
                //else {
                //    roomDiv.onclick = () => { joinRoom(element.roomID, ""); };
                //}
                //roomDiv += "</div>";




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
                    roomDiv.onclick = () => { joinRoom(element.roomID, ""); };
                }

                document.querySelector(divToPassList).appendChild(roomDiv);
            });
        };
        options.error = function () {
            $("#msg").html("Error while calling the Web API!");
        };
        $.ajax(options);
}