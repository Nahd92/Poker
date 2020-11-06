using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Lib
{
    public class Player : IPlayer, IEnumerable
    {
        #region Properties
        public string Name { get; set; }
        string IPlayer.Name => Name;
        public Hand Hand { get; set; }
        ICard[] IPlayer.Hand => Hand.ToArray();
        HandType IPlayer.HandType => Hand.HandType;
        public int Wins { get; set; }
        int IPlayer.Wins => Wins;
        public ICard[] Discard { get; set; }
        ICard[] IPlayer.Discard { set { Discard = value; } }

        #endregion

        #region Constructors
        public Player()
        {

        }

        public Player(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Methods
        //Takes a card to hand.
        public void GetCard(Card card)
        {
            if (Hand == null)
            {
                Hand = new Hand();
            }
            Hand.Add(card);
        }

        public void DiscardCard(Card discardCard)
        {
            Hand.TransferTo(new CardCollection(new[] { discardCard }));
        }

        public override string ToString()
        {
            return $"Name: {Name} Total wins: {Wins}";
        }

        #endregion

        #region Interfaces
        public IEnumerator GetEnumerator() => Hand.GetEnumerator();





        #endregion


    }
}

