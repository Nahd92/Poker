namespace Poker.Lib
{
    public interface IDealer
    {
        Deck Deck { get; set; }
        void Deal(Player player);
        void GiveNewCards(Player player);
        void CollectCards(CardCollection cards, Player players);

    }
}