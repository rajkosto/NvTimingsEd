namespace NvTimingsEd
{
    partial class TimingReview
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
            this.dataGridExisting = new System.Windows.Forms.DataGridView();
            this.dataGridNew = new System.Windows.Forms.DataGridView();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.flowBtns = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabNew = new System.Windows.Forms.TabPage();
            this.tabExisting = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridExisting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridNew)).BeginInit();
            this.flowBtns.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabNew.SuspendLayout();
            this.tabExisting.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridExisting
            // 
            this.dataGridExisting.AllowUserToAddRows = false;
            this.dataGridExisting.AllowUserToDeleteRows = false;
            this.dataGridExisting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridExisting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridExisting.Location = new System.Drawing.Point(3, 3);
            this.dataGridExisting.MultiSelect = false;
            this.dataGridExisting.Name = "dataGridExisting";
            this.dataGridExisting.ReadOnly = true;
            this.dataGridExisting.ShowEditingIcon = false;
            this.dataGridExisting.Size = new System.Drawing.Size(506, 172);
            this.dataGridExisting.TabIndex = 2;
            // 
            // dataGridNew
            // 
            this.dataGridNew.AllowUserToAddRows = false;
            this.dataGridNew.AllowUserToDeleteRows = false;
            this.dataGridNew.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridNew.Location = new System.Drawing.Point(3, 3);
            this.dataGridNew.MultiSelect = false;
            this.dataGridNew.Name = "dataGridNew";
            this.dataGridNew.ShowEditingIcon = false;
            this.dataGridNew.Size = new System.Drawing.Size(506, 172);
            this.dataGridNew.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(442, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnDialog_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(361, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnDialog_Click);
            // 
            // flowBtns
            // 
            this.flowBtns.Controls.Add(this.btnCancel);
            this.flowBtns.Controls.Add(this.btnOK);
            this.flowBtns.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowBtns.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowBtns.Location = new System.Drawing.Point(0, 204);
            this.flowBtns.Name = "flowBtns";
            this.flowBtns.Size = new System.Drawing.Size(520, 29);
            this.flowBtns.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabNew);
            this.tabControl1.Controls.Add(this.tabExisting);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(520, 204);
            this.tabControl1.TabIndex = 0;
            // 
            // tabNew
            // 
            this.tabNew.Controls.Add(this.dataGridNew);
            this.tabNew.Location = new System.Drawing.Point(4, 22);
            this.tabNew.Name = "tabNew";
            this.tabNew.Padding = new System.Windows.Forms.Padding(3);
            this.tabNew.Size = new System.Drawing.Size(512, 178);
            this.tabNew.TabIndex = 0;
            this.tabNew.Text = "New";
            this.tabNew.UseVisualStyleBackColor = true;
            // 
            // tabExisting
            // 
            this.tabExisting.Controls.Add(this.dataGridExisting);
            this.tabExisting.Location = new System.Drawing.Point(4, 22);
            this.tabExisting.Name = "tabExisting";
            this.tabExisting.Padding = new System.Windows.Forms.Padding(3);
            this.tabExisting.Size = new System.Drawing.Size(512, 178);
            this.tabExisting.TabIndex = 1;
            this.tabExisting.Text = "Existing";
            this.tabExisting.UseVisualStyleBackColor = true;
            // 
            // TimingReview
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(520, 233);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.flowBtns);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimingReview";
            this.ShowIcon = false;
            this.Text = "TimingReview";
            this.Shown += new System.EventHandler(this.TimingReview_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridExisting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridNew)).EndInit();
            this.flowBtns.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabNew.ResumeLayout(false);
            this.tabExisting.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridExisting;
        private System.Windows.Forms.DataGridView dataGridNew;
        private System.Windows.Forms.FlowLayoutPanel flowBtns;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabNew;
        private System.Windows.Forms.TabPage tabExisting;
    }
}