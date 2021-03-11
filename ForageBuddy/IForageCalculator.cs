using System;
using System.Collections.Generic;

namespace ForageBuddy
{
    public interface IForageCalculator
    {
        // This 'could/should' be async.
        List<string> CalculateScores(IntPtr clientHandle);
    }
}
