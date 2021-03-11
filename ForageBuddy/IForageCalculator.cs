using System;
using System.Collections.Generic;
using System.Text;

namespace ForageBuddy
{
    public interface IForageCalculator
    {
        List<string> CalculateScores();

        string GetScoreString();
    }
}
