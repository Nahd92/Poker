namespace Poker.Lib
{
    public interface IDealer
    {
        void Deal(Player player);
        void GiveNewCards(Player player);
        void CollectCards(CardCollection cards, Player players);

    }
}