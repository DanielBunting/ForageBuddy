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
        private readonly ILogger _logger;
        private readonly IForageCalculator _forageCalculator;

        public Form1(IForageCalculator forageCalculator, ILogger logger)
        {
            _forageCalculator = forageCalculator;
            _logger = logger;
            
            InitializeComponent();
            
            _logger.Information("Application started");
            lbScores.Items.Insert(0, "Select a screenshot folder");
        }

        private void btnFolderSelect_Click(object sender, EventArgs e)
        {
            _logger.Information("Selecting output folder...");
            
            if (fbdScreenieFolder.ShowDialog() != DialogResult.OK)
            {
                _logger.Information("No folder selected");
                return;
            }
            
            _forageCalculator.SetDirectory(fbdScreenieFolder.SelectedPath);

            lbScores.Items.Clear();
            lbScores.Items.Insert(0, "Ready");
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            var scores = _forageCalculator.CalculateScores();
            _forageCalculator.ResetCalculator();
            
            lbScores.Items.Clear();
            
            foreach(var score in scores)
                lbScores.Items.Insert(0, score);
        }

        private void btnCopyScores_Click(object sender, EventArgs e)
        {
            var textValue = _forageCalculator.GetScoreString();
            if (!string.IsNullOrEmpty(textValue))
            {
                Clipboard.SetText(textValue);
                _logger.Information($"Scores copied to clipboard: {textValue}");
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            lbScores.Items.Clear();
            _forageCalculator.ResetCalculator();
        }

        private void btnCcCounts_Click(object sender, EventArgs e)
        {
            var textValue = _forageCalculator.GetCursedChestScoreString();
            if (!string.IsNullOrEmpty(textValue))
            {
                Clipboard.SetText(textValue);
                _logger.Information($"Scores copied to clipboard: {textValue}");
            }
        }
    }
}
