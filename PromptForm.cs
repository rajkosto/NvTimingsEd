using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NvTimingsEd
{
    public partial class PromptForm : Form
    {
        public PromptForm()
        {
            InitializeComponent();
        }

        public PromptForm(string title, string label)
        {
            InitializeComponent();
            Text = title;
            Label = label;
        }

        public string Label
        {
            get
            {
                return lblText.Text;
            }
            set
            {
                lblText.Text = value;
            }
        }

        public string Value
        {
            get
            {
                return textValue.Text;
            }
            set
            {
                textValue.Text = value;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void textValue_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = textValue.Text.Trim().Length > 0;
        }
    }
}
