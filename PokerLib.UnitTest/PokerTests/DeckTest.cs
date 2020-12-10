using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Poker.Lib.UnitTest
{
    public class DeckTest
    {

        private Deck Deck = new Deck();

        /// <summary>
        /// Test if Deck have the right amount of cards in deck at
        /// Start.
        /// </summary>
        [Test]
        public void DeckHaveRightAmountOfCardsAtStart()
        {
            //Arrange
            Deck deck = new Deck();
            //Act 
            var expectedAmount = 52;
            //Assert
            Assert.AreEqual(expectedAmount, deck.Cards.Count);
        }
        [Test]
        public void CardsIsNotTheSameAfterShuffledTwoTimes()
        {
            //Arrange
            var temporaryDeck = new Deck();

            //Act
            temporaryDeck.ShuffleCards();
            temporaryDeck.ShuffleCards();
            //Assert

            Assert.That(temporaryDeck, Is.Not.EqualTo(Deck));
        }
        /// <summary>
        /// Test if Deck have the right amount of cards in deck at
        /// Start.
        /// </summary>
        [Test]
        public void DeckHaveRightAmountOfCardsAfterShuffled()
        {
            //Arrange
            Deck deck = new Deck();
            deck.ShuffleCards();
            //Act 
            var expectedAmount = 52;
            //Assert
            Assert.AreEqual(expectedAmount, deck.Cards.Count);
        }

        /// <summary>
        /// Check if Deck draws the topCard in Game
        /// </summary>
        [Test]
        public void DeckDrawsTopCardInDeck()
        {
            //Arrange
            Deck deck = new Deck();
            //Act
            Card cardTake = deck.DrawCard();
            //Assert
            Assert.IsFalse(deck.Cards.ToList().Exists(c => c == cardTake));
        }

        /// <summary>
        /// Test to check if there's one less card in deck 
        /// when card have been drawn.
        /// </summary>
        [Test]
        public void DeckAmountDecreaseByOneWhenDrawCard()
        {
            //Arrange
            Deck deck = new Deck();
            //Act
            deck.DrawCard();
            //Assert
            Assert.AreEqual(51, deck.Cards.Count);
        }
    }
}