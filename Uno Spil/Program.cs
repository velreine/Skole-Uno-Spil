using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno_Spil
{
	class Program
	{
		static void Main(string[] args)
		{

			//Draw a card, the highest value has to give cards, special cards are equal to 0 
			// during the draw.

            // ^ In the real world...






			// Shuffle the cards, and give each player 7 cards.



			// The rest of the cards will be laid with the backside upwards,
			// This pool of cards will be known as the stack.

			// Drawn cards are thrown in the garbage pile:





            // SCRATCHPAD:::

            //TODO: Implement that if we only have 2 players and the direction is changed, the same player should play again.

		    string[] players = new[] {"Nicky", "Per"/*, "Mikkel", "Martin"*/};

            var myGame = new Game(players.Length, players);

            // Træk 7 kort til hver spiller:
		    foreach (var player in myGame.Players)
		    {

		        for (int i = 0; i < 7; i++)
		        {
		            player.PlayerHand.Add(myGame.DrawCardFromStackPile());
		        }


		    }

		    int debugmm = 0;


            // Game loop.
		    for (;;)
		    {
		        var currentPlayer = myGame.GetPlayerAtID(myGame.ActivePlayerID);

		        Console.WriteLine($"Den aktive spiller er: {currentPlayer.Name}");

                Console.WriteLine($"Spillerens hånd: ");

		        int cardIndex = 0;

                foreach(Card c in currentPlayer.PlayerHand)
		        {
                    Console.Write($"Card Index: {cardIndex++} | {c} \n");
		        }
                                                                    /* TODO: print actual sidste kort */
                Console.Write("\nDet sidste kort fra afkastpuljen er: ");

		        Console.Write("\nVælg et kort index: ");

		        string keyboardInput = Console.ReadLine();
		        
                myGame.TakeNextTurn();
		        Console.Clear();
		    }


		}
	}
}
