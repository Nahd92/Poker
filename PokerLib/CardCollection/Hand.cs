using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{
    public class Hand : CardCollection
    {
        public HandType HandType { get; set; }
        public List<Rank> Ranks { get; private set; }
        public List<Rank> PairRanks { get; private set; }
        public Rank ThreeRank { get; private set; }
        public Rank FourRank { get; private set; }

        public Hand()
        {

        }

        public override void AddCard(Card card)
        {
            if (cards.Count >= 5)
            {
                throw new IndexOutOfRangeException("Too many cards, 5 is max");
            }
            cards.Add(card);
        }

        new public void RemoveCard(Card card)
        {
            base.RemoveCard(card);
        }

        public void SortHand()
        {
            cards = cards.OrderBy(x => (int)x.Rank).ToList();
        }

        public HandType EvaluateHand()
        {
            HandType handType;
            if (RoyalStraightFlush())
                handType = HandType.RoyalStraightFlush;
            else if (FourOfAKind())
                handType = HandType.FourOfAKind;
            else if (FullHouse())
                handType = HandType.FullHouse;
            else if (StraightFlush())
                handType = HandType.StraightFlush;
            else if (Flush())
                handType = HandType.Flush;
            else if (Straight())
                handType = HandType.Straight;
            else if (ThreeOfAKind())
                handType = HandType.ThreeOfAKind;
            else if (TwoPairs())
                handType = HandType.TwoPairs;
            else if (Pair())
                handType = HandType.Pair;
            else
            {
                handType = HandType.HighCard;
            }

            Ranks = cards.Select(card => card.Rank)
                    .OrderBy(r => r).ToList();

            if (handType == HandType.Pair || handType == HandType.TwoPairs || handType == HandType.FullHouse)
            {
                PairRanks = cards.GroupBy(card => card.Rank)
                            .Where(group => group.Count() == 2)
                            .Select(group => group.Key)
                            .OrderByDescending(x => x).ToList();
            }
            if (handType == HandType.ThreeOfAKind || handType == HandType.FullHouse)
            {
                ThreeRank = cards.GroupBy(card => card.Rank)
                            .Where(group => group.Count() == 3)
                            .Select(group => group.Key).First();
            }
            if (handType == HandType.FourOfAKind)
            {
                FourRank = cards.GroupBy(card => card.Rank)
                           .Where(group => group.Count() == 4)
                           .Select(group => group.Key).First();
            }

            HandType = handType;
            return handType;
        }

        private bool Straight()
        {
            var aceHigh = cards.Select(c => (int)c.Rank).OrderBy(x => x).ToArray();
            var aceLow = aceHigh.Select(x => x == 14 ? 1 : x).ToArray();

            return new[] { aceHigh, aceLow }.
                    Any(cs => cs
                    .Skip(1)
                    .Zip(cs, (c1, c0) => c1 - c0)
                    .All(x => x == 1));
        }
        private bool RoyalStraightFlush()
        {
            var firstCardIsTen =
                    cards.Select(c => (int)c.Rank)
                    .Min() == 10;
            return firstCardIsTen && this.StraightFlush();
        }
        private bool FourOfAKind() =>
                    cards.GroupBy(x => x.Rank)
                    .Any(x => x.Count() == 4);

        private bool FullHouse() =>
                    cards.GroupBy(x => x.Rank)
                    .Select(x => x.Count())
                    .OrderBy(x => x)
                    .SequenceEqual(new[] { 2, 3 });
        private bool StraightFlush() =>
                    this.Straight() &
                    this.Flush();
        private bool Flush() =>
                     cards.Select(x => x.Suite)
                     .Distinct().Count() == 1;
        private bool ThreeOfAKind() =>
                    cards.GroupBy(x => x.Rank)
                    .Any(x => x.Count() == 3);

        private bool TwoPairs() =>
                    cards.GroupBy(x => x.Rank)
                    .Select(x => x.Count())
                    .OrderBy(x => x)
                    .SequenceEqual(new[] { 1, 2, 2 });
        private bool Pair() =>
                    cards.GroupBy(x => x.Rank)
                    .Select(x => x.Count())
                    .OrderBy(x => x)
                    .SequenceEqual(new[] { 1, 1, 1, 2 });


    }
}