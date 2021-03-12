using LockedBitmapUtil;
using System.Collections.Generic;

namespace ForageBuddy
{
    public interface IImageParser
    {
        IEnumerable<PlayerScore> GetPlayerScoresInImage(LockedBitmap image);
    }
}
