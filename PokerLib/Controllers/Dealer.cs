using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{
    public class Dealer : IDealer
    {
        private const int NumberOfCardsPerPlayer = 5;
        public Deck Deck { get; set; }


        public Dealer(Deck deck)
        {
            this.Deck = deck;
        }

        public void Deal(Player player)
        {
            for (var i = 1; i <= NumberOfCardsPerPlayer; i++)
            {
                player.GetCard(Deck.DrawCard());
            }
        }

        public void GiveNewCards(Player player)
        {
            while (player.Hand.Count() < NumberOfCardsPerPlayer)
            {
                player.GetCard(Deck.DrawCard());
            }
        }

        public void CollectCards(CardCollection card, Player players)
        {
            card.TransferToDeck(Deck, players);
        }
    }
}