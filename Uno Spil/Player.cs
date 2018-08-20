using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno_Spil
{
    public class Player
    {
        public string Name { get; set; }

        public List<Card> PlayerHand { get; }

        // TODO: ADD list of cards.
        // TODO: ADD score system.

        public Player(string name)
        {
            this.Name = name;
            this.PlayerHand = new List<Card>();
        }

    }
}
