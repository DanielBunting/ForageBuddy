using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ForageBuddy
{
    public partial class Form1 : Form
    {
        private string _folderPath;
        private List<string> _filesInScreenshotDirectory;
        private Dictionary<string, PlayerScore> _playerScores;

        private readonly ImageParser _imageParser;

        public Form1()
        {
            _imageParser = new ImageParser();
            InitializeComponent();
            lbScores.Items.Insert(0, "Please Specify Screenshot folder");
        }

        private void btnFolderSelect_Click(object sender, EventArgs e)
        {
            if (fbdScreenieFolder.ShowDialog() != DialogResult.OK) return;
            
            Console.WriteLine(fbdScreenieFolder.SelectedPath);
            _folderPath = fbdScreenieFolder.SelectedPath;
            _filesInScreenshotDirectory = GetPngFiles();

            lbScores.Items.Clear();
            lbScores.Items.Insert(0, "Ready");
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (_folderPath == null)
            {
                MessageBox.Show("Screenshot output folder not set.");
                return;
            }

            lbScores.Items.Clear();
            _playerScores = new Dictionary<string, PlayerScore>();

            var allFilesInOutputDir = GetPngFiles();

            var tmpPlayerScores = new List<PlayerScore>();

            foreach (var file in allFilesInOutputDir.Where(x => !_filesInScreenshotDirectory.Contains(x)))
            {
                tmpPlayerScores.AddRange(_imageParser.GetPlayerScoresInImage(file));
            }

            foreach (var score in tmpPlayerScores)
            {
                if (!_playerScores.ContainsKey(score.PlayerName)) _playerScores.Add(score.PlayerName, score);
                
                if (score.TotalScore > _playerScores[score.PlayerName].TotalScore)
                {
                    _playerScores[score.PlayerName] = score;
                }
            }

            var orderedScores = _playerScores.Select(x => x.Value).OrderBy(x => x.TotalScore).ToList();
            
            foreach(var score in orderedScores)
            {
                lbScores.Items.Insert(0, score.PlayerScoreString());
            }
        }

        private void btnCopyScores_Click(object sender, EventArgs e)
        {
            if (_playerScores == null || _playerScores.Count == 0) return;

            var orderedScores = _playerScores.Select(x => x.Value).OrderByDescending(x => x.TotalScore).ToList().Select(x => x.PlayerScoreString());
            var output = string.Join("\n", orderedScores);

            Clipboard.SetText(output);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
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
