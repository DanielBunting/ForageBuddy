namespace ForageBuddy
{
    public record PlayerScore
    {
        public string PlayerName { get; }
        public int TotalScore { get; }

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
        => $"{PlayerName} - {TotalScore}";

        public string PlayerDetailedScoreString()
        => $"{PlayerName} - {TotalScore} -> ({_totalBoneBoxes}, {_totalFetishJars}, {_totalCursedChests})";

        public string PlayerScoreStringBreakdown()
        => $"{PlayerName} - {_totalBoneBoxes}, {_totalFetishJars}, {_totalCursedChests}";
    }
}
