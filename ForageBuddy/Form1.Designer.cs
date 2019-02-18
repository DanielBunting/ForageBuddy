namespace ForageBuddy
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fbdScreenieFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnFolderSelect = new System.Windows.Forms.Button();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.lbScores = new System.Windows.Forms.ListBox();
            this.btnCopyScores = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFolderSelect
            // 
            this.btnFolderSelect.Location = new System.Drawing.Point(12, 12);
            this.btnFolderSelect.Name = "btnFolderSelect";
            this.btnFolderSelect.Size = new System.Drawing.Size(75, 42);
            this.btnFolderSelect.TabIndex = 0;
            this.btnFolderSelect.Text = "Output Folder";
            this.btnFolderSelect.UseVisualStyleBackColor = true;
            this.btnFolderSelect.Click += new System.EventHandler(this.btnFolderSelect_Click);
            // 
            // btnCalculate
            // 
            this.btnCalculate.Location = new System.Drawing.Point(12, 60);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 42);
            this.btnCalculate.TabIndex = 1;
            this.btnCalculate.Text = "Calculate scores";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // lbScores
            // 
            this.lbScores.FormattingEnabled = true;
            this.lbScores.Location = new System.Drawing.Point(93, 12);
            this.lbScores.Name = "lbScores";
            this.lbScores.Size = new System.Drawing.Size(126, 186);
            this.lbScores.TabIndex = 2;
            // 
            // btnCopyScores
            // 
            this.btnCopyScores.Location = new System.Drawing.Point(12, 156);
            this.btnCopyScores.Name = "btnCopyScores";
            this.btnCopyScores.Size = new System.Drawing.Size(75, 42);
            this.btnCopyScores.TabIndex = 3;
            this.btnCopyScores.Text = "Copy Scores";
            this.btnCopyScores.UseVisualStyleBackColor = true;
            this.btnCopyScores.Click += new System.EventHandler(this.btnCopyScores_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(12, 108);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 42);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Clear Scores";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 210);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnCopyScores);
            this.Controls.Add(this.lbScores);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.btnFolderSelect);
            this.Name = "Form1";
            this.Text = "Forage Buddy";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog fbdScreenieFolder;
        private System.Windows.Forms.Button btnFolderSelect;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.ListBox lbScores;
        private System.Windows.Forms.Button btnCopyScores;
        private System.Windows.Forms.Button btnReset;
    }
}

