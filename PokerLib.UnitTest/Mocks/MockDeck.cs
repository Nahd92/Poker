using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib.UnitTest
{
    public class MockDeck : IDeck
    {
        private List<Card> cards = new List<Card>();
        public bool DealerShuffledTHeDeckBeforeDrawingCards { get; private set; }
        private bool DealerHasDrawnCardsFromDeck = false;

        public MockDeck(int numberOfPlayers, List<Card> hands)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < numberOfPlayers; j++)
                {
                    cards.Add(hands[i]);
                }
            }
        }


        public Card DrawCard()
        {
            var card = cards.First();
            cards.Remove(card);
            return card;
        }

        public void ShuffleCards()
        {
            if (!DealerHasDrawnCardsFromDeck)
            {
                DealerShuffledTHeDeckBeforeDrawingCards = true;
            }
        }
    }
}