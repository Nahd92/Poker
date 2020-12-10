using System.Collections.Generic;
using System.Linq;
using Poker.Lib;

namespace PokerLib.UnitTest.Mocks
{
    public class MockCardCollection : ICardCollection
    {

        internal List<Card> cards = new List<Card>();

        public MockCardCollection()
        {

        }

        public void AddCard(Card card)
        {
            this.cards.Add(card);
        }

        public void RemoveCard(Card card)
        {
            cards.Remove(card);
        }

        public void TransferToDeck(CardCollection cards, Player players)
        {
            foreach (Card card in players.Discard)
            {
                this.AddCard(card);
            }
            foreach (Card card in cards.ToList())
            {
                this.AddCard(card);
                RemoveCard(card);
            }
        }

    }

}