using NUnit.Framework;


namespace Poker.Lib.UnitTest
{
    public class GameFactoryTest
    {
        /// <summary>
        /// Check if GameFactory Creates a new Game with palyers
        /// </summary>
        [Test]
        public void GameFactoryReturnsPlayersFromPokerGame()
        {
            string[] players = new string[]
             {
                 new string("Dhan"),
                 new string("Erik")
            };
            var gameFactory = GameFactory.NewGame(players);

            var expectedNumberOfPlayers = 2;

            Assert.AreEqual(expectedNumberOfPlayers, gameFactory.Players.Length);
        }

        /// <summary>
        /// Check if GameFactory Can Load Players
        /// </summary>
        [Test]
        public void GameFactoryTestIfSpecificFileIsNotEmpty()
        {
            string path = @"C:\Users\Admin\Desktop\Inlämningsuppgift2\Poker\PokerConsoleApp\savedgame.txt";
            MockPokerGame pokerGame = new MockPokerGame();
            var LoadGame = GameFactory.LoadGame(path);
            var fileExist = pokerGame.FileExists(path);

            Assert.IsNotEmpty(LoadGame.Players);
            Assert.IsTrue(fileExist, path);
        }
    }
}