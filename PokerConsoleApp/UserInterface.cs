using System;
using System.Linq;
using System.Collections.Generic;
using static System.Console;

namespace Poker.ConsoleApp
{
    static class UserInterface
    {
        static public string[] RegisterPlayers()
        {
            UserInterface.Clear();

            List<string> playerNames = new List<string>();
            while (true)
            {
                string playerName = RegisterPlayer();
                if (playerName == null)
                {
                    break;
                }
                playerNames.Add(playerName);
            }
            return playerNames.ToArray();
        }

        static public void AnnounceNewDeal()
        {
            UserInterface.Clear();
            UserInterface.Message("Ny giv.");
            UserInterface.Clear();
        }

        static public ICard[] SelectCardsToDiscard(IPlayer player)
        {
            UserInterface.GetReady(player);

            bool[] selected = new bool[5];
            bool validKey = true;
            while (true)
            {
                if (validKey)
                {
                    Clear();

                    PrintHand(player);

                    for (int i = 0; i < selected.Length; ++i)
                    {
                        Write($" {i + 1}  ");
                    }
                    WriteLine();

                    for (int i = 0; i < selected.Length; ++i)
                    {
                        Write($"[{(selected[i] ? 'X' : ' ')}] ");
                    }
                    WriteLine();
                    WriteLine("[1-5] Välj kort, [Enter] Kasta");
                }

                validKey = false;

                var keyInfo = ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }

                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.D2:
                    case ConsoleKey.D3:
                    case ConsoleKey.D4:
                    case ConsoleKey.D5:
                        int keyIndex = int.Parse(keyInfo.KeyChar.ToString()) - 1;
                        selected[keyIndex] = !selected[keyIndex];
                        validKey = true;
                        break;
                    default:
                        break;
                }
            }

            List<ICard> selectedCards = new List<ICard>();

            for (int i = 0; i < selected.Length; i++)
            {
                if (selected[i])
                {
                    selectedCards.Add(player.Hand[i]);
                }
            }

            return selectedCards.ToArray();
        }

        static public void InspectReplacementCards(IPlayer player)
        {
            UserInterface.Clear();
            UserInterface.PrintHand(player);
            UserInterface.WaitForKey();
            UserInterface.Clear();
        }

        static public void ShowHand(IPlayer player)
        {
            UserInterface.PrintHand(player);
            UserInterface.PresentHandType(player.HandType);
        }

        static public bool PresentStats(IPlayer[] players)
        {
            WriteLine("Totalt antal vinster\n"
                + "====================\n"
                + String.Join(", ",
                    from player in players
                    select $"{player.Name}: {player.Wins}"));
            return Char.ToLower(
                    WaitForKey("[Enter] Fortsätt, [Q] Avsluta").KeyChar) !=
                    'q';
        }

        static public void PresentWinner(IPlayer winner)
        {
            WriteLine($"Vinnaren är: {winner.Name}\n");
        }

        static public void DeclareDraw(IPlayer[] tiedPlayers)
        {
            WriteLine("Oavgjort mellan: " + String.Join(", ",
                from player in tiedPlayers select player.Name));
        }

        static public void ProgramError(string message)
        {
            Console.WriteLine($"Program error: {message}");
            Environment.Exit(1);
        }

        static private void Clear()
        {
            Console.Clear();
        }

        static private void Message(string message)
        {
            WriteLine(message + "\n");
            WaitForKey();
        }

        static public ConsoleKeyInfo WaitForKey(
            string prompt = "Tryck [Enter] för att fortsätta..")
        {
            WriteLine(prompt);
            return ReadKey(true);
        }

        static private string RegisterPlayer()
        {
            Write($"Namn på spelare: ");
            string name = ReadLine();
            return name.Trim() == "" ? null : name;
        }

        static private void PresentHandType(HandType handType)
        {
            WriteLine(handType switch
            {
                HandType.RoyalStraightFlush => "Royal straight flush",
                HandType.StraightFlush => "Färgstege",
                HandType.FourOfAKind => "Fyrtal",
                HandType.FullHouse => "Kåk",
                HandType.Flush => "Färg",
                HandType.Straight => "Stege",
                HandType.ThreeOfAKind => "Triss",
                HandType.TwoPairs => "Två par",
                HandType.Pair => "Ett par",
                HandType.HighCard => "Högsta kort",
                _ => throw new NotImplementedException("Programmer error"),
            } + "\n");
        }

        static private void GetReady(IPlayer player)
        {
            Clear();

            WriteLine(player.Name);

            var fg = ForegroundColor;
            var bg = BackgroundColor;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < player.Hand.Length; ++j)
                {
                    BackgroundColor = ConsoleColor.Blue;
                    Write("   ");
                    BackgroundColor = bg;
                    Write(" ");
                }
                WriteLine();
            }

            WaitForKey();

            Clear();
        }

        static private void PrintHand(IPlayer player)
        {
            ConsoleColor color(Suite suite) => suite switch
            {
                Suite.Hearts => ConsoleColor.Red,
                Suite.Diamonds => ConsoleColor.Red,
                _ => ConsoleColor.Black,
            };

            string rank2str(Rank rank) => rank switch
            {
                Rank.Two => "2",
                Rank.Three => "3",
                Rank.Four => "4",
                Rank.Five => "5",
                Rank.Six => "6",
                Rank.Seven => "7",
                Rank.Eight => "8",
                Rank.Nine => "9",
                Rank.Ten => "10",
                Rank.Jack => "J",
                Rank.Queen => "Q",
                Rank.King => "K",
                Rank.Ace => "A",
                _ => "?",
            };

            char suite2char(Suite suite) => suite switch
            {
                Suite.Diamonds => '♦',
                Suite.Clubs => '♣',
                Suite.Hearts => '♥',
                Suite.Spades => '♠',
                _ => '?'
            };

            WriteLine(player.Name);

            var cards = player.Hand;
            var fg = ForegroundColor;
            var bg = BackgroundColor;
            for (int i = 0; i < cards.Length; ++i)
            {
                ICard card = cards[i];
                BackgroundColor = ConsoleColor.White;
                ForegroundColor = color(card.Suite);
                var rank = rank2str(card.Rank);
                Write(rank + new string(' ', 3 - rank.Length));
                BackgroundColor = bg;
                Write(" ");
            }
            WriteLine();
            for (int i = 0; i < cards.Length; ++i)
            {
                ICard card = cards[i];
                BackgroundColor = ConsoleColor.White;
                ForegroundColor = color(card.Suite);
                Write(" " + suite2char(card.Suite) + " ");
                BackgroundColor = bg;
                Write(" ");
            }
            WriteLine();
            for (int i = 0; i < cards.Length; ++i)
            {
                ICard card = cards[i];
                BackgroundColor = ConsoleColor.White;
                ForegroundColor = color(card.Suite);
                var rank = rank2str(card.Rank);
                Write(new string(' ', 3 - rank.Length) + rank);
                BackgroundColor = bg;
                Write(" ");
            }
            ForegroundColor = fg;
            WriteLine();
        }
    }
}