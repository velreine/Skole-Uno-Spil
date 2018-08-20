using System;
using System.Collections.Generic;
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

        // Active game variables:
        private GameDirection _direction = GameDirection.DIRECTION_FORWARDS;
        public int ActivePlayerID { get; private set; }
        public int AmountOfPlayers { get; private set; }


        private List<Card> _stackPile = null;
        private List<Card> _garbagePile = null;

        // TODO: Add List of players...
        public List<Player> Players = null;

        public Game(int numPlayers, string[] playerNames)
        {
            // Initialize the stackPile & the garbagePile:
            _stackPile = new List<Card>(UnoDefaultNumberOfCards);
            _garbagePile = new List<Card>(UnoDefaultNumberOfCards);

            InitializeStackPile();

            // Set the number of players:
            AmountOfPlayers = numPlayers;
            ActivePlayerID = 0;

            // Setup the players:
            Players = new List<Player>(numPlayers);

            foreach (string name in playerNames)
            {
                Players.Add(new Player(name));
            }


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

        public void TakeNextTurn()
        {



            // If the game is going forwards we should increment our ActivePlayerID:
            if (this._direction == GameDirection.DIRECTION_FORWARDS)
            {

                ActivePlayerID++;

                if (ActivePlayerID > (AmountOfPlayers-1)) ActivePlayerID = 0;



            }

            // If the game is going backwards we should decrement our ActrivePlayerID:
            if (this._direction == GameDirection.DIRECTION_BACKWARDS)
            {
                ActivePlayerID--;

                if (ActivePlayerID < 0) ActivePlayerID = (AmountOfPlayers-1);

            }




        }


        public Player GetPlayerAtID(int id)
        {
            return Players.ElementAt(id);
        }

        public Card DrawCardFromStackPile()
        {
            Card m = _stackPile.First();

            _stackPile.RemoveAt(0);

            return m;

        }


        private void InitializeStackPile()
        {

            // Normal 0-9 cards, of each color, 2 of each.
            for (int i = 0; i < 10; i++)
            {
                // 1 because 0 is NO COLOR to our jokers in ENUM.
                for (int j = 1; j < 5; j++)
                {
                    _stackPile.Add(new Card(i, (Card.CardColor)j));


                    // Only one 0 value card per color.
                    if(i != 0)
                    _stackPile.Add(new Card(i, (Card.CardColor)j));
                }
            }
            
            // Add special cards, 2 of each color.
                for (int j = 1; j < 5; j++)
                {
                    _stackPile.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.TAKE_TWO));
                    _stackPile.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.TAKE_TWO));

                    _stackPile.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.CHANGE_DIRECTION));
                    _stackPile.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.CHANGE_DIRECTION));

                    _stackPile.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.PASS));
                    _stackPile.Add(new Card(20, (Card.CardColor)j, Card.CardSpecial.PASS));

                }

            

            // Add joker cards.
            for (int i = 0; i < 4; i++)
            {
                _stackPile.Add(new Card(50, Card.CardColor.NONE, Card.CardSpecial.JOKER_CHOOSE_COLOR));
                _stackPile.Add(new Card(50, Card.CardColor.NONE, Card.CardSpecial.JOKER_CHOOSE_COLOR_AND_DRAW_4_CARDS));
            }
        }

        private enum GameDirection
        {
            DIRECTION_FORWARDS,
            DIRECTION_BACKWARDS
        }


    }
}
