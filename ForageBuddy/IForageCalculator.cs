using System.Collections.Generic;
using System.Linq;

namespace ForageBuddy
{
    public interface IForageCalculator
    {
        void SetDirectory(string directory);

        List<string> CalculateScores();

        string GetScoreString();

        string GetCursedChestScoreString();

        void ResetCalculator();
    }
}