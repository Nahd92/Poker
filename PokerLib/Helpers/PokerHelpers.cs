using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Poker.Lib
{
    public class PokerHelpers
    {
        public static string Serialize(Player player)
        {
            return $"{player.Name} -- {player.Wins}";
        }

        public static Player Deserialize(string line)
        {
            Regex re = new Regex(@"([^""]+)");
            Match match = re.Match(line);
            return new Player()
            {
                Name = match.Groups[1].Value,
            };
        }
    }
}