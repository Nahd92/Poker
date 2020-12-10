namespace Poker.Lib
{
    public interface IDeck
    {
        Card DrawCard();
        void ShuffleCards();
    }
}