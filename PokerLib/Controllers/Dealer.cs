using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{
    public class Dealer
    {
        private const int NumberOfCardsPerPlayer = 5;
        private Deck deck { get; set; }

        public Dealer()
        {
            deck = new Deck();
        }

        public void GetHand(Player player)
        {
            for (var i = 1; i <= NumberOfCardsPerPlayer; i++)
            {
                player.GetCard(deck.DrawCard());
            }
        }

        public void GiveNewCards(Player player)
        {
            while (player.Hand.Count() < NumberOfCardsPerPlayer)
            {
                player.GetCard(deck.DrawCard());
            }
        }

        public void CollectCards(CardCollection card, Player players)
        {
            card.TransferToDeck(deck, players);
        }
    }
}