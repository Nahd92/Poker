using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Poker.Lib.UnitTest
{
    public class DealerTest
    {

        /// <summary>
        /// Dealer can deal cards to player
        /// </summary>
        [Test]
        public void DealerCanDealRightAmountOfCards()
        {
            //Arrange 
            Dealer dealer = new Dealer(new Deck());
            Player player = new Player("Dhan", 0);

            //Act
            dealer.Deal(player);
            //Assert
            Assert.AreEqual(5, player.Hand.Count());
        }


        /// <summary>
        /// Make sure Player get new Cards when Dealer gives new cards.
        /// </summary>
        [Test]
        public void DealerCanDealNewCards()
        {
            Dealer dealer = new Dealer(new Deck());
            Player player = new Player("Dhan", 0);
            //Arrange 
            List<Card> cards = new List<Card>
            {
            new Card(Rank.Two, Suite.Clubs),
            new Card(Rank.Three, Suite.Hearts),
            new Card(Rank.King, Suite.Diamonds),
            new Card(Rank.Ace, Suite.Clubs),
            new Card(Rank.Nine, Suite.Spades),
            };

            for (int i = 0; i < 5; i++)
            {
                player.GetCard(cards[i]);
            }

            //Act
            foreach (Card card in cards)
            {
                player.DiscardCard(card);
            }
            CollectionAssert.IsEmpty(player.Hand);

            //Assert
            dealer.GiveNewCards(player);
            CollectionAssert.AreNotEqual(cards, player.Hand);
        }


        /// <summary>
        /// Make sure the Dealer can collect cards and throw the them
        /// back to the deck.
        /// </summary>
        [Test]
        public void DealerCanCollectCards()
        {
            //Arrange 
            Dealer dealer = new Dealer(new Deck());
            List<Player> players = new List<Player>
            {
                new Player("Dhan", 0),
                new Player("Erik", 0),
            };

            //Act
            foreach (Player play in players)
            {
                dealer.Deal(play);
            }
            var expectedAmount = 42;
            Assert.AreEqual(expectedAmount, dealer.Deck.Cards.Count);

            foreach (Player player in players)
            {
                dealer.CollectCards(player.Hand, player);
            }
            //Assert
            foreach (Player player in players)
            {
                CollectionAssert.IsEmpty(player.Hand);
            }
            var expected = 52;
            Assert.AreEqual(expected, dealer.Deck.Cards.Count);
        }
    }
}