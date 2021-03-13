using NUnit.Framework;

namespace ForageBuddy.Tests
{
    class PlayerScoreTests
    {
        [TestCase(10, 10, 10, 60)]
        [TestCase(5, 5, 5, 30)]
        [TestCase(0, 0, 0, 0)]
        public void OnNewPlayerScoreTest_Counts_AreReturnedCorrectly(int boneBoxes, int fetishJars, int cursedChests, int total)
        {
            // Arrange 
            var playerScore = new PlayerScore("Test", boneBoxes, fetishJars, cursedChests);

            // Act
            var score = playerScore.PlayerScoreString();

            // Assert
            Assert.AreEqual($"Test - {total}", score);
        }

        [TestCase("Test")]
        [TestCase("AnotherTest")]
        [TestCase("ThisTest")]
        public void OnNewPlayerScoreTest_PlayerName_IsReturnedCorrectly(string name)
        {
            // Arrange 
            var playerScore = new PlayerScore(name, 1, 1, 1);

            // Act
            var score = playerScore.PlayerScoreString();

            // Assert
            Assert.AreEqual($"{name} - 6", score);
        }
    }
}
