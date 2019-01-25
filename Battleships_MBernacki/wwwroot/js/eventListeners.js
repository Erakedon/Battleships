

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


function onLobbyLoad() {
    console.log('Lobby loaded');

}