using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib.UnitTest
{
    public class PlayerTest
    {

        /// <summary>
        /// Make sure player can discard Cards and get new Cards  .<!--Ej Klar-->
        /// </summary>
        [Test]
        public void PlayerCanDiscardCardsAndGetNewCards()
        {
            //Arrange
            Player player = new Player("Dhan", 0);
            List<Card> initialCards = new List<Card>
            {
                new Card(Rank.Seven, Suite.Spades),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Eight, Suite.Diamonds),
                new Card(Rank.Ten, Suite.Hearts),
                new Card(Rank.Four, Suite.Clubs),
            };
            foreach (Card cards in initialCards)
            {
                player.GetCard(cards);
            }

            List<Card> ReplacementCards = new List<Card>
            {
                new Card(Rank.Two, Suite.Hearts),
                new Card(Rank.Three, Suite.Clubs),
                new Card(Rank.King, Suite.Hearts),
            };

            foreach (Card card in initialCards)
            {
                player.DiscardCard(card);
            }

            Assert.AreEqual(0, player.Hand.Count());

            foreach (var card in ReplacementCards)
            {
                player.GetCard(card);
            }
            Assert.AreEqual(3, player.Hand.Count());
            //Assert

            var expectedOnHand = new ICard[] { ReplacementCards[0],
            ReplacementCards[1], ReplacementCards[2] };
            CollectionAssert.AreEqual(expectedOnHand, player.Hand);
        }


        /// <summary>
        /// Make sure player can discard Cards.  .<!--Ej Klar-->
        /// </summary>
        [Test]
        public void PlayerCanDiscardCards()
        {
            //Arrange
            Player player = new Player("Dhan", 0);
            List<Card> cards = new List<Card>
            {
                new Card(Rank.Seven, Suite.Spades),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Eight, Suite.Diamonds),
                new Card(Rank.Ten, Suite.Hearts),
                new Card(Rank.Four, Suite.Clubs),
            };
            foreach (Card card in cards)
            {
                player.GetCard(card);
            }


            player.Discard = new ICard[] { cards[0], cards[1] };

            foreach (Card card in player.Discard)
            {
                player.DiscardCard(card);
            }
            //Assert
            var expectedAmountInCardCollection = new ICard[] { cards[0], cards[1] };
            CollectionAssert.AreEqual(expectedAmountInCardCollection, player.Discard);
        }

        /// <summary>
        /// Make sure players hand is empty after player have discarded all
        /// cards in hand
        /// </summary>
        [Test]
        public void PlayersHandIsEmptyAfterCanBeDiscarded()
        {
            //Arrange
            Player player = new Player("Dhan", 0);
            List<Card> cards = new List<Card>
            {
            new Card(Rank.Two, Suite.Clubs),
            new Card(Rank.Three, Suite.Hearts),
            new Card(Rank.King, Suite.Diamonds),
            new Card(Rank.Ace, Suite.Clubs),
            new Card(Rank.Nine, Suite.Spades),
            };
            //Act
            for (int i = 0; i < 5; i++)
            {
                player.GetCard(cards[i]);
                CollectionAssert.AreEqual(cards.Take(i + 1), player.Hand);
            }
            Assert.AreEqual(5, player.Hand.Count());

            foreach (Card card in cards)
            {
                player.DiscardCard(card);
            }
            //Assert
            CollectionAssert.IsEmpty(player.Hand);
        }

        /// <summary>
        /// Make sure Win Method is working
        /// </summary>
        [Test]
        public void AddOneWinToPlayer()
        {
            //Arrange 
            Player dhan = new Player("Dhan", 0);
            Player erik = new Player("Erik", 0);
            PokerGame game = new PokerGame();
            //Act
            dhan.Win();
            erik.Win();
            //Assert
            Assert.AreEqual(1, dhan.Wins);
            Assert.AreEqual(1, erik.Wins);
        }

    }
}