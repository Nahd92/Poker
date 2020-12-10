
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Poker.Lib
{
    public class PokerHelpers
    {
        public static Player Deserialize(string line)
        {
            Regex re = new Regex(@"([^""]+) (\d+)");
            Match match = re.Match(line);
            return new Player()
            {
                Name = match.Groups[1].Value,
                Wins = int.Parse(match.Groups[2].Value)
            };
        }
    }
}