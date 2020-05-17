using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;

namespace ForageBuddy
{
    public class ForageCalculator : IForageCalculator
    {
        private string _folderPath;

        private List<string> _filesInScreenshotDirectory;
        private Dictionary<string, PlayerScore> _playerScores;
        
        private readonly ILogger _logger;
        private readonly IImageParser _imageParser;

        public ForageCalculator(IImageParser imageParser, ILogger logger)
        {
            _imageParser = imageParser;
            _logger = logger;
        }

        public void SetDirectory(string directory)
        {
            _folderPath = directory;
            _filesInScreenshotDirectory = GetPngFilesInDirectory();
            _logger.Information($"Folder selected: {_folderPath}");
        }

        public List<string> CalculateScores()
        {
            if (string.IsNullOrEmpty(_folderPath))
                throw new FileLoadException("Folder Path not set.");
            
            _logger.Information("Calculating scores...");

            _playerScores = new Dictionary<string, PlayerScore>();

            var allFilesInOutputDir = GetPngFilesInDirectory();

            var tmpPlayerScores = new List<PlayerScore>();

            foreach (var file in allFilesInOutputDir.Where(x => !_filesInScreenshotDirectory.Contains(x)))
                tmpPlayerScores.AddRange(_imageParser.GetPlayerScoresInImage(file));


            foreach (var score in tmpPlayerScores)
            {
                if (!_playerScores.ContainsKey(score.PlayerName))
                {
                    _playerScores.Add(score.PlayerName, score);
                    _logger.Information($"Added: {score.PlayerScoreStringBreakdown()}");
                }
                else if (score.TotalScore > _playerScores[score.PlayerName].TotalScore)
                {
                    _logger.Information($"Updated: {_playerScores[score.PlayerName].PlayerScoreStringBreakdown()} --> {score.PlayerScoreStringBreakdown()}");
                    _playerScores[score.PlayerName] = score;
                }
            }
            
            _logger.Information("Ordering scores...");
            return _playerScores
                .Select(x => x.Value)
                .OrderBy(x => x.TotalScore)
                .Select(x => x.PlayerScoreString())
                .ToList();
        }

        public string GetScoreString()
        => _playerScores != null && _playerScores.Count > 0 
                ? string.Join("\n",_playerScores.Select(x => x.Value)
                    .OrderByDescending(x => x.TotalScore)
                    .ToList()
                    .Select(x => x.PlayerScoreString()))
                : string.Empty;

        public void ResetCalculator()
        {
            if (string.IsNullOrEmpty(_folderPath))
                throw new FileLoadException("Folder Path not set.");
            
            _filesInScreenshotDirectory = GetPngFilesInDirectory();  
            _logger.Information("Directory status reset.");
            _playerScores = new Dictionary<string, PlayerScore>();
        }
        
        private List<string> GetPngFilesInDirectory()
            => Directory.EnumerateFiles(_folderPath)
                .ToList()
                .FindAll(x => x.Split('.').LastOrDefault() == "png");

        public string GetCursedChestScoreString()
        => _playerScores != null && _playerScores.Count > 0
                ? string.Join("\n", _playerScores.Select(x => x.Value)
                    .OrderByDescending(x => x.CursedChestScore)
                    .ToList()
                    .Select(x => x.PlayerScoreString()))
                : string.Empty;
    }
}