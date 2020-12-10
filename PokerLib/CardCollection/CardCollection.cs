using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{
    public class CardCollection : ICardCollection, IEnumerable<Card>
    {
        protected List<Card> cards = new List<Card>();

        public virtual void AddCard(Card card)
        {
            this.cards.Add(card);
        }

        protected virtual void RemoveCard(Card card)
        {
            this.cards.Remove(card);
        }

        public void TransferToDeck(CardCollection cards, Player players)
        {
            foreach (Card iCard in players.Discard)
            {
                cards.AddCard(iCard);
            }
            foreach (Card card in this.cards.ToList())
            {
                cards.AddCard(card);
                RemoveCard(card);
            }
        }


        public IEnumerator GetEnumerator() => cards.GetEnumerator();

        IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
        {
            return cards.GetEnumerator();
        }
    }
}


