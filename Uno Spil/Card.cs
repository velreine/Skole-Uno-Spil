using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno_Spil
{
	public class Card
	{

		public int Value { get; set; }
		public CardColor Color { get; set; }
		public CardSpecial Special { get; set; }

	   

		public Card(int value, CardColor color) : this(value, color, CardSpecial.NORMAL)
		{

		}

		public Card(int value, CardColor color, CardSpecial special)
		{
			this.Value = value;
			this.Color = color;
			this.Special = special;


		}

		public enum CardColor
		{
			NONE,
			RED,
			BLUE,
			GREEN,
			YELLOW
		}

		public enum CardSpecial
		{
			NORMAL,
			TAKE_TWO,
			CHANGE_DIRECTION,
			PASS,
			JOKER_CHOOSE_COLOR,
			JOKER_CHOOSE_COLOR_AND_DRAW_4_CARDS,
		}

	    public override string ToString()
	    {
	        return $"{this.Color} card with Value: {this.Value} Special Effect:{this.Special}";
	    }
	}


}
