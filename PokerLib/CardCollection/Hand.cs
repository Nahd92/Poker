using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{

    public class Hand : CardCollection, IComparable<Hand>
    {

        #region Properties
        public HandType HandType { get; set; }
        private Hand hand { get; set; }
        #endregion

        #region Constructors



        public Hand(List<Card> card)
        {
            if (cards.Count != 5)
            {
                throw new Exception("Invalid amount of cards");
            }
            cards = card;
        }
        #endregion

        public Hand()
        {

        }
        #region Methods
        public override void Add(Card card)
        {
            cards.Add(card);
        }

        public void SortHand()
        {
            cards = cards.OrderBy(x => (int)x.Rank).ToList();
        }


        public HandType EvaluateHand()
        {
            //gets number of each suit on hand
            if (RoyalStraightFlush())

                return HandType = HandType.RoyalStraightFlush;
            else if (FourOfAKind())
                return HandType = HandType.FourOfAKind;
            else if (FullHouse())
                return HandType = HandType.FullHouse;
            else if (StraightFlush())
                return HandType = HandType.StraightFlush;
            else if (Flush())
                return HandType = HandType.Flush;
            else if (Straight())
                return HandType = HandType.Straight;
            else if (ThreeOfAKind())
                return HandType = HandType.ThreeOfAKind;
            else if (TwoPairs())
                return HandType = HandType.TwoPairs;
            else if (Pair())
                return HandType = HandType.Pair;
            else
            {
                return HandType = HandType.HighCard;
            }
        }



        private bool Straight()
        {
            var aceHigh = cards.Select(c => (int)c.Rank).OrderBy(x => x).ToArray();
            var aceLow = aceHigh.Select(x => x == 14 ? 1 : x).ToArray();

            return new[] { aceHigh, aceLow }.Any(cs => cs.Skip(1).Zip(cs, (c1, c0) => c1 - c0).All(x => x == 1));
        }
        private bool RoyalStraightFlush()
        {
            var firstIsTen = cards.Select(c => (int)c.Rank).Min() == 10;
            return firstIsTen && this.StraightFlush();
        }
        private bool FourOfAKind() =>
                    cards.GroupBy(x => x.Rank)
                    .Any(x => x.Count() == 4);
        private bool FullHouse() =>
                    cards
                    .GroupBy(x => x.Rank)
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
        #endregion


        #region Interfaces

        public int CompareTo(Hand other)
        {
            return 0;
        }

        #endregion

    }
}