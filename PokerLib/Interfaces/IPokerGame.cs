using System.Collections.Generic;

namespace Poker.Lib
{
    public delegate void OnNewDeal();
    public delegate void OnSelectCardsToDiscard(IPlayer player);
    public delegate void OnRecievedReplacementCards(IPlayer player);

    public delegate void OnShowAllHands();

    public delegate void OnWinner(IPlayer winner);

    public delegate void OnDraw(IPlayer[] tiedPlayers);


    public interface IPokerGame
    {
        IPlayer[] Players { get; }

        void RunGame();

        void Exit();

        void SaveGameAndExit(string fileName);

        void CheckBestHand(List<Player> players);
        List<Player> FindHighestCard(List<Player> players);
        List<Player> FindHighestPair(List<Player> players);
        List<Player> FindHighestThree(List<Player> players);

        event OnNewDeal NewDeal;

        event OnSelectCardsToDiscard SelectCardsToDiscard;

        event OnRecievedReplacementCards RecievedReplacementCards;

        event OnShowAllHands ShowAllHands;

        event OnWinner Winner;

        event OnDraw Draw;
    }
}