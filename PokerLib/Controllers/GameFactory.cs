using System.Collections.Generic;

namespace Poker.Lib
{
    public static class GameFactory
    {

        public static IPokerGame NewGame(string[] playerNames)
        {
            var Game = new PokerGame(playerNames);
            return Game;
        }

        public static IPokerGame LoadGame(string fileName)
        {
            var LoadTheGame = new PokerGame(fileName);
            return LoadTheGame;
        }
    }
}