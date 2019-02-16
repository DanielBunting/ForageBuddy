using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        }

        private void btnFolderSelect_Click(object sender, EventArgs e)
        {
            // TODO: Gracefully show if the client is connected properly.

            if (fbdScreenieFolder.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(fbdScreenieFolder.SelectedPath);
                _folderPath = fbdScreenieFolder.SelectedPath;
                _filesInScreenshotDirectory = GetPngFiles();
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            lbScores.Items.Clear();
            _playerScores = new Dictionary<string, PlayerScore>();

            if (_folderPath == null) return;

            var allFilesInOutputDir = GetPngFiles();

            var tmpPlayerScores = new List<PlayerScore>();

            foreach (var file in allFilesInOutputDir)
            {
                if (_filesInScreenshotDirectory.Contains(file))
                {
                    Console.WriteLine($"File {file} already found. Continuing.");
                    continue;
                }

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

            foreach (var score in _playerScores)
            {
                lbScores.Items.Insert(0, score.Value.PlayerScoreString());
            }
        }

        private void btnCopyScores_Click(object sender, EventArgs e)
        {
            if (_playerScores == null) return;

            var output = string.Join("\n", _playerScores.Values.Select(x => x.PlayerScoreString()));

            Clipboard.SetText(output);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (_folderPath == null) return;

            _playerScores = null;
            lbScores.Items.Clear();
            _filesInScreenshotDirectory = GetPngFiles();

            // Resets the scores since the last forage. 
        }

        private List<string> GetPngFiles()
        => Directory.EnumerateFiles(_folderPath)
            .ToList()
            .FindAll(x => x.Split('.').LastOrDefault() == "png");
    }
}
