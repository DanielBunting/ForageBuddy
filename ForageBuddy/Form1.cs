using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Serilog;
using Serilog.Events;

namespace ForageBuddy
{
    public partial class Form1 : Form
    {
        private string _folderPath;
        private List<string> _filesInScreenshotDirectory;
        private Dictionary<string, PlayerScore> _playerScores;
        
        private readonly ILogger _logger;
        private readonly ImageParser _imageParser;

        public Form1()
        {
            _imageParser = new ImageParser();
            InitializeComponent();
            lbScores.Items.Insert(0, "Please specify a screenshot folder.");

            var logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);
            
            _logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(logFolder, "log.txt"))
                .CreateLogger();
            
            _logger.Information("Application started.");
        }

        private void btnFolderSelect_Click(object sender, EventArgs e)
        {
            _logger.Information("Selecting output folder...");
            
            if (fbdScreenieFolder.ShowDialog() != DialogResult.OK)
            {
                _logger.Information("No folder selected.");
                return;
            }
            
            _folderPath = fbdScreenieFolder.SelectedPath;

            _filesInScreenshotDirectory = GetPngFiles();

            lbScores.Items.Clear();
            lbScores.Items.Insert(0, "Ready");
            
            _logger.Information("Folder selected.");
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (_folderPath == null)
            {
                MessageBox.Show(@"Screenshot output folder not set.");
                return;
            }
            
            _logger.Information("Calculating scores...");

            lbScores.Items.Clear();
            _playerScores = new Dictionary<string, PlayerScore>();

            var allFilesInOutputDir = GetPngFiles();

            var tmpPlayerScores = new List<PlayerScore>();

            foreach (var file in allFilesInOutputDir.Where(x => !_filesInScreenshotDirectory.Contains(x)))
                tmpPlayerScores.AddRange(_imageParser.GetPlayerScoresInImage(file, _logger));


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
            
            var orderedScores = _playerScores.Select(x => x.Value).OrderBy(x => x.TotalScore).ToList();
            
            _logger.Information("Ordering scores...");
            foreach(var score in orderedScores)
            {
                lbScores.Items.Insert(0, score.PlayerScoreString());
                _logger.Information($"[{score.TotalScore}]: {score.PlayerScoreStringBreakdown()}");
            }
        }

        private void btnCopyScores_Click(object sender, EventArgs e)
        {
            _logger.Information("Copying scores...");
            
            if (_playerScores == null || _playerScores.Count == 0)
            {
                _logger.Information("No scores to copy.");
                return;
            }

            var orderedScores = _playerScores.Select(x => x.Value).OrderByDescending(x => x.TotalScore).ToList().Select(x => x.PlayerScoreString());
            var output = string.Join("\n", orderedScores);
            
            Clipboard.SetText(output);
            
            _logger.Information("Scores copied.");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _logger.Information("Resetting directory history...");

            if (_folderPath == null)
            {
                _logger.Information("No directory to reset.");
                return;
            }

            _playerScores = null;
            _filesInScreenshotDirectory = GetPngFiles();
            
            lbScores.Items.Clear();
            lbScores.Items.Insert(0,"Ready");
            
            _logger.Information("Ready for new files.");
        }

        private List<string> GetPngFiles()
        => Directory.EnumerateFiles(_folderPath)
            .ToList()
            .FindAll(x => x.Split('.').LastOrDefault() == "png");
    }
}
