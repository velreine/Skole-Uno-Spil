using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

            string[] players = new[] { "Nicky", "Per"/*, "Mikkel", "Martin"*/};

            var myGame = new Game(players.Length, players);

            // Træk 7 kort til hver spiller:
            foreach (var player in myGame.Players)
            {

                for (int i = 0; i < 7; i++)
                {
                    player.PlayerHand.Add(myGame.DrawCardFromMainStack());
                }


            }

            int debugmm = 0;


            // Game loop.
            for (;;)
            {
                var currentPlayer = myGame.GetPlayerAtID(myGame.ActivePlayerID);



                myGame.PrintPlayerInfo(currentPlayer);

                myGame.PrintOffStack();

                if (currentPlayer.HasCardsLeft)
                {

                    bool cardHasBeenPutDown = false;

                    do
                    {
                        Console.Write("Hvis du ikke kan ligge et kort skriv: \"-1\" for at trække.");
                        Console.Write("\nVælg et kort index: ");
                        int choice = Convert.ToInt32(Console.ReadLine());

                        if (choice == -1)
                        {
                            currentPlayer.PlayerHand.Add(myGame.DrawCardFromMainStack());
                            Console.WriteLine($"Du trak et kort: {currentPlayer.PlayerHand.Last()}");
                            string input = string.Empty;
                            Console.Write($"Vil du forsøge at ligge dette kort ned (J/N): ");
                            input = Console.ReadLine();

                            if (input == "J")
                            {
                                myGame.PutCardOnMainStack(currentPlayer, currentPlayer.PlayerHand.Count - 1);
                                break;
                            }
                            else
                            {
                                // Ellers break ud af løkken og gå videre til næste spiller.
                                break;
                            }

                        }
                        

                        cardHasBeenPutDown = myGame.PutCardOnMainStack(currentPlayer, choice);


                    } while (!cardHasBeenPutDown);


                }

                //TODO:: Add logic for pass with more than 2 players.
                if (myGame.PeekOffStack() != null)
                    if (myGame.PeekOffStack().Special == Card.CardSpecial.CHANGE_DIRECTION && myGame.AmountOfPlayers == 2 || myGame.PeekOffStack().Special == Card.CardSpecial.PASS && myGame.AmountOfPlayers == 2)
                    {
                        //Then don't change player.
                    }
                    else
                    {
                        myGame.ChangeGameDirection();
                        myGame.TakeNextTurn();
                    }





                // Calculate points:::
                if (currentPlayer.HasCardsLeft == false)
                {
                    
                    myGame.CalculateScore(currentPlayer);

                    Console.WriteLine("Tryk enter for at fortsætte...");

                    // Alle spillere får deres kort fjernet, og får tildelt 7 nye::
                    myGame.ResetStacks();

                    foreach (Player player in myGame.Players)
                    {
                        player.PlayerHand.Clear();

                        for (int i = 0; i < 7; i++)
                        {
                            player.PlayerHand.Add(myGame.DrawCardFromMainStack());
                        }


                    }


                    Console.ReadLine();

                    // Nu skal spillet "resettes"
                }





                Console.Clear();
            }

            //for (;;)
            //{
            //    Card c = myGame.DrawCardFromMainStack();
            //    myGame.PutCardInOffStack(c);
            //    System.Threading.Thread.Sleep(50);
            //}



        }



    }
}
