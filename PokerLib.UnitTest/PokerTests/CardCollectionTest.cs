using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PokerLib.UnitTest.Mocks;

namespace Poker.Lib.UnitTest
{
    public class CardCollectionTest
    {
        /// <summary>
        /// Transfer Cards to deck again.
        /// </summary>
        [Test]
        public void CanTransferCardsToDeck()
        {
            //Arrange
            MockCardCollection cardCollection = new MockCardCollection();
            var player = new Player("Dhan", 0, cardCollection);
            List<Card> cards = new List<Card>
            {
            new Card(Rank.Two, Suite.Clubs),
            new Card(Rank.Three, Suite.Hearts),
            new Card(Rank.King, Suite.Diamonds),
            new Card(Rank.Ace, Suite.Clubs),
            new Card(Rank.Nine, Suite.Spades),
            };

            //Act
            foreach (Card card in cards)
            {
                player.GetCard(card);
            }

            player.Discard = new ICard[] { cards[0], cards[1] };

            foreach (Card card in player.Discard)
            {
                player.DiscardCard(card);
            }
            cardCollection.TransferToDeck(player.Hand, player);

            var expectedAmountInCardCollection = new ICard[] { cards[0], cards[1] };
            var expectedOnPlayersHand = new ICard[] { cards[2], cards[3], cards[4] };
            //Assert
            CollectionAssert.AreEqual(expectedAmountInCardCollection, cardCollection.cards);
            CollectionAssert.AreEqual(expectedOnPlayersHand, player.Hand);
        }

        /// <summary>
        /// Transfer Cards to deck again.   .<!--EJ KLAR-->
        /// </summary>
        [Test]
        public void CanTransferAllCardsToDeck()
        {
            //Arrange
            MockCardCollection cardCollection = new MockCardCollection();
            Player players = new Player("Dhan", 0, cardCollection);
            List<Card> cards = new List<Card>
            {
            new Card(Rank.Two, Suite.Clubs),
            new Card(Rank.Three, Suite.Hearts),
            new Card(Rank.King, Suite.Diamonds),
            new Card(Rank.Ace, Suite.Clubs),
            new Card(Rank.Nine, Suite.Spades),
            };
            cards = cards.OrderBy(x => x.Rank).ThenBy(s => s.Suite).ToList();

            //Act
            foreach (Card card in cards)
            {
                players.GetCard(card);
            }

            players.Discard = new ICard[] { cards[0], cards[1] };

            foreach (Card card in players.Discard)
            {
                players.DiscardCard(card);
            }

            cardCollection.TransferToDeck(players.Hand, players);

            //Assert
            Assert.AreEqual(2, players.Discard.Length);
        }
    }
}