using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{
    public class Dealer
    {
        #region Properties
        private const int NumberOfCardsPerPlayer = 5;
        private Deck deck { get; set; }
        #endregion

        #region Constructor
        public Dealer()
        {
            deck = new Deck();
        }

        #endregion

        #region Methods
        // Dealer deals A card to player
        public void GetHand(Player player)
        {
            for (var i = 1; i <= NumberOfCardsPerPlayer; i++)
            {
                //Players Get hand and Deck draws from deck
                player.GetCard(deck.DrawCard());
            }
        }

        public void GiveNewCards(Player player)
        {   // Om Players hand är mindre är 5 så tar player ett kort och deck tar bort 
            // ett kort från kortleken.
            //Loopen ska fortsätta så länge playern har mindre än NumberOfCardsPerPlayer (Alltså 5)
            while (player.Hand.Count() < NumberOfCardsPerPlayer)
            {
                player.GetCard(deck.DrawCard());
            }
        }

        public void CollectCards(CardCollection card)
        {
            card.TransferTo(deck);
        }

        #endregion

    }
}