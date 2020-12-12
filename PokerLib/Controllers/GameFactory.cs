
namespace Poker.Lib
{
    public static class GameFactory
    {

        public static IPokerGame NewGame(string[] playerNames)
        {
            var Game = new PokerGame(playerNames);
            return Game;
        }
        //Game Contains a list of players
        public static IPokerGame LoadGame(string fileName)
        {
            var LoadTheGame = new PokerGame(fileName);
            return LoadTheGame;
        }
    }
}