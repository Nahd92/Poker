using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Poker.Lib;

namespace Poker.Lib
{
    /// <summary>
    /// This class is responsible of the game logic
    /// </summary>
    public class PokerGame : IPokerGame
    {
        #region Events
        public event OnNewDeal NewDeal;
        public event OnSelectCardsToDiscard SelectCardsToDiscard;
        public event OnRecievedReplacementCards RecievedReplacementCards;
        public event OnShowAllHands ShowAllHands;
        public event OnWinner Winner;
        public event OnDraw Draw;
        #endregion

        #region Properties
        private bool quit = false;
        private List<Player> players = new List<Player>();
        public IPlayer[] Players { get => players.ToArray(); }

        #endregion

        #region Constructors
        //Load the game from a file
        public PokerGame()
        {

        }

        public PokerGame(string fileName)
        {
            //Loads every player and score from the file given in argument
            LoadTextFile(fileName);
        }

        public PokerGame(string[] playerNames)
        {
            //This Constructor creates every player given from userinterface
            CreatePlayers(playerNames);
        }
        #endregion

        #region Methods

        //Creates every player passed in from arguments. 
        private void CreatePlayers(string[] playerNames)
        {
            // If playersNames is empty, throw a exception else create players
            if (playerNames.Length == 0)
            {
                throw new Exception("Cannot create players, no name was given, Try Again!");
            }
            foreach (var users in playerNames)
            {
                players.Add(new Player
                {
                    Name = users
                });
            }
        }
        private void SaveToFile(string fileName)
        {
            var stream = File.Open(fileName, FileMode.Create);
            TextWriter writer = new StreamWriter(stream);
            foreach (var player in players)
            {
                writer.WriteLine(PokerHelpers.Serialize(player));
            }
            writer.Close();
        }
        private void LoadTextFile(string fileName)
        {
            var stream = File.Open(fileName, FileMode.OpenOrCreate);
            TextReader reader = new StreamReader(stream);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                players.Add(PokerHelpers.Deserialize(line));
            }
            reader.Close();
        }

        public void Exit()
        {
            //Sets quit to true (I might not need this)
            quit = true;
            // Quit the Console
            Environment.Exit(0);
        }


        public void RunGame()
        {
            //Creates the deck and shuffle the cards
            var deck = new Deck();
            //  a new dealer
            var dealer = new Dealer();

            //Userinterfaces - DealNewCard
            NewDeal();

            // All the players get 5 cards each.
            foreach (var player in players)
            {
                //Get 5 cards      
                dealer.GetHand(player);
                //Player sort hand by Rank
                player.Hand.SortHand();
                //Evaluate the present handtype
                player.Hand.EvaluateHand();

            }
            //Show all the cards for user
            ShowAllHands();
            Console.ReadKey();

            //Every player get the chance to select a card to discard
            foreach (var player in players)
            {
                // Player selects cards to be discarded. 
                SelectCardsToDiscard(player);
                //If theres any cards in player.Discard
                foreach (Card card in player.Discard)
                {
                    //Player discard each card thats in player.Discard
                    player.DiscardCard(card);
                }
                //IF playershand count is lesser than 5, give new cards.
                dealer.GiveNewCards(player);
                //Evalutate the new hand for each player.
                player.Hand.EvaluateHand();
            }

            //Show all the hands.
            ShowAllHands();




            Draw(players.ToArray());



        }


        public void SaveGameAndExit(string fileName)
        {
            //Saves to the specifik file from parameters
            SaveToFile(fileName);
            //Exits the game
            Exit();
        }

        #endregion
    }
}