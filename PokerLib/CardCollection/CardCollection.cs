using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{
    public class CardCollection : IEnumerable<Card>, IComparable<CardCollection>
    {
        #region Properties
        protected List<Card> cards = new List<Card>();

        #endregion

        #region Constructors


        public CardCollection()
        {

        }
        public CardCollection(IEnumerable<Card> card)
        {
            this.cards = card.ToList();
        }
        #endregion


        #region Virtual Methods
        //Add a Card to Card
        public virtual void Add(Card card)
        {
            this.cards.Add(card);
        }


        //Removes a Card at Index 0 
        protected virtual void RemoveCard(Card card)
        {
            this.cards.Remove(card);
        }
        #endregion

        #region Methods

        //Cards that have been thrown and Can go back to deck.
        public void TransferTo(CardCollection cardsss)
        {
            foreach (Card card in cardsss.ToList())
            {
                RemoveCard(card);
            }
        }

        #endregion

        #region Interfaces
        public IEnumerator GetEnumerator() => cards.GetEnumerator();

        IEnumerator<Card> IEnumerable<Card>.GetEnumerator()
        {
            return cards.GetEnumerator();
        }
        public int CompareTo(CardCollection other)
        {
            return 0;
        }
        #endregion
    }
}


