namespace Poker
{
    public interface ICard
    {
        Suite Suite { get; }

        Rank Rank { get; }
    }
}