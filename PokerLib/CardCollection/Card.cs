using System.Collections;
using System.Collections.Generic;

namespace Poker.Lib
{
    public class Card : ICard
    {
        public Suite Suite { get; set; }
        public Rank Rank { get; set; }

        public Card()
        {

        }

        public Card(Rank Rank, Suite Suite)
        {
            this.Rank = Rank;
            this.Suite = Suite;
        }
    }
}