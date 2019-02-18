using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForageBuddy
{
    public class PlayerScore
    {
        public string PlayerName { get; }
        public int TotalScore { get; }

        private readonly int _totalOneChests;
        private readonly int _totalTwoChests;
        private readonly int _totalThreeChests;

        public PlayerScore(string playerName, int totalOneChests, int totalTwoChests, int totalThreeChests)
        {
            PlayerName = playerName;
            _totalOneChests = totalOneChests;
            _totalTwoChests = totalTwoChests;
            _totalThreeChests = totalThreeChests;
            TotalScore = (totalOneChests) + (totalTwoChests * 2) + (totalThreeChests * 3);
        }

        public string PlayerScoreString()
        {
            return $"{PlayerName} - {(_totalOneChests*1) + (_totalTwoChests*2) + (_totalThreeChests*3)}";
        }

        public string PlayerScoreStringBreakdown()
        {
            return $"{PlayerName} - {_totalOneChests}, {_totalTwoChests}, {_totalThreeChests}";
        }
    }
}
