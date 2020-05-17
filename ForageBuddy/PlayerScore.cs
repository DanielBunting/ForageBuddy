namespace ForageBuddy
{
    public class PlayerScore
    {
        public string PlayerName { get; }
        public int TotalScore { get; }

        public int CursedChestScore { get => _totalCursedChests; }

        private readonly int _totalBoneBoxes;
        private readonly int _totalFetishJars;
        private readonly int _totalCursedChests;

        public PlayerScore(string playerName, int totalBoneBoxes, int totalFetishJars, int totalCursedChests)
        {
            PlayerName = playerName;
            _totalBoneBoxes = totalBoneBoxes;
            _totalFetishJars = totalFetishJars;
            _totalCursedChests = totalCursedChests;
            TotalScore = (totalBoneBoxes) + (totalFetishJars * 2) + (totalCursedChests * 3);
        }

        public string PlayerScoreString()
        {
            return $"{PlayerName} - {(_totalBoneBoxes*1) + (_totalFetishJars*2) + (_totalCursedChests*3)} ({_totalCursedChests})";
        }

        public string PlayerScoreStringBreakdown()
        {
            return $"{PlayerName} - {_totalBoneBoxes}, {_totalFetishJars}, {_totalCursedChests}";
        }
    }
}
