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
        public int Score { get; set; }
        public bool HasCardsLeft => PlayerHand.Count > 0;


        public Player(string name)
        {
            this.Name = name;
            this.PlayerHand = new List<Card>();
            this.Score = 0;
        }

    }
}
