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
        private string folderPath;
        private List<string> filesInScreenshotDirectory;
        private Dictionary<string, PlayerScore> PlayerScores;

        private ImageParser imageParser;

        public Form1()
        {
            imageParser = new ImageParser();
            InitializeComponent();

        }

        private void btnFolderSelect_Click(object sender, EventArgs e)
        {
            if (fbdScreenieFolder.ShowDialog() == DialogResult.OK)
            {
                Console.WriteLine(fbdScreenieFolder.SelectedPath);
                folderPath = fbdScreenieFolder.SelectedPath;
                filesInScreenshotDirectory = GetPngFiles();
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            lbScores.Items.Clear();
            PlayerScores = new Dictionary<string, PlayerScore>();

            if (folderPath == null) return;

            var allFilesInOutputDir = GetPngFiles();

            var tmpPlayerScores = new List<PlayerScore>();

            foreach (var file in allFilesInOutputDir)
            {
                if (filesInScreenshotDirectory.Contains(file))
                {
                    Console.WriteLine($"File {file} already found. Continuing.");
                    continue;
                }

                tmpPlayerScores.AddRange(imageParser.GetPlayerScoresInImage(file));
            }

            foreach (var score in tmpPlayerScores)
            {
                if (!PlayerScores.ContainsKey(score.GetPlayerName())) PlayerScores.Add(score.GetPlayerName(), score);
                if (score.GetTotalScore() > PlayerScores[score.GetPlayerName()].GetTotalScore())
                {
                    PlayerScores[score.GetPlayerName()] = score;
                }
            }

            foreach (var score in PlayerScores)
            {
                lbScores.Items.Insert(0, score.Value.PlayerScoreString());
            }
        }

        private void btnCopyScores_Click(object sender, EventArgs e)
        {
            if (PlayerScores == null) return;

            var output = string.Join("\n", PlayerScores.Values.Select(x => x.PlayerScoreString()));

            Clipboard.SetText(output);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (folderPath == null) return;

            PlayerScores = null;
            lbScores.Items.Clear();
            filesInScreenshotDirectory = GetPngFiles();

            // Resets the scores since the last forage. 
        }

        private List<string> GetPngFiles()
        => Directory.EnumerateFiles(folderPath)
            .ToList()
            .FindAll(x => x.Split('.').LastOrDefault() == "png");
    }
}
