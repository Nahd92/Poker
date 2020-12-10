using NUnit.Framework;

namespace Poker.Lib.UnitTest
{
    public class CardTest
    {
        [SetUp]
        public void Setup()
        {

        }

        /// <summary>
        /// Controls if two different cards are equal.
        /// </summary>
        [Test]
        public void NewCardsGetsCorrectSuiteAndRank()
        {
            //Arrange 
            Rank rank = Rank.King;
            Suite suite = Suite.Hearts;

            //Act
            var newCard = new Card(rank, suite);
            //Assert
            Assert.AreEqual(newCard.Rank, rank);
            Assert.AreEqual(newCard.Suite, suite);
        }

        /// <summary>
        /// Check if Its possible to create Card With Value
        /// </summary>
        [Test]
        public void CreateCardsWithValue()
        {
            //Arrange 
            var card = new Card();
            //Assert
            Assert.NotNull(card.Rank);
            Assert.NotNull(card.Suite);
        }
    }
}