using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Poker.Lib;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PokerLib.UnitTest")]

namespace Poker.Lib
{
    public class PokerGame : IPokerGame
    {
        public event OnNewDeal NewDeal;
        public event OnSelectCardsToDiscard SelectCardsToDiscard;
        public event OnRecievedReplacementCards RecievedReplacementCards;
        public event OnShowAllHands ShowAllHands;
        public event OnWinner Winner;
        public event OnDraw Draw;
        private bool quit = true;
        private List<Player> players = new List<Player>();
        public IPlayer[] Players { get => players.ToArray(); }
        public PokerGame()
        {

        }
        public PokerGame(string fileName)
        {
            LoadTextFile(fileName);
        }

        public PokerGame(string[] playerNames)
        {
            CreatePlayers(playerNames);
        }


        internal void CreatePlayers(string[] playerNames)
        {
            if (playerNames.Any(string.IsNullOrEmpty) || playerNames.Length == 0)
            {
                throw new NullReferenceException("Cannot create players, no name was given, Try Again!");
            }
            foreach (string user in playerNames)
            {
                players.Add(new Player(user, 0));
            }
        }
        internal void SaveToFile(string fileName)
        {
            var stream = File.Open(fileName, FileMode.Create);
            using (var writer = new StreamWriter(stream))
                foreach (var player in players)
                {
                    writer.WriteLine(player);
                }
        }
        public void LoadTextFile(string fileName)
        {
            var stream = File.Open(fileName, FileMode.OpenOrCreate);
            string line;
            using (var reader = new StreamReader(stream))
                while ((line = reader.ReadLine()) != null)
                {
                    players.Add(PokerHelpers.Deserialize(line));
                }
        }

        public void Exit()
        {
            Environment.Exit(0);
        }


        public bool KeepPlayingOrNot()
        {
            return quit = false;
        }

        public void RunGame()
        {
            while (quit)
            {
                var dealer = new Dealer(new Deck());
                NewDeal();

                foreach (var player in players)
                {
                    dealer.Deal(player);
                    player.Hand.SortHand();
                    player.Hand.EvaluateHand();
                }
                foreach (var player in players)
                {
                    SelectCardsToDiscard?.Invoke(player);
                    foreach (Card card in player.Discard)
                    {
                        player.DiscardCard(card);
                    }
                    dealer.GiveNewCards(player);
                    player.Hand.SortHand();
                    player.Hand.EvaluateHand();
                    RecievedReplacementCards(player);
                }

                ShowAllHands();
                CheckBestHand(players);

                foreach (Player player in players)
                {
                    dealer.CollectCards(player.Hand, player);
                }
            }
        }

        public void CheckBestHand(List<Player> players)
        {
            List<Player> HighestHand = new List<Player>();
            HandType HighestHandValue = HandType.HighCard;
            foreach (var player in players)
            {
                if ((int)player.Hand.HandType > (int)HighestHandValue)
                {
                    HighestHandValue = player.Hand.HandType;
                    HighestHand.Clear();
                }
                if ((int)player.Hand.HandType >= (int)HighestHandValue)
                {
                    HighestHand.Add(player);
                }
            }
            if (HighestHand.Count == 1)
            {
                HighestHand[0].Win();
                Winner?.Invoke(HighestHand[0]);
            }
            else
            {
                if (HighestHandValue == HandType.HighCard)
                {
                    HighestHand = FindHighestCard(HighestHand);
                }
                if (HighestHandValue == HandType.Pair)
                {
                    HighestHand = FindHighestPair(HighestHand);
                }
                else if (HighestHandValue == HandType.TwoPairs)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Rank highestRank = HighestHand.Select(player => player.Hand.PairRanks[i]).Max();
                        HighestHand = HighestHand.Where(player => player.Hand.PairRanks[i] == highestRank).ToList();
                        if (HighestHand.Count == 1) break;
                    }
                    if (HighestHand.Count > 1)
                    {
                        HighestHand = FindHighestCard(HighestHand);
                    }
                }
                else if (HighestHandValue == HandType.ThreeOfAKind)
                {
                    HighestHand = FindHighestThree(HighestHand);
                }
                else if (HighestHandValue == HandType.Straight)
                {
                    HighestHand = FindHighestCard(HighestHand);
                }
                else if (HighestHandValue == HandType.Flush)
                {
                    HighestHand = FindHighestCard(HighestHand);
                }
                else if (HighestHandValue == HandType.FullHouse)
                {
                    HighestHand = FindHighestThree(HighestHand);
                    if (HighestHand.Count > 1)
                    {
                        HighestHand = FindHighestPair(HighestHand);
                    }
                }
                else if (HighestHandValue == HandType.FourOfAKind)
                {
                    Rank highestRank = HighestHand.Select(player => player.Hand.FourRank).Max();
                    HighestHand = players.Where(player => player.Hand.FourRank == highestRank).ToList();
                    if (HighestHand.Count > 1)
                    {
                        HighestHand = FindHighestCard(HighestHand);
                    }
                }
                else if (HighestHandValue == HandType.StraightFlush)
                {
                    HighestHand = FindHighestCard(HighestHand);
                }
                else if (HighestHandValue == HandType.RoyalStraightFlush)
                {
                    // Will be a draw
                }
                foreach (var player in players)
                {
                    if (HighestHand.Count == 1)
                    {
                        HighestHand[0].Win();
                        Winner?.Invoke(HighestHand[0]);
                    }
                    else
                    {
                        Draw?.Invoke(HighestHand.ToArray());
                    }
                }

            }
        }

        public List<Player> FindHighestCard(List<Player> players)
        {
            for (int i = 0; i < 5; i++)
            {
                Rank highest = players.Select(player => player.Hand.Ranks[i]).Max();
                players = players.Where(player => player.Hand.Ranks[i] == highest).ToList();
                if (players.Count == 1) break;
            }
            return players;
        }

        public List<Player> FindHighestPair(List<Player> players)
        {
            Rank highestRank = players.Select(player => player.Hand.PairRanks.First()).Max();
            players = players.Where(player => player.Hand.PairRanks.First() == highestRank).ToList();
            if (players.Count > 1)
            {
                players = FindHighestCard(players);
            }
            return players;
        }

        public List<Player> FindHighestThree(List<Player> players)
        {
            Rank highestRank = players.Select(player => player.Hand.ThreeRank).Max();
            players = players.Where(player => player.Hand.ThreeRank == highestRank).ToList();
            if (players.Count > 1)
            {
                players = FindHighestCard(players);
            }
            return players;
        }

        public void SaveGameAndExit(string fileName)
        {
            //Saves to the specifik file from parameters
            SaveToFile(fileName);
            //Exits the game
            Exit();
        }
    }
}