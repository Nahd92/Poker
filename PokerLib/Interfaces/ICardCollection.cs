
namespace Poker.Lib
{
    public interface ICardCollection
    {
        void TransferToDeck(CardCollection cards, Player player);
        void AddCard(Card card);
    }
}
