using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{
    public class Player : IPlayer, IEnumerable<Hand>
    {
        public string Name { get; set; }
        string IPlayer.Name => Name;
        public Hand Hand { get; set; }
        ICard[] IPlayer.Hand => Hand.ToArray();
        HandType IPlayer.HandType => Hand.HandType;
        public int Wins { get; set; }
        int IPlayer.Wins => Wins;
        public ICard[] Discard { get; set; }
        ICard[] IPlayer.Discard { set { Discard = value; } }



        public void GetCard(Card card)
        {
            if (Hand == null || Hand.Count() == 0)
            {
                Hand = new Hand();
                Discard = new ICard[0];
            }
            Hand.AddCard(card);
        }

        public void DiscardCard(Card discardCard)
        {
            Hand.RemoveCard(discardCard);
        }

        public void Win()
        {
            Wins++;
        }

        public override string ToString()
        {
            return $"{Name} {Wins}";
        }

        public IEnumerator GetEnumerator() => Hand.GetEnumerator();

        IEnumerator<Hand> IEnumerable<Hand>.GetEnumerator()
        {
            return (IEnumerator<Hand>)Hand.GetEnumerator();
        }
    }
}

