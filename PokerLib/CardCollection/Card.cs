namespace Poker.Lib
{
    public class Card : ICard
    {
        public Suite Suite { get; set; }
        public Rank Rank { get; set; }

    }
}