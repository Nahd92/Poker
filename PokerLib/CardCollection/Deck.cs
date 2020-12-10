using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{
    public class Deck : CardCollection, IDeck
    {
        public IReadOnlyList<Card> Cards => cards.AsReadOnly();
        private static Random rdn = new Random();

        public Deck()
        {
            SetUpDeck();
        }

        internal void SetUpDeck()
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
            var card = cards.First();
            cards.Remove(card);
            return card;
        }

        public void ShuffleCards()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                int j = rdn.Next(cards.Count);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

    }
}