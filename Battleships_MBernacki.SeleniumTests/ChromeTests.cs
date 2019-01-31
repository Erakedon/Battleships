using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
namespace Battleships_MBernacki.SeleniumTests
{
    [TestFixture]
    class ChromeTests
    {
        public IWebDriver _driver { set; get; }

        [SetUp]
        public void StartBrowser()
        {
            _driver = new ChromeDriver(@"C:\Users\erake\source\repos\Battleships_MBernacki\Battleships_MBernacki.SeleniumTests\bin\Debug\netcoreapp2.1");
        }

        [Test]
        [TestCase("")]
        [TestCase("a")]
        [TestCase("me")]
        public void CreateRoom_TooShortNicknamePassed_LabelShowCorrectError(string nickname)
        {
            //string nickname = "me";
            _driver.Navigate().GoToUrl("https://localhost:44318/");

            var nicknameInput = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("nicknameInput")));
            nicknameInput.SendKeys(nickname);

            _driver.FindElement(By.Id("submitNickname")).Click();

            var labelInfo = _driver.FindElement(By.Id("nicknameErrorLabel")).Text;

            Assert.AreEqual("Nickname mus have at least 3 haracters", labelInfo);
        }

        [Test]
        public void OpenLobby_CorrectNickname_ShowsLobbyHeader()
        {
            string nickname = "Nickname";

            _driver.Navigate().GoToUrl("https://localhost:44318/");

            var nicknameInput = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("nicknameInput")));
            nicknameInput.SendKeys(nickname);

            _driver.FindElement(By.Id("submitNickname")).Click();

            var lobbyHeaderText = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("lobbyheader"))).Text;
            
            Assert.AreEqual("Lobby", lobbyHeaderText);
        }

        [Test]
        public void CreateRoom_CorrectNicknameAndRoomName_RoomNameEqualsGivenName()
        {
            string nickname = "Nickname";
            string roomName = "Room Name";

            _driver.Navigate().GoToUrl("https://localhost:44318/");

            var nicknameInput = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("nicknameInput")));
            nicknameInput.SendKeys(nickname);

            _driver.FindElement(By.Id("submitNickname")).Click();

            var createRoomBtn = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("createRoomBtn")));
            createRoomBtn.Click();

            _driver.FindElement(By.Id("roomNameInput")).SendKeys(roomName);
            _driver.FindElement(By.Id("CreateRoomSubmit")).Click();

            var roomNameDiv = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("roomName")));
            var roomDisplayName = roomNameDiv.Text;

            Assert.AreEqual(roomName, roomDisplayName);
        }

        [Test]
        [TestCase(2,1)]
        [TestCase(0,0)]
        [TestCase(5,5)]
        public void SetShipOnMap_CorrectNicknameAndRoomName_CorrectClassnames(int x, int y)
        {
            string nickname = "Nickname";
            string roomName = "Room Name";

            _driver.Navigate().GoToUrl("https://localhost:44318/");

            var nicknameInput = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("nicknameInput")));
            nicknameInput.SendKeys(nickname);

            _driver.FindElement(By.Id("submitNickname")).Click();

            var createRoomBtn = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("createRoomBtn")));
            createRoomBtn.Click();

            _driver.FindElement(By.Id("roomNameInput")).SendKeys(roomName);
            _driver.FindElement(By.Id("CreateRoomSubmit")).Click();

            var playerMap = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("playerMap")));

            var rows = playerMap.FindElements(By.ClassName("mapRow"));
            var cells = rows[x].FindElements(By.ClassName("cell"));
            cells[y].Click();
            var className = cells[y].GetAttribute("class");

            Assert.AreEqual("cell ship", className);
        }

        [Test]
        [TestCase(2, 1, 3, 2)]
        [TestCase(2, 1, 3, 0)]
        [TestCase(2, 1, 1, 2)]
        [TestCase(2, 1, 1, 0)]
        public void SetShipOnMap_CorrectNicknameAndRoomNamePutsTwoSecondOnSlope_CorrectClassnames(
            int x1, int y1, int x2, int y2)
        {
            string nickname = "Nickname";
            string roomName = "Room Name";

            _driver.Navigate().GoToUrl("https://localhost:44318/");

            var nicknameInput = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("nicknameInput")));
            nicknameInput.SendKeys(nickname);

            _driver.FindElement(By.Id("submitNickname")).Click();

            var createRoomBtn = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("createRoomBtn")));
            createRoomBtn.Click();

            _driver.FindElement(By.Id("roomNameInput")).SendKeys(roomName);
            _driver.FindElement(By.Id("CreateRoomSubmit")).Click();

            var playerMap = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.Id("playerMap")));

            var rows = playerMap.FindElements(By.ClassName("mapRow"));
            var cells1 = rows[x1].FindElements(By.ClassName("cell"));
            cells1[y1].Click();

            var cells2 = rows[x2].FindElements(By.ClassName("cell"));
            var className = cells2[y2].GetAttribute("class");

            Assert.AreEqual("cell miss", className);
        }


        [TearDown]
        public void CloseBrowser()
        {
            _driver.Close();
        }



    }
}
