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
        private ILogger<MainForm> _logger;
        private readonly IForageCalculator _forageCalculator;

        public MainForm(ILogger<MainForm> logger, IForageCalculator forageCalculator)
        {
            InitializeComponent();
            _logger = logger;
            _forageCalculator = forageCalculator;
        }

        private void Run_Click(object sender, EventArgs e)
        {

        }
    }
}
