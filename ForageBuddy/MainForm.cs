using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForageBuddy
{
    public partial class MainForm : Form
    {
        public MainForm(ILogger<MainForm> logger)
        {
            InitializeComponent();
            _logger = logger;
        }

        private ILogger<MainForm> _logger;

        private void button1_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Test");
        }
    }
}
