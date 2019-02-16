using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForageBuddy
{
    public class PlayerScore
    {
        private readonly string _playerName;
        private readonly int _totalOneChests;
        private readonly int _totalTwoChests;
        private readonly int _totalThreeChests;
        private readonly int _totalScore;

        public PlayerScore(string playerName, int totalOneChests, int totalTwoChests, int totalThreeChests)
        {
            _playerName = playerName;
            _totalOneChests = totalOneChests;
            _totalTwoChests = totalTwoChests;
            _totalThreeChests = totalThreeChests;
            _totalScore = (totalOneChests) + (totalTwoChests * 2) + (totalThreeChests * 3);
        }

        public string PlayerScoreString()
        {
            return $"{_playerName} - {(_totalOneChests*1) + (_totalTwoChests*2) + (_totalThreeChests*3)}.";
        }

        public string PlayerScoreStringBreakdown()
        {
            return $"{_playerName} - {_totalOneChests}, {_totalTwoChests}, {_totalThreeChests}.";
        }

        public string GetPlayerName()
        {
            return _playerName;
        }

        public int GetTotalScore()
        {
            return _totalScore;
        }
    }
}
