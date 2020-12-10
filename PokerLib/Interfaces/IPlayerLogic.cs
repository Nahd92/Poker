using Poker.Lib;

namespace Poker
{
    public interface IPlayerLogic
    {
        Hand Hand { get; set; }
        int Wins { get; set; }
        string name { get; set; }
        void GetCard(Card card);
        void DiscardCard(Card discardCard);
        void Win();
    }
}