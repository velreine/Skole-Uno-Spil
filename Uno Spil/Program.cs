using System;
using System.Linq;


namespace Uno_Spil
{
    class Program
    {
        static void Main(string[] args)
        {



            string[] players = { "Nicky", "Per"/*, "Mikkel", "Martin"*/};

            var myGame = new Game(players.Length, players);

            // Træk 7 kort til hver spiller:
            foreach (var player in myGame.Players)
            {
                for (int i = 0; i < 7; i++)
                {
                    player.PlayerHand.Add(myGame.DrawCardFromMainStack());
                }
            }


            // Main game loop.
            //for (;;)
            do
            {

                if (myGame.CheckForWinner()) break;


                var currentPlayer = myGame.GetPlayerAtID(myGame.ActivePlayerID);

                myGame.PrintPlayerInfo(currentPlayer);

                myGame.PrintOffStack();

                if (currentPlayer.HasCardsLeft)
                {

                    bool cardHasBeenPutDown;

                    do
                    {
                        Console.Write("Hvis du ikke kan ligge et kort skriv: \"-1\" for at trække.");
                        Console.Write("\nVælg et kort index: ");
                        int choice = Convert.ToInt32(Console.ReadLine());

                        if (choice == -1)
                        {
                            currentPlayer.PlayerHand.Add(myGame.DrawCardFromMainStack());
                            Console.WriteLine($"Du trak et kort: {currentPlayer.PlayerHand.Last()}");
                            Console.Write("Vil du forsøge at ligge dette kort ned (J/N): ");
                            var input = Console.ReadLine();

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



                // Check if player has cards left to put down,
                // If not, we should calculate his points.
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

                }

                //TODO:: Add logic for pass with more than 2 players.
                if (myGame.PeekOffStack() != null)
                    if (myGame.PeekOffStack().Special == Card.CardSpecial.CHANGE_DIRECTION &&
                        myGame.AmountOfPlayers == 2 || myGame.PeekOffStack().Special == Card.CardSpecial.PASS &&
                        myGame.AmountOfPlayers == 2)
                    {
                        //Then don't change player.
                    }
                    else
                    {
                        myGame.ChangeGameDirection();
                        myGame.AdvanceToNextPlayer();
                    }


                Console.Clear();
            } while (!myGame.GameHasBeenWon);

            

            Console.ReadLine();


        }



    }
}
