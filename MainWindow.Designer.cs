namespace NvTimingsEd
{
    partial class MainWindow
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToNvtimingsiniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.comboMonitors = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRefreshMonitors = new System.Windows.Forms.Button();
            this.btnNewMonitor = new System.Windows.Forms.Button();
            this.btnCopyMonitor = new System.Windows.Forms.Button();
            this.btnDeleteMonitor = new System.Windows.Forms.Button();
            this.listMonitorTimings = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnRefreshTimings = new System.Windows.Forms.Button();
            this.btnNewTiming = new System.Windows.Forms.Button();
            this.btnEditTiming = new System.Windows.Forms.Button();
            this.btnCopyTiming = new System.Windows.Forms.Button();
            this.btnDeleteTiming = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(344, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToNvtimingsiniToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exportToNvtimingsiniToolStripMenuItem
            // 
            this.exportToNvtimingsiniToolStripMenuItem.Name = "exportToNvtimingsiniToolStripMenuItem";
            this.exportToNvtimingsiniToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exportToNvtimingsiniToolStripMenuItem.Text = "&Export to nvtimings.ini";
            this.exportToNvtimingsiniToolStripMenuItem.Click += new System.EventHandler(this.exportToNvtimingsiniToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.comboMonitors, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.listMonitorTimings, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(344, 157);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // comboMonitors
            // 
            this.comboMonitors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboMonitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMonitors.FormattingEnabled = true;
            this.comboMonitors.Location = new System.Drawing.Point(3, 7);
            this.comboMonitors.Name = "comboMonitors";
            this.comboMonitors.Size = new System.Drawing.Size(106, 21);
            this.comboMonitors.TabIndex = 1;
            this.comboMonitors.SelectedIndexChanged += new System.EventHandler(this.comboMonitors_SelectedIndexChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnRefreshMonitors);
            this.flowLayoutPanel1.Controls.Add(this.btnNewMonitor);
            this.flowLayoutPanel1.Controls.Add(this.btnCopyMonitor);
            this.flowLayoutPanel1.Controls.Add(this.btnDeleteMonitor);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(115, 3);
            this.flowLayoutPanel1.MinimumSize = new System.Drawing.Size(226, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(226, 29);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // btnRefreshMonitors
            // 
            this.btnRefreshMonitors.Location = new System.Drawing.Point(3, 3);
            this.btnRefreshMonitors.Name = "btnRefreshMonitors";
            this.btnRefreshMonitors.Size = new System.Drawing.Size(57, 23);
            this.btnRefreshMonitors.TabIndex = 7;
            this.btnRefreshMonitors.Text = "Refresh";
            this.btnRefreshMonitors.UseVisualStyleBackColor = true;
            this.btnRefreshMonitors.Click += new System.EventHandler(this.btnRefreshMonitors_Click);
            // 
            // btnNewMonitor
            // 
            this.btnNewMonitor.Location = new System.Drawing.Point(66, 3);
            this.btnNewMonitor.Name = "btnNewMonitor";
            this.btnNewMonitor.Size = new System.Drawing.Size(48, 23);
            this.btnNewMonitor.TabIndex = 5;
            this.btnNewMonitor.Text = "New";
            this.btnNewMonitor.UseVisualStyleBackColor = true;
            this.btnNewMonitor.Click += new System.EventHandler(this.btnNewOrCopyMonitor_Click);
            // 
            // btnCopyMonitor
            // 
            this.btnCopyMonitor.Location = new System.Drawing.Point(120, 3);
            this.btnCopyMonitor.Name = "btnCopyMonitor";
            this.btnCopyMonitor.Size = new System.Drawing.Size(48, 23);
            this.btnCopyMonitor.TabIndex = 6;
            this.btnCopyMonitor.Text = "Copy";
            this.btnCopyMonitor.UseVisualStyleBackColor = true;
            this.btnCopyMonitor.Click += new System.EventHandler(this.btnNewOrCopyMonitor_Click);
            // 
            // btnDeleteMonitor
            // 
            this.btnDeleteMonitor.Location = new System.Drawing.Point(174, 3);
            this.btnDeleteMonitor.Name = "btnDeleteMonitor";
            this.btnDeleteMonitor.Size = new System.Drawing.Size(48, 23);
            this.btnDeleteMonitor.TabIndex = 8;
            this.btnDeleteMonitor.Text = "Delete";
            this.btnDeleteMonitor.UseVisualStyleBackColor = true;
            this.btnDeleteMonitor.Click += new System.EventHandler(this.btnDeleteMonitor_Click);
            // 
            // listMonitorTimings
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.listMonitorTimings, 2);
            this.listMonitorTimings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listMonitorTimings.FormattingEnabled = true;
            this.listMonitorTimings.Location = new System.Drawing.Point(3, 38);
            this.listMonitorTimings.Name = "listMonitorTimings";
            this.listMonitorTimings.Size = new System.Drawing.Size(338, 87);
            this.listMonitorTimings.TabIndex = 9;
            this.listMonitorTimings.SelectedIndexChanged += new System.EventHandler(this.listMonitorTimings_SelectedIndexChanged);
            this.listMonitorTimings.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listMonitorTimings_MouseDoubleClick);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.flowLayoutPanel2, 2);
            this.flowLayoutPanel2.Controls.Add(this.btnRefreshTimings);
            this.flowLayoutPanel2.Controls.Add(this.btnNewTiming);
            this.flowLayoutPanel2.Controls.Add(this.btnEditTiming);
            this.flowLayoutPanel2.Controls.Add(this.btnCopyTiming);
            this.flowLayoutPanel2.Controls.Add(this.btnDeleteTiming);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 128);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(338, 29);
            this.flowLayoutPanel2.TabIndex = 10;
            // 
            // btnRefreshTimings
            // 
            this.btnRefreshTimings.Location = new System.Drawing.Point(3, 3);
            this.btnRefreshTimings.Name = "btnRefreshTimings";
            this.btnRefreshTimings.Size = new System.Drawing.Size(57, 23);
            this.btnRefreshTimings.TabIndex = 11;
            this.btnRefreshTimings.Text = "Refresh";
            this.btnRefreshTimings.UseVisualStyleBackColor = true;
            this.btnRefreshTimings.Click += new System.EventHandler(this.btnRefreshTimings_Click);
            // 
            // btnNewTiming
            // 
            this.btnNewTiming.Location = new System.Drawing.Point(66, 3);
            this.btnNewTiming.Name = "btnNewTiming";
            this.btnNewTiming.Size = new System.Drawing.Size(48, 23);
            this.btnNewTiming.TabIndex = 9;
            this.btnNewTiming.Text = "New";
            this.btnNewTiming.UseVisualStyleBackColor = true;
            this.btnNewTiming.Click += new System.EventHandler(this.btnNewEditCopyTiming_Click);
            // 
            // btnEditTiming
            // 
            this.btnEditTiming.Location = new System.Drawing.Point(120, 3);
            this.btnEditTiming.Name = "btnEditTiming";
            this.btnEditTiming.Size = new System.Drawing.Size(48, 23);
            this.btnEditTiming.TabIndex = 13;
            this.btnEditTiming.Text = "Edit";
            this.btnEditTiming.UseVisualStyleBackColor = true;
            this.btnEditTiming.Click += new System.EventHandler(this.btnNewEditCopyTiming_Click);
            // 
            // btnCopyTiming
            // 
            this.btnCopyTiming.Location = new System.Drawing.Point(174, 3);
            this.btnCopyTiming.Name = "btnCopyTiming";
            this.btnCopyTiming.Size = new System.Drawing.Size(48, 23);
            this.btnCopyTiming.TabIndex = 10;
            this.btnCopyTiming.Text = "Copy";
            this.btnCopyTiming.UseVisualStyleBackColor = true;
            this.btnCopyTiming.Click += new System.EventHandler(this.btnNewEditCopyTiming_Click);
            // 
            // btnDeleteTiming
            // 
            this.btnDeleteTiming.Location = new System.Drawing.Point(228, 3);
            this.btnDeleteTiming.Name = "btnDeleteTiming";
            this.btnDeleteTiming.Size = new System.Drawing.Size(48, 23);
            this.btnDeleteTiming.TabIndex = 12;
            this.btnDeleteTiming.Text = "Delete";
            this.btnDeleteTiming.UseVisualStyleBackColor = true;
            this.btnDeleteTiming.Click += new System.EventHandler(this.btnDeleteTiming_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 181);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(360, 220);
            this.Name = "MainWindow";
            this.Text = "NvTimingsEd";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToNvtimingsiniToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnRefreshMonitors;
        private System.Windows.Forms.Button btnNewMonitor;
        private System.Windows.Forms.Button btnCopyMonitor;
        private System.Windows.Forms.Button btnDeleteMonitor;
        private System.Windows.Forms.ListBox listMonitorTimings;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnRefreshTimings;
        private System.Windows.Forms.Button btnNewTiming;
        private System.Windows.Forms.Button btnCopyTiming;
        private System.Windows.Forms.Button btnDeleteTiming;
        private System.Windows.Forms.ComboBox comboMonitors;
        private System.Windows.Forms.Button btnEditTiming;
    }
}