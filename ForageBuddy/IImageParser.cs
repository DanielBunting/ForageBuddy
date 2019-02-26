using System.Collections.Generic;

namespace ForageBuddy
{
    public interface IImageParser
    {
        IEnumerable<PlayerScore> GetPlayerScoresInImage(string filePath);
    }
}