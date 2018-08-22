using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uno_Spil
{
    public class Game
    {

        // Constants:
        private const int UnoDefaultNumberOfCards = 108;
        private const int UnoMaxPlayers = 10;
        private const int UnoMinPlayers = 2;
        private const int UnoScoreToWinGame = 500;
        public int DEBUG_SCORE_FACTOR = 30;

        // Active game variables:
        private GameDirection _direction = GameDirection.DIRECTION_FORWARDS;
        public int ActivePlayerID { get; private set; }
        public int AmountOfPlayers { get; private set; }
        public bool GameHasBeenWon { get; set; }

        private Stack<Card> _mainStack = null;
        private Stack<Card> _offStack = null;

        //private readonly List<Card> _stackPile = null;
        //private readonly List<Card> _garbagePile = null;

        // TODO: Add List of players...
        public List<Player> Players = null;

        public Game(int numPlayers, string[] playerNames)
        {
            // First we need to initialize the mainStack:
            List<Card> listOfCards = new List<Card>(UnoDefaultNumberOfCards);
            CreateListOfCards(listOfCards);

            // We should also allocate space for the off stack:
            _offStack = new Stack<Card>(UnoDefaultNumberOfCards);

            // We should shuffle the list of cards:
            listOfCards = CardShuffler.Shuffle(listOfCards);

            // Now convert it to a stack.
            _mainStack = ListToStack(listOfCards);

            // Set the number of players:
            AmountOfPlayers = numPlayers;
            ActivePlayerID = 0;

            // Setup the players:
            Players = new List<Player>(numPlayers);

            foreach (string name in playerNames)
            {
                Players.Add(new Player(name));
            }

            GameHasBeenWon = false;

            // Eventuelle fejl:
            if (playerNames.Length != numPlayers)
                throw new ArgumentException("Der er ikke suppleret nok spiller-navne, kontra hvor mange" +
                                            " spillere der er blevet bedt om");
            if (numPlayers > UnoMaxPlayers || numPlayers < UnoMinPlayers)
                throw new ArgumentException("Der er ikke suppleret et korrekt antal spillere, vælg mellem 2-10 spillere.");



        }

        public void ChangeGameDirection()
        {
            this._direction = this._direction == GameDirection.DIRECTION_FORWARDS ? GameDirection.DIRECTION_BACKWARDS : GameDirection.DIRECTION_FORWARDS;


            //if (this.Direction == GameDirection.DIRECTION_FORWARDS)
            //{
            //    this.Direction = GameDirection.DIRECTION_BACKWARDS;
            //}
            //else
            //{
            //    this.Direction = GameDirection.DIRECTION_FORWARDS;
            //}

        }

        public void AdvanceToNextPlayer()
        {
            // Check for Winner before taking a turn:::

            if (CheckForWinner()) return;


            // If the game is going forwards we should increment our ActivePlayerID:
            if (this._direction == GameDirection.DIRECTION_FORWARDS)
            {
                ActivePlayerID++;

                if (ActivePlayerID > (AmountOfPlayers - 1)) ActivePlayerID = 0;
            }

            // If the game is going backwards we should decrement our ActrivePlayerID:
            if (this._direction == GameDirection.DIRECTION_BACKWARDS)
            {
                ActivePlayerID--;

                if (ActivePlayerID < 0) ActivePlayerID = (AmountOfPlayers - 1);
            }
        }

        public Player GetNextPlayer()
        {
            
            int tempActivePlayerID = ActivePlayerID;

            if (this._direction == GameDirection.DIRECTION_FORWARDS)
            {
                tempActivePlayerID++;

                if(tempActivePlayerID > (AmountOfPlayers -1)) tempActivePlayerID = 0;
            }

            if (this._direction == GameDirection.DIRECTION_BACKWARDS)
            {
                tempActivePlayerID--;

                if (tempActivePlayerID < 0) tempActivePlayerID = (AmountOfPlayers - 1);
            }


            return Players[tempActivePlayerID];
        }

        public Player GetPlayerAtID(int id)
        {
            return Players.ElementAt(id);
        }

        public Card DrawCardFromMainStack()
        {

            if (_mainStack.Count == 0)
            {
                Console.WriteLine("There's no cards left in the main stack.\nSo we'll put the cards from the off stack into the mainstack now.");

                // Shuffle the off stack:
                List<Card> temporaryList = new List<Card>(_offStack.Count);

                foreach (Card c in _offStack) temporaryList.Add(c);

                temporaryList = CardShuffler.Shuffle(temporaryList);

                foreach (Card ca in temporaryList) _mainStack.Push(ca);


            }

            return _mainStack.Pop();
        }

        public void PutCardInOffStack(Card card)
        {
            _offStack.Push(card);


        }

        public Card PeekOffStack()
        {


            if (_offStack.Count > 0)
                return _offStack.Peek();

            return null;

        }

        public void ResetStacks()
        {

            _mainStack.Clear();
            _offStack.Clear();

            List<Card> listOfCards = new List<Card>(UnoDefaultNumberOfCards);

            CreateListOfCards(listOfCards);

            listOfCards = CardShuffler.Shuffle(listOfCards);

            _mainStack = ListToStack(listOfCards);




        }


        private static void CreateListOfCards(List<Card> listOfCards)
        {

            // Normal 0-9 cards, of each color, 2 of each.
            for (int i = 0; i < 10; i++)
            {
                // 1 because 0 is NO COLOR to our jokers in ENUM.
                for (int j = 1; j < 5; j++)
                {
                    listOfCards.Add(new Card(i, (Card.CardColor)j));


                    // Only one 0 value card per color.
                    if (i != 0)
                        listOfCards.Add(new Card(i, (Card.CardColor)j));
                }
            }

            // Add special cards, 2 of each color.
            for (int j = 1; j < 5; j++)
            {
                listOfCards.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.TAKE_TWO));
                listOfCards.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.TAKE_TWO));

                listOfCards.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.CHANGE_DIRECTION));
                listOfCards.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.CHANGE_DIRECTION));

                listOfCards.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.PASS));
                listOfCards.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.PASS));

            }



            // Add joker cards.
            for (int i = 0; i < 4; i++)
            {
                listOfCards.Add(new Card(50, Card.CardColor.NONE, Card.CardSpecial.JOKER_CHOOSE_COLOR));
                listOfCards.Add(new Card(50, Card.CardColor.NONE, Card.CardSpecial.JOKER_CHOOSE_COLOR_AND_DRAW_4_CARDS));
            }
        }

        private Stack<Card> ListToStack(List<Card> cards)
        {
            Stack<Card> s = new Stack<Card>(cards.Capacity);

            foreach (Card c in cards)
            {
                s.Push(c);
            }

            return s;

        }

        public bool PutCardOnMainStack(Player player, int choice)
        {
            // Peek top card from off stack:
            Card peeked = null;
            if (_offStack.Count > 0) peeked = _offStack.Peek();
            
            // Validate correct card index was given:
            if ((player.PlayerHand.Count > 0 && (player.PlayerHand.Count - 1) >= choice) && choice >= 0)
            {

                Card.CardColor currentColorOfDeck = Card.CardColor.NONE;

                if(_offStack.Count > 0) currentColorOfDeck = _offStack.Peek().Color;



                //TODO:: Need to do the draw 4 cards parts.
                //If Joker card player can set the color...:
                if (player.PlayerHand[choice].Special == Card.CardSpecial.JOKER_CHOOSE_COLOR ||
                    player.PlayerHand[choice].Special == Card.CardSpecial.JOKER_CHOOSE_COLOR_AND_DRAW_4_CARDS)
                {
                    Console.WriteLine("JOKER SÆT FARVE");
                    Console.WriteLine("R = RØD, G = GRØN, B = BLÅ, Y = GUL: ");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "R":
                            player.PlayerHand[choice].Color = Card.CardColor.RED;
                            currentColorOfDeck = Card.CardColor.RED;
                            break;
                        case "G":
                            player.PlayerHand[choice].Color = Card.CardColor.GREEN;
                            currentColorOfDeck = Card.CardColor.GREEN;
                            break;
                        case "B":
                            player.PlayerHand[choice].Color = Card.CardColor.BLUE;
                            currentColorOfDeck = Card.CardColor.BLUE;
                            break;
                        case "Y":
                            player.PlayerHand[choice].Color = Card.CardColor.YELLOW;
                            currentColorOfDeck = Card.CardColor.YELLOW;
                            break;
                        default:
                            break;
                    }
                }
                
                // Handle special effects:: (TAKE TWO & JOKER 4 CARDS).
                if (player.PlayerHand[choice].Special == Card.CardSpecial.JOKER_CHOOSE_COLOR_AND_DRAW_4_CARDS)
                {
                    Player next = GetNextPlayer();
                    for (int i = 0; i < 4; i++)
                    {
                        next.PlayerHand.Add(DrawCardFromMainStack());
                    }
                }

                if (player.PlayerHand[choice].Special == Card.CardSpecial.TAKE_TWO)
                {
                    Player next = GetNextPlayer();
                    next.PlayerHand.Add((DrawCardFromMainStack()));
                    next.PlayerHand.Add((DrawCardFromMainStack()));
                }
                
                if (peeked != null)
                {
                    if (player.PlayerHand[choice].Value == PeekOffStack().Value || player.PlayerHand[choice].Color == currentColorOfDeck || (player.PlayerHand[choice].Special == PeekOffStack().Special && player.PlayerHand[choice].Special != Card.CardSpecial.NORMAL))
                    {
                        PutCardInOffStack(player.PlayerHand[choice]);
                        player.PlayerHand.RemoveAt(choice);

                        return true;
                    }
                }
                else // peeked will be null first round of the game.
                {
                    PutCardInOffStack(player.PlayerHand[choice]);
                    player.PlayerHand.RemoveAt(choice);

                    return true;
                }
            }
            
            // If a valid input was not chosen output error messages.
            Console.WriteLine("Du har valgt et forkert index eller et ugyldigt kort.");
            Console.WriteLine("Kortet skal have samme farve, værdi eller symbol");



            return false;



        }

        public void CalculateScore(Player currentPlayer)
        {
            Console.WriteLine($"Spilleren: {currentPlayer.Name} har ikke flere kort tilbage, så der skal tælles point.");

            int playerScore = 0;

            foreach (Player player in Players)
            {
                if (!ReferenceEquals(currentPlayer, player))
                {
                    foreach (Card c in player.PlayerHand)
                    {
                        playerScore += c.Value * DEBUG_SCORE_FACTOR;
                    }
                }
            }

            currentPlayer.Score += playerScore;
        }

        public bool CheckForWinner()
        {
            if (Players != null)
            {

                foreach (Player player in Players)
                {

                    if (player.Score >= UnoScoreToWinGame)
                    {
                        Console.WriteLine($"WINNER WINNER CHICKEN DINNER!!!\n {player.Name} has won the game!!!!");
                        GameHasBeenWon = true;
                        return true;
                    }

                }



            }

            return false;


        }

        public void PrintPlayerInfo(Player player)
        {
            Console.WriteLine($"Den aktive spiller er: {player.Name} med scoren: {player.Score}");

            Console.WriteLine($"Spillerens hånd: ");

            int cardIndex = -1;

            foreach (Card c in player.PlayerHand)
            {
                Console.Write($"Card Index: {++cardIndex} | {c} \n");
            }
        }

        public void PrintOffStack()
        {
            Card peeked = null;

            if (_offStack.Count > 0) peeked = _offStack.Peek();


            if (peeked != null)
            {
                Console.WriteLine($"\nDet sidste kort fra afkastpuljen er: {peeked}");
            }
            else
            {
                Console.WriteLine("Der er ikke nogen kort i afkastpuljen endnu.");
            }




        }

        private enum GameDirection
        {
            DIRECTION_FORWARDS,
            DIRECTION_BACKWARDS
        }


    }
}
