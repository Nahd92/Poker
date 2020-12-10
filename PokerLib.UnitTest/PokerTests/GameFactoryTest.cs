using System.IO;
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
            string fileName = "savedGame.txt";
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine("Dhan 0");
                writer.WriteLine("Erik 0");
            }

            var LoadGame = GameFactory.LoadGame(fileName);

            Assert.IsNotEmpty(LoadGame.Players);
        }
    }
}