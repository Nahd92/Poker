using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{
    public class Deck : CardCollection
    {

        private int numberOfCards = 52;
        private static Random rdn = new Random();

        public Deck()
        {
            SetUpDeck();
        }

        private void SetUpDeck()
        {
            foreach (Suite s in Enum.GetValues(typeof(Suite)))
            {
                foreach (Rank r in Enum.GetValues(typeof(Rank)))
                {
                    AddCard(new Card { Suite = s, Rank = r });
                }
            }
            ShuffleCards();
        }

        public Card DrawCard()
        {
            if (!cards.Any())
            {
                throw new NullReferenceException("Deck do not contain any cards!");
            }
            var card = cards.First();
            cards.Remove(card);
            return card;
        }

        public void ShuffleCards()
        {
            for (int i = 0; i < numberOfCards; i++)
            {
                int secondCardsIndex = rdn.Next(0, 52);
                Card temp = cards[i];
                cards[i] = cards[secondCardsIndex];
                cards[secondCardsIndex] = temp;
            }
        }
    }
}