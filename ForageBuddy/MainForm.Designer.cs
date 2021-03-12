
namespace ForageBuddy
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ScoresOutput = new System.Windows.Forms.ListBox();
            this.Run = new System.Windows.Forms.Button();
            this.ClientPicker = new System.Windows.Forms.ComboBox();
            this.RefreshClients = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ScoresOutput
            // 
            this.ScoresOutput.FormattingEnabled = true;
            this.ScoresOutput.ItemHeight = 15;
            this.ScoresOutput.Location = new System.Drawing.Point(12, 12);
            this.ScoresOutput.Name = "ScoresOutput";
            this.ScoresOutput.Size = new System.Drawing.Size(149, 169);
            this.ScoresOutput.TabIndex = 0;
            // 
            // Run
            // 
            this.Run.Location = new System.Drawing.Point(167, 119);
            this.Run.Name = "Run";
            this.Run.Size = new System.Drawing.Size(153, 62);
            this.Run.TabIndex = 1;
            this.Run.Text = "Run";
            this.Run.UseVisualStyleBackColor = true;
            this.Run.Click += new System.EventHandler(this.Run_Click);
            // 
            // ClientPicker
            // 
            this.ClientPicker.FormattingEnabled = true;
            this.ClientPicker.Location = new System.Drawing.Point(167, 12);
            this.ClientPicker.Name = "ClientPicker";
            this.ClientPicker.Size = new System.Drawing.Size(153, 23);
            this.ClientPicker.TabIndex = 2;
            this.ClientPicker.SelectedIndexChanged += new System.EventHandler(this.ClientPicker_SelectedIndexChanged);
            // 
            // RefreshClients
            // 
            this.RefreshClients.Location = new System.Drawing.Point(167, 41);
            this.RefreshClients.Name = "RefreshClients";
            this.RefreshClients.Size = new System.Drawing.Size(153, 26);
            this.RefreshClients.TabIndex = 3;
            this.RefreshClients.Text = "Refresh Client List";
            this.RefreshClients.UseVisualStyleBackColor = true;
            this.RefreshClients.Click += new System.EventHandler(this.RefreshClients_Click);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(327, 195);
            this.Controls.Add(this.RefreshClients);
            this.Controls.Add(this.ClientPicker);
            this.Controls.Add(this.Run);
            this.Controls.Add(this.ScoresOutput);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Forage Buddy";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ScoresOutput;
        private System.Windows.Forms.Button Run;
        private System.Windows.Forms.ComboBox ClientPicker;
        private System.Windows.Forms.Button RefreshClients;
    }
}

