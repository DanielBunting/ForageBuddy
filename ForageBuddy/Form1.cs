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
        private readonly string _logFile = "log.txt";

        public Form1()
        {
            _imageParser = new ImageParser();
            InitializeComponent();
            lbScores.Items.Insert(0, "Please specify a screenshot folder.");

            var logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

            // Check for logFolder, create if missing
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            // Check for logFile, create new name if one already exists
            if (File.Exists(Path.Combine(logFolder, _logFile)))
            {
                var i = 1;
                
                _logFile = _logFile.Insert(_logFile.IndexOf("."), $" ({i})");
                
                while (File.Exists(Path.Combine(logFolder, _logFile)))
                {
                    i++;
                    _logFile = _logFile.Replace($"{i - 1}", $"{i}");
                }
            }
            
            // Create logger to record program activity
            _logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(logFolder, _logFile))
                .CreateLogger();
            
            _logger.Information("Application started.");
            _logger.Information($"Logging at: {Path.Combine(logFolder, _logFile)}");
        }

        private void btnFolderSelect_Click(object sender, EventArgs e)
        {
            _logger.Information($"\"{sender.ToString().Substring(sender.ToString().IndexOf("Text: ") + 6)}\" button pressed.");
            
            // Select an output folder
            _logger.Information("Selecting output folder...");
            
            if (fbdScreenieFolder.ShowDialog() != DialogResult.OK)
            {
                _logger.Information("Folder not selected. Returning.");
                return;
            }
            
            _folderPath = fbdScreenieFolder.SelectedPath;
            _logger.Information($"Selected folder: {fbdScreenieFolder.SelectedPath}");
            
            // Retrieve screenshots from selected folder
            _filesInScreenshotDirectory = GetPngFiles();
            _logger.Information($"Found {_filesInScreenshotDirectory.Count} pre-existing files:");
            foreach(string str in _filesInScreenshotDirectory)
                _logger.Information($"  {str}");

            // Update GUI display status
            lbScores.Items.Clear();
            lbScores.Items.Insert(0, "Ready");
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            _logger.Information($"\"{sender.ToString().Substring(sender.ToString().IndexOf("Text: ") + 6)}\" button pressed.");
            
            if (_folderPath == null)
            {
                MessageBox.Show(@"Screenshot output folder not set.");
                _logger.Information("\"_folderPath\" variable not set. Returning.");
                return;
            }

            lbScores.Items.Clear();
            _playerScores = new Dictionary<string, PlayerScore>();

            var allFilesInOutputDir = GetPngFiles();

            var tmpPlayerScores = new List<PlayerScore>();

            _logger.Information("New images found:");
            foreach (var file in allFilesInOutputDir.Where(x => !_filesInScreenshotDirectory.Contains(x)))
            {
                _logger.Information($"  {file.ToString()}");
                tmpPlayerScores.AddRange(_imageParser.GetPlayerScoresInImage(file, _logger));
            }

            foreach (var score in tmpPlayerScores)
            {
                if (!_playerScores.ContainsKey(score.PlayerName))
                {
                    _playerScores.Add(score.PlayerName, score);
                    _logger.Information(score.PlayerScoreStringBreakdown());
                }

                if (score.TotalScore > _playerScores[score.PlayerName].TotalScore)
                {
                    _playerScores[score.PlayerName] = score;
                }
            }
            
            var orderedScores = _playerScores.Select(x => x.Value).OrderBy(x => x.TotalScore).ToList();
            
            foreach(var score in orderedScores)
            {
                _logger.Information($"lorem ispsum");
                lbScores.Items.Insert(0, score.PlayerScoreString());
            }
        }

        private void btnCopyScores_Click(object sender, EventArgs e)
        {
            _logger.Information($"\"{sender.ToString().Substring(sender.ToString().IndexOf("Text: ") + 6)}\" button pressed.");
            
            if (_playerScores == null || _playerScores.Count == 0) return;

            var orderedScores = _playerScores.Select(x => x.Value).OrderByDescending(x => x.TotalScore).ToList().Select(x => x.PlayerScoreString());
            var output = string.Join("\n", orderedScores);
            
            // copy to clipboard
            Clipboard.SetText(output);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _logger.Information($"\"{sender.ToString().Substring(sender.ToString().IndexOf("Text: ") + 6)}\" button pressed.");
            
            if (_folderPath == null) return;

            _playerScores = null;
            lbScores.Items.Clear();
            lbScores.Items.Insert(0,"Ready");
            _filesInScreenshotDirectory = GetPngFiles();
        }

        private List<string> GetPngFiles()
        => Directory.EnumerateFiles(_folderPath)
            .ToList()
            .FindAll(x => x.Split('.').LastOrDefault() == "png");
    }
}
