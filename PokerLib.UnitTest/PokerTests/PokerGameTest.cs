using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using NUnit.Framework;

namespace Poker.Lib.UnitTest
{
    public class PokerGameTest
    {
        private PokerGame game;


        [SetUp]
        public void Setup()
        {
            game = new PokerGame();
        }

        /// <summary>
        /// Test to see of if game starts with a list of 0 players
        /// </summary>
        [Test]
        public void PokerGameInitializeithZeroPlayers()
        {
            PokerGame game = new PokerGame();
            Assert.AreEqual(0, game.Players.Length);
        }

        /// <summary>
        /// Check if Events is working and RunGame works proper
        /// </summary>
        [Test]
        public void PokerGameHaveWorkingEvents()
        {
            PokerGame game = new PokerGame(new string[2] { "PlayerOne", "PlayerTwo" });
            int count = 0;
            bool eventsIsWorking = false;

            game.NewDeal += IfNewDealEventIsWorking;
            game.SelectCardsToDiscard += IfSelectCardsToDiscardEventIsWorking;
            game.RecievedReplacementCards += IfRecievedReplacementCardsWork;
            game.ShowAllHands += IfTheShowAllHandsEventIsWorking;
            game.Winner += IfTheWinnerEventIsWorking;
            game.Draw += IfDrawEventIsWorking;

            void IfNewDealEventIsWorking()
            {
                eventsIsWorking = true;
                count++;
            }
            void IfSelectCardsToDiscardEventIsWorking(IPlayer player)
            {
                player.Discard = new ICard[] { player.Hand[0] };
                player.Discard = new ICard[] { player.Hand[0] };
                eventsIsWorking = true;
                count++;
                game.KeepPlayingOrNot();
            }

            void IfTheShowAllHandsEventIsWorking()
            {
                eventsIsWorking = true;
                count++;
                game.KeepPlayingOrNot();
            }
            void IfTheWinnerEventIsWorking(IPlayer player)
            {
                eventsIsWorking = true;
                count++;
                game.KeepPlayingOrNot();
            }
            void IfRecievedReplacementCardsWork(IPlayer player) { }
            void IfDrawEventIsWorking(IPlayer[] player) { }


            game.RunGame();
            Assert.IsTrue(eventsIsWorking);
        }

        /// <summary>
        /// Check if pokerGame can Save to File.
        /// </summary>
        [Test]
        public void PokerGameCanSaveToFile()
        {
            //Arrange
            string[] players = { "Dhan", "Felix" };
            string fileName = "savedgame.txt";
            PokerGame game = new PokerGame();
            game.CreatePlayers(players);
            game.SaveToFile(fileName);
            string[] lines;
            //Act
            lines = File.ReadAllLines(fileName);

            //Assert
            Assert.AreEqual("Dhan 0", lines[0]);
            Assert.AreEqual("Felix 0", lines[1]);
        }
        /// <summary>
        /// Check if pokerGame can Load to File.   <!--EJ klar-->
        /// </summary>
        [Test]
        public void PokerGameCanLoadAFile()
        {
            //Arrange
            string[] players = { "Dhan", "Felix" };
            string fileName = "savedgame.txt";
            PokerGame game = new PokerGame(players);
            game.SaveGameAndExit(fileName);

            //Act
            PokerGame LoadGame = new PokerGame(fileName);
            //Assert
            Assert.AreEqual(2, game.Players.Length);
            Assert.AreEqual("Dhan", game.Players[0].Name);
            Assert.AreEqual(0, game.Players[0].Wins);
            Assert.AreEqual("Felix", game.Players[1].Name);
            Assert.AreEqual(0, game.Players[1].Wins);
        }

        /// <summary>
        /// Check if pokerGame can SaveGameAndExit
        /// </summary>
        [Test]
        public void PokerGameExitGamesAsItShouldAfterGameHaveBeenSaved()
        {
            string fileName = "savedGame.txt";
            MockPokerGame game = new MockPokerGame();
            //Act
            game.SaveGameAndExit(fileName);
            //Assert
            Assert.IsTrue(game.GameExitsWhenExitMethodIsCalled);
        }


        /// <summary>
        /// Check if pokerGame creates two players
        /// </summary>
        [Test]
        public void PokerGameCanCreatePlayers()
        {
            //Arrange
            string[] players = { "Dhan", "Felix" };
            //Act
            PokerGame game = new PokerGame();
            game.CreatePlayers(players);
            //Assert
            Assert.AreEqual(2, game.Players.Length);
        }

        /// <summary>
        /// Find the HighestCard 
        /// </summary>
        [Test]
        public void FindHighestCard()
        {
            PokerGame pokerGame = new PokerGame();
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Seven, Suite.Spades),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Eight, Suite.Diamonds),
                new Card(Rank.Ten, Suite.Hearts),
                new Card(Rank.Four, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Three, Suite.Spades),
                new Card(Rank.Two, Suite.Hearts),
                new Card(Rank.Eight, Suite.Diamonds),
                new Card(Rank.Nine, Suite.Diamonds),
                new Card(Rank.Four, Suite.Hearts),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }
            pokerGame.CheckBestHand(players);
            for (int i = 0; i < 5; i++)
            {
                Assert.Greater(player.Hand.Ranks[i], playerTwo.Hand.Ranks[i]);
            }
            Assert.Greater(player.Wins, playerTwo.Wins);
        }

        /// <summary>
        /// Find the HighestPair 
        /// </summary>
        [Test]
        public void FindHighestPair()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Seven, Suite.Spades),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Ace, Suite.Diamonds),
                new Card(Rank.Ten, Suite.Hearts),
                new Card(Rank.Four, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Three, Suite.Spades),
                new Card(Rank.Three, Suite.Hearts),
                new Card(Rank.Eight, Suite.Diamonds),
                new Card(Rank.Nine, Suite.Diamonds),
                new Card(Rank.Four, Suite.Hearts),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }
            game.FindHighestPair(players);
            for (int i = 0; i < 1; i++)
            {
                Assert.Greater(player.Hand.PairRanks[i], playerTwo.Hand.PairRanks[i]);
            }
        }

        /// <summary>
        /// Find the HigestThree 
        /// </summary>
        [Test]
        public void FindHighestThree()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Seven, Suite.Spades),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Ace, Suite.Diamonds),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Four, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Three, Suite.Spades),
                new Card(Rank.Three, Suite.Hearts),
                new Card(Rank.Three, Suite.Diamonds),
                new Card(Rank.Nine, Suite.Diamonds),
                new Card(Rank.Four, Suite.Hearts),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }
            game.FindHighestThree(players);
            Assert.Greater(player.Hand.ThreeRank, playerTwo.Hand.ThreeRank);
        }
        /// <summary>
        /// Check If both have same HighCard  
        /// </summary>
        [Test]
        public void IfPlayersHaveSameHighCard()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Seven, Suite.Spades),
                new Card(Rank.Two, Suite.Hearts),
                new Card(Rank.Three, Suite.Diamonds),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Four, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Three, Suite.Spades),
                new Card(Rank.Six, Suite.Hearts),
                new Card(Rank.Ace, Suite.Diamonds),
                new Card(Rank.Nine, Suite.Diamonds),
                new Card(Rank.Four, Suite.Hearts),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }
            game.CheckBestHand(players);
            game.FindHighestCard(players);
            for (int i = 0; i < 5; i++)
            {
                Assert.GreaterOrEqual(playerTwo.Hand.Ranks[i], player.Hand.Ranks[i]);
            }
            Assert.Greater(playerTwo.Wins, player.Wins);
        }

        /// <summary>
        /// Find the HigestThree   <!-- Ej klar--->
        /// </summary>
        [Test]
        public void IfPlayersHaveSamePair()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Seven, Suite.Spades),
                new Card(Rank.Two, Suite.Hearts),
                new Card(Rank.Three, Suite.Diamonds),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Ace, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Ace, Suite.Spades),
                new Card(Rank.Six, Suite.Hearts),
                new Card(Rank.Ace, Suite.Diamonds),
                new Card(Rank.Nine, Suite.Diamonds),
                new Card(Rank.Four, Suite.Hearts),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }

            game.FindHighestPair(players);
            for (int i = 0; i < 1; i++)
            {
                Assert.GreaterOrEqual(playerTwo.Hand.PairRanks[i], player.Hand.PairRanks[i]);
            }
            game.CheckBestHand(players);
            Assert.Greater(playerTwo.Wins, player.Wins);
        }


        /// <summary>
        /// Find the HigestThree   <!-- Ej klar--->
        /// </summary>
        [Test]
        public void IfBothPlayersHaveTwoPairs()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Six, Suite.Spades),
                new Card(Rank.Six, Suite.Hearts),
                new Card(Rank.Five, Suite.Diamonds),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Ace, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Ace, Suite.Spades),
                new Card(Rank.Six, Suite.Hearts),
                new Card(Rank.Ace, Suite.Diamonds),
                new Card(Rank.Six, Suite.Diamonds),
                new Card(Rank.Four, Suite.Hearts),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }

            if (player.Hand.HandType.Equals(playerTwo.Hand.HandType))
            {
                game.CheckBestHand(players);
                Assert.GreaterOrEqual(playerTwo.Hand.FourRank, player.Hand.FourRank);
            }
            Assert.Greater(player.Wins, playerTwo.Wins);
        }
        /// <summary>
        /// Find the FillHouse   <!-- Ej klar--->
        /// </summary>
        [Test]
        public void IfBothPlayersHaveFullHouse()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Six, Suite.Spades),
                new Card(Rank.Six, Suite.Hearts),
                new Card(Rank.Six, Suite.Diamonds),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Ace, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Ace, Suite.Spades),
                new Card(Rank.Six, Suite.Hearts),
                new Card(Rank.Ace, Suite.Diamonds),
                new Card(Rank.Six, Suite.Diamonds),
                new Card(Rank.Six, Suite.Hearts),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }

            if (player.Hand.HandType.Equals(playerTwo.Hand.HandType))
            {
                game.CheckBestHand(players);
                Assert.GreaterOrEqual(playerTwo.Hand.ThreeRank, player.Hand.ThreeRank);
                Assert.AreEqual(playerTwo.Wins, player.Wins);
            }
        }

        /// <summary>
        /// Find the FillHouse   <!-- Ej klar--->
        /// </summary>
        [Test]
        public void IfBothPlayersHaveStraight()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Two, Suite.Spades),
                new Card(Rank.Three, Suite.Hearts),
                new Card(Rank.Four, Suite.Diamonds),
                new Card(Rank.Five, Suite.Hearts),
                new Card(Rank.Six, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Ace, Suite.Spades),
                new Card(Rank.King, Suite.Hearts),
                new Card(Rank.Queen, Suite.Diamonds),
                new Card(Rank.Jack, Suite.Diamonds),
                new Card(Rank.Ten, Suite.Hearts),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }
            game.CheckBestHand(players);
            if (player.Hand.HandType.Equals(playerTwo.Hand.HandType))
            {
                for (int i = 0; i < 5; i++)
                {
                    Assert.GreaterOrEqual(playerTwo.Hand.Ranks[i], player.Hand.Ranks[i]);
                }
                Assert.Greater(playerTwo.Wins, player.Wins);
            }
        }


        /// <summary>
        /// Find the FillHouse   <!-- Ej klar--->
        /// </summary>
        [Test]
        public void IfBothPlayersHaveStraightFlush()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Two, Suite.Spades),
                new Card(Rank.Three, Suite.Spades),
                new Card(Rank.Four, Suite.Spades),
                new Card(Rank.Five, Suite.Spades),
                new Card(Rank.Six, Suite.Spades),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Six, Suite.Diamonds),
                new Card(Rank.Seven, Suite.Diamonds),
                new Card(Rank.Eight, Suite.Diamonds),
                new Card(Rank.Nine, Suite.Diamonds),
                new Card(Rank.Ten, Suite.Diamonds),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }
            game.CheckBestHand(players);
            if (player.Hand.HandType.Equals(playerTwo.Hand.HandType))
            {
                for (int i = 0; i < 5; i++)
                {
                    Assert.GreaterOrEqual(playerTwo.Hand.Ranks[i], player.Hand.Ranks[i]);
                }
                Assert.Greater(playerTwo.Wins, player.Wins);
            }
        }


        /// <summary>
        /// Find the FillHouse   <!-- Ej klar--->
        /// </summary>
        [Test]
        public void IfBothPlayersHaveRoyalStraightFlush()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Ace, Suite.Spades),
                new Card(Rank.King, Suite.Spades),
                new Card(Rank.Queen, Suite.Spades),
                new Card(Rank.Jack, Suite.Spades),
                new Card(Rank.Ten, Suite.Spades),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Ace, Suite.Diamonds),
                new Card(Rank.King, Suite.Diamonds),
                new Card(Rank.Queen, Suite.Diamonds),
                new Card(Rank.Jack, Suite.Diamonds),
                new Card(Rank.Ten, Suite.Diamonds),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }
            game.CheckBestHand(players);
            if (player.Hand.HandType.Equals(playerTwo.Hand.HandType))
            {
                for (int i = 0; i < 5; i++)
                {
                    Assert.GreaterOrEqual(playerTwo.Hand.Ranks[i], player.Hand.Ranks[i]);
                }
                Assert.GreaterOrEqual(playerTwo.Wins, player.Wins);
            }
        }

        /// <summary>
        /// Find the HighestCard if both have Flush   <!-- Ej klar--->
        /// </summary>
        [Test]
        public void IfBothPlayersHaveFlush()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Two, Suite.Clubs),
                new Card(Rank.Seven, Suite.Clubs),
                new Card(Rank.Four, Suite.Clubs),
                new Card(Rank.Five, Suite.Clubs),
                new Card(Rank.Six, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Nine, Suite.Spades),
                new Card(Rank.King, Suite.Spades),
                new Card(Rank.Seven, Suite.Spades),
                new Card(Rank.Jack, Suite.Spades),
                new Card(Rank.Ten, Suite.Spades),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }
            game.CheckBestHand(players);
            if (player.Hand.HandType.Equals(playerTwo.Hand.HandType))
            {
                for (int i = 0; i < 5; i++)
                {
                    Assert.Greater(playerTwo.Hand.Ranks[i], player.Hand.Ranks[i]);
                }
                Assert.Greater(playerTwo.Wins, player.Wins);
            }
        }
        /// <summary>
        /// Find the FillHouse   <!-- Ej klar--->
        /// </summary>
        [Test]
        public void IfBothPlayersHaveFullHouseAndCheckHighestHighCard()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Two, Suite.Spades),
                new Card(Rank.Two, Suite.Hearts),
                new Card(Rank.Two, Suite.Diamonds),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Ace, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.King, Suite.Spades),
                new Card(Rank.Six, Suite.Hearts),
                new Card(Rank.King, Suite.Diamonds),
                new Card(Rank.Six, Suite.Diamonds),
                new Card(Rank.Six, Suite.Hearts),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }

            if (player.Hand.HandType.Equals(playerTwo.Hand.HandType))
            {
                game.FindHighestThree(players);
                Assert.Greater(playerTwo.Hand.ThreeRank, player.Hand.ThreeRank);
            }
            game.CheckBestHand(players);
            Assert.GreaterOrEqual(playerTwo.Hand.HandType, player.Hand.HandType);
        }



        /// <summary>
        /// Find the FourOfAKind   <!-- Ej klar--->
        /// </summary>
        [Test]
        public void IfBothPlayersHaveFourOfAKind()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Six, Suite.Spades),
                new Card(Rank.Six, Suite.Hearts),
                new Card(Rank.Ace, Suite.Diamonds),
                new Card(Rank.Six, Suite.Hearts),
                new Card(Rank.Six, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.King, Suite.Spades),
                new Card(Rank.Six, Suite.Hearts),
                new Card(Rank.Six, Suite.Diamonds),
                new Card(Rank.Six, Suite.Diamonds),
                new Card(Rank.Six, Suite.Hearts),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }
            if (player.Hand.HandType.Equals(playerTwo.Hand.HandType))
            {
                game.CheckBestHand(players);
                Assert.GreaterOrEqual(playerTwo.Hand.FourRank, player.Hand.FourRank);
                game.FindHighestCard(players);
                for (int i = 0; i < 5; i++)
                {
                    Assert.GreaterOrEqual(player.Hand.Ranks[i], playerTwo.Hand.Ranks[i]);
                }
                Assert.Greater(player.Wins, playerTwo.Wins);
            }
        }




        /// <summary>
        /// Find the HigestThree   <!-- Ej klar--->
        /// </summary>
        [Test]
        public void IfBothPlayersHaveThreeofSameCards()
        {
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Ace, Suite.Spades),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Ace, Suite.Diamonds),
                new Card(Rank.Two, Suite.Hearts),
                new Card(Rank.Three, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Ace, Suite.Spades),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Ace, Suite.Diamonds),
                new Card(Rank.Nine, Suite.Diamonds),
                new Card(Rank.Four, Suite.Hearts),
            };
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }
            game.FindHighestThree(players);
            Assert.AreEqual(playerTwo.Hand.ThreeRank, player.Hand.ThreeRank);
            game.CheckBestHand(players);
            Assert.Greater(playerTwo.Wins, player.Wins);
        }


        /// <summary>
        /// Test if Game knows who's the winner <!-- EJ klar-->
        /// </summary>
        [Test]
        public void PokerGameKnowsTheWinner()
        {
            //Arrange
            Player player = new Player("Dhan", 0);
            Player playerTwo = new Player("Erik", 0);
            List<Player> players = new List<Player> { player, playerTwo };
            List<Card> CardForFirstPlayer = new List<Card>
            {
                new Card(Rank.Ace, Suite.Spades),
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.Ace, Suite.Diamonds),
                new Card(Rank.Two, Suite.Hearts),
                new Card(Rank.Two, Suite.Clubs),
            };
            List<Card> CardForSecondPlayer = new List<Card>
            {
                new Card(Rank.Ace, Suite.Hearts),
                new Card(Rank.King, Suite.Hearts),
                new Card(Rank.Queen, Suite.Hearts),
                new Card(Rank.Jack, Suite.Hearts),
                new Card(Rank.Ten, Suite.Hearts),
            };

            //Act
            foreach (Card cards in CardForFirstPlayer)
            {
                player.GetCard(cards);
            }
            foreach (Card cards in CardForSecondPlayer)
            {
                playerTwo.GetCard(cards);
            }

            foreach (Player playe in players)
            {
                playe.Hand.EvaluateHand();
            }
            //Asserts
            game.CheckBestHand(players);
            Assert.Greater(playerTwo.Hand.HandType, player.Hand.HandType);
            Assert.Greater(playerTwo.Wins, player.Wins);
        }


        /// <summary>
        /// Test to make sure you cant create blank Names
        /// </summary>
        [Test]
        public void PokerGameCanOnlyCreatePlayersWithName()
        {
            //Arrrange
            string nameLess = null;
            List<string> players = new List<string> { "Dhan", "Erik" };
            PokerGame game = new PokerGame(players.ToArray());

            Assert.AreEqual(2, players.Count);

            players.Add(nameLess);

            var ex = Assert.Throws<NullReferenceException>(
                 () => new PokerGame(players.ToArray())
                 );

            Assert.AreEqual("Cannot create players, no name was given, Try Again!", ex.Message);

        }
    }
}