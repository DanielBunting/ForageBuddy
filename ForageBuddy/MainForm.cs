using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace ForageBuddy
{
    public partial class MainForm : Form
    {
        private ILogger<MainForm> _logger;
        private readonly IForageCalculator _forageCalculator;

        private IntPtr _selectedClient;

        private List<(string clientName, IntPtr clientHandle)> _clients;

        public MainForm(ILogger<MainForm> logger, IForageCalculator forageCalculator)
        {
            InitializeComponent();
            _logger = logger;
            _forageCalculator = forageCalculator;
        }

        private void Run_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Running");
            var scores = _forageCalculator.CalculateScores(_selectedClient);

            Clipboard.SetText(string.Join("\n", scores));
        }

        private void RefreshClients_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Refreshing the linked clients");
            PopulateClientPicker();
        }

        private void PopulateClientPicker()
        {
            _clients = WindowsInterface
                .GetOpenPuzzlePiratesClients()
                .ToList();

            _logger.LogInformation($"Found {_clients.Count} client intances");

            ClientPicker.Items.Clear();
            foreach (var client in _clients)
                ClientPicker.Items.AddRange(_clients.Select(x => x.clientName).ToArray());
        }

        private void ClientPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = ClientPicker.SelectedItem as string;

            _logger.LogInformation($"Client selected: {selectedItem}");
            _selectedClient = _clients.Find(x => x.clientName == selectedItem).clientHandle;
        }
    }
}
