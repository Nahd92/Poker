namespace Poker
{
    public interface IPlayer
    {
        string Name { get; }

        ICard[] Hand { get; }

        HandType HandType { get; }

        int Wins { get; }

        ICard[] Discard { set; }
    }
}