using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib.UnitTest
{

    public class HandTest
    {


        /// <summary>
        /// Test to make sure that only 5 cards are allowed in hand, if to many
        /// Cards in Hand, it returns a IndexOutOfRangeException with message saying
        /// "Too many cards, 5 is max".
        /// </summary>
        [Test]
        public void CardsCanBeAddedToHand()
        {
            //Arrange 
            List<Card> cards = new List<Card>
            {
                new Card(Rank.Ace, Suite.Spades),
                new Card(Rank.Two, Suite.Diamonds),
                new Card(Rank.Three, Suite.Spades),
                new Card(Rank.Ten, Suite.Hearts),
                new Card(Rank.Nine, Suite.Clubs),
            };
            Hand hand = new Hand();


            //Assert
            CollectionAssert.IsEmpty(hand);
            for (int i = 0; i < 5; i++)
            {
                hand.AddCard(cards[i]);
                CollectionAssert.AreEqual(hand.Take(i + 1), hand);
            }
            CollectionAssert.AreEqual(cards, hand);
        }


        /// <summary>
        /// Hand can be sorted. .<!-- EJ klar-->
        /// </summary>
        [Test]
        public void HandCanBeSorted()
        {
            //Arrange 
            Player player1 = new Player("Dhan", 0);
            Hand hand = new Hand();
            List<Card> cards = new List<Card>
            {
                new Card(Rank.Ace, Suite.Spades),
                new Card(Rank.Two, Suite.Diamonds),
                new Card(Rank.Three, Suite.Spades),
                new Card(Rank.Ten, Suite.Hearts),
                new Card(Rank.Nine, Suite.Clubs),
            };
            List<Card> Ordereredcards = new List<Card>
            {
                new Card(Rank.Two, Suite.Diamonds),
                new Card(Rank.Three, Suite.Spades),
                new Card(Rank.Nine, Suite.Clubs),
                new Card(Rank.Ten, Suite.Hearts),
                new Card(Rank.Ace, Suite.Spades),
            };

            foreach (Card card in cards)
            {
                player1.GetCard(card);
            }

            player1.Hand.SortHand();
            //Assert
            CollectionAssert.AreEquivalent(cards, player1.Hand);
        }



        /// <summary>
        /// Test to make sure that only 5 cards are allowed in hand, if to many
        /// Cards in Hand, it returns a IndexOutOfRangeException with message saying
        /// "Too many cards, 5 is max".
        /// </summary>
        [Test]
        public void MaximumFiveCards()
        {
            //Arrange 
            Hand hand = new Hand();
            hand.AddCard(new Card(Rank.Two, Suite.Clubs));
            hand.AddCard(new Card(Rank.Three, Suite.Hearts));
            hand.AddCard(new Card(Rank.King, Suite.Diamonds));
            hand.AddCard(new Card(Rank.Ace, Suite.Clubs));
            hand.AddCard(new Card(Rank.Nine, Suite.Spades));

            //Act

            //Assert
            var ex = Assert.Throws<IndexOutOfRangeException>(
                 () => hand.AddCard(new Card(Rank.Nine, Suite.Clubs))
             );

            Assert.AreEqual("Too many cards, 5 is max", ex.Message);
        }


        /// <summary>
        /// Make sure the Cards in Hand can be HighCard
        /// </summary>
        [Test]
        public void HandCanBeHighCard()
        {
            //Arrange
            var hand = new Hand();
            hand.AddCard(new Card(Rank.Two, Suite.Clubs));
            hand.AddCard(new Card(Rank.Three, Suite.Hearts));
            hand.AddCard(new Card(Rank.King, Suite.Diamonds));
            hand.AddCard(new Card(Rank.Queen, Suite.Clubs));
            hand.AddCard(new Card(Rank.Ace, Suite.Spades));
            //Act
            var expected = HandType.HighCard;
            //When
            var actual = hand.EvaluateHand();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Make sure the Cards in Hand can be Pair
        /// </summary>
        [Test]
        public void HandCanBePair()
        {
            //Arrange
            var hand = new Hand();
            hand.AddCard(new Card(Rank.Two, Suite.Clubs));
            hand.AddCard(new Card(Rank.Two, Suite.Hearts));
            hand.AddCard(new Card(Rank.King, Suite.Diamonds));
            hand.AddCard(new Card(Rank.Queen, Suite.Clubs));
            hand.AddCard(new Card(Rank.Ace, Suite.Spades));
            //Act
            var expected = HandType.Pair;
            var actual = hand.EvaluateHand();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Make sure the Cards in Hand can be TwoPair
        /// </summary>
        [Test]
        public void HandCanBeTwoPair()
        {
            //Arrange
            var hand = new Hand();
            hand.AddCard(new Card(Rank.Two, Suite.Clubs));
            hand.AddCard(new Card(Rank.King, Suite.Hearts));
            hand.AddCard(new Card(Rank.King, Suite.Diamonds));
            hand.AddCard(new Card(Rank.Ace, Suite.Clubs));
            hand.AddCard(new Card(Rank.Ace, Suite.Spades));
            //Act
            var expected = HandType.TwoPairs;
            //When
            var actual = hand.EvaluateHand();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Make sure the Cards in Hand can be ThreeOfAKind
        /// </summary>
        [Test]
        public void HandCanBeThreeOfAKind()
        {
            //Arrange
            var hand = new Hand();
            hand.AddCard(new Card(Rank.Two, Suite.Clubs));
            hand.AddCard(new Card(Rank.Three, Suite.Hearts));
            hand.AddCard(new Card(Rank.Ace, Suite.Diamonds));
            hand.AddCard(new Card(Rank.Ace, Suite.Clubs));
            hand.AddCard(new Card(Rank.Ace, Suite.Spades));
            //Act
            var expected = HandType.ThreeOfAKind;
            //When
            var actual = hand.EvaluateHand();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Make sure the Cards in Hand can be FourOfAKind
        /// </summary>
        [Test]
        public void HandCanFourOfAKind()
        {
            //Arrange
            var hand = new Hand();
            hand.AddCard(new Card(Rank.Two, Suite.Clubs));
            hand.AddCard(new Card(Rank.Ace, Suite.Hearts));
            hand.AddCard(new Card(Rank.Ace, Suite.Diamonds));
            hand.AddCard(new Card(Rank.Ace, Suite.Clubs));
            hand.AddCard(new Card(Rank.Ace, Suite.Spades));
            //Act
            var expected = HandType.FourOfAKind;
            //When
            var actual = hand.EvaluateHand();
            //Assert
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        /// Make sure the Cards in Hand can be FullHouse
        /// </summary>
        [Test, Combinatorial]
        public void HandCanBeFullHouse([Values(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)] int rankOne, [Values(2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14)] int rankTwo)
        {
            //Arrange
            var player = new Player("Dhan", 0);
            //Act

            for (int i = 0; i < 5; i++)
            {
                Assume.That(rankOne != rankTwo);

                if (i < 3)
                {
                    player.GetCard(new Card((Rank)rankOne, (Suite)0));
                }
                else
                {
                    player.GetCard(new Card((Rank)rankTwo, (Suite)0));
                }
            }


            var expected = HandType.FullHouse;
            //When
            var actual = player.Hand.EvaluateHand();
            //Assert
            Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// Make sure the Cards in Hand can be Flush
        /// </summary>
        [Test, Combinatorial]
        public void HandCanBeFlush([Values(0, 1, 2, 3)] int suite)
        {
            //Arrange
            var hand = new Hand();

            hand.AddCard(new Card(Rank.Two, (Suite)suite));
            hand.AddCard(new Card(Rank.Three, (Suite)suite));
            hand.AddCard(new Card(Rank.Seven, (Suite)suite));
            hand.AddCard(new Card(Rank.King, (Suite)suite));
            hand.AddCard(new Card(Rank.Ace, (Suite)suite));
            //Act
            var expected = HandType.Flush;
            hand.EvaluateHand();
            //Assert
            Assert.IsTrue(expected == hand.HandType);
        }
        /// <summary>
        /// Make sure the Cards in Hand can be Straight
        /// </summary>
        [Test, Combinatorial]
        public void HandCanBeStraight([Values(2, 3, 4, 5, 6, 7, 8, 9)] int rank, [Values(0, 1, 2, 3, 4)] int notSameColors)
        {
            //Arrange
            Player player = new Player("Dhan", 0);
            for (int i = 0; i < 5; i++)
            {
                if (i == notSameColors)
                {
                    player.GetCard(new Card((Rank)(i + rank), (Suite)1));
                    continue;
                }
                player.GetCard(new Card((Rank)(i + rank), (Suite)0));
            }

            //Act
            var expected = HandType.Straight;
            //When
            player.Hand.EvaluateHand();
            //Assert
            Assert.IsTrue(expected == player.Hand.HandType);
        }

        /// <summary>
        /// Make sure the Cards in Hand can be RoyalStraightFlush
        /// </summary>
        [Test, Combinatorial]
        public void HandCanBeStraightFlush([Values(2, 3, 4, 5, 6, 7, 8, 9)] int rank, [Values(0, 1, 2, 3)] int suite)
        {
            //Arrange
            Player player = new Player("Dhan", 0);
            for (int i = 0; i < 5; i++)
            {
                player.GetCard(new Card((Rank)(i + rank), (Suite)suite));
            }
            //Act
            var expected = HandType.StraightFlush;
            //When
            player.Hand.EvaluateHand();
            //Assert
            Assert.IsTrue(expected == player.Hand.HandType);

        }

        /// <summary>
        /// Make sure the Cards in Hand can be RoyalStraightFlush
        /// </summary>
        [Test, Combinatorial]
        public void HandCanBeRoyalStraightFlush([Values(0, 1, 2, 3)] int suite)
        {
            //Arrange
            Player player = new Player("Dhan", 0);
            for (int i = 0; i < 5; i++)
            {
                player.GetCard(new Card((Rank)(10 + i), (Suite)suite));
            }
            //Act
            var expected = HandType.RoyalStraightFlush;
            //When
            player.Hand.EvaluateHand();
            //Assert
            Assert.IsTrue(expected == player.Hand.HandType);
        }
    }
}