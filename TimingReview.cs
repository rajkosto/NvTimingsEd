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
    public partial class TimingReview : Form
    {
        public TimingReview(string monitorName, ICollection<ResourceTiming> existingTimings, ICollection<ResourceTiming> newTimings)
        {
            InitializeComponent();
            if (monitorName != null)
                this.Text = String.Format("Monitor timings for " + monitorName);

            var type = typeof(ResourceTiming);
            var fieldInfos = type.GetFields();

            DataGridView[] views = { dataGridExisting, dataGridNew };
            foreach (var currView in views)
            {
                DataTable table = new DataTable(type.Name);
                foreach (var fld in fieldInfos)
                {
                    var column = new DataColumn(fld.Name, fld.FieldType);
                    var dispAttr = (DisplayAttribute)fld.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
                    if (dispAttr != null)
                    {
                        if (!dispAttr.Visible)
                            continue;
                        if (dispAttr.Caption != null)
                            column.Caption = dispAttr.Caption;
                        else
                            column.Caption = fld.Name;

                        if (dispAttr.CustomType != null)
                            column.DataType = dispAttr.CustomType;
                    }
                    else
                        column.Caption = fld.Name;

                    column.ReadOnly = false;
                    column.AutoIncrement = false;
                    column.Unique = false;
                    table.Columns.Add(column);
                }

                ICollection<ResourceTiming> currTimings = null;
                if (currView == dataGridExisting)
                    currTimings = existingTimings;
                else if (currView == dataGridNew)
                    currTimings = newTimings;

                if (currTimings != null)
                {
                    foreach (var theTiming in currTimings)
                    {
                        if (theTiming.IsEmpty())
                            continue;

                        var row = table.NewRow();
                        foreach (DataColumn col in table.Columns)
                        {
                            var p = type.GetField(col.ColumnName);
                            if (p != null)
                                row[col.ColumnName] = Convert.ChangeType(p.GetValue(theTiming), col.DataType);
                        }
                        table.Rows.Add(row);
                    }
                }

                currView.DataSource = table;
                foreach (DataGridViewColumn col in currView.Columns)
                    col.HeaderText = table.Columns[col.HeaderText].Caption;

                currView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            }            
        }

        private void TimingReview_Shown(object sender, EventArgs e)
        {
            DataGridView[] views = { dataGridExisting, dataGridNew };
            foreach (var currView in views)
            {
                currView.AutoResizeColumns(currView.AutoSizeColumnsMode);
                currView.Height = currView.ColumnHeadersHeight + currView.RowTemplate.Height * (6 + 1);
                Size newSize = currView.GetPreferredSize(new Size(0, 0));
                newSize.Width += currView.Location.X;
                newSize.Height += currView.Location.Y + flowBtns.Height;
                if (currView != dataGridExisting)
                {
                    if (newSize.Height < this.ClientSize.Height)
                        newSize.Height = this.ClientSize.Height;
                }
                this.ClientSize = newSize;
            }
        }

        public ResourceTiming[] GetNewTimings()
        {
            var table = (DataTable)dataGridNew.DataSource;
            var cols = table.Columns;
            var rows = table.Rows;
            var dst = new ResourceTiming[rows.Count];
            var type = dst.GetType().GetElementType();
            for (int i=0; i<dst.Length; i++)
            {
                var row = rows[i];
                object temp = dst[i];
                foreach (DataColumn col in table.Columns)
                {
                    var p = type.GetField(col.ColumnName);
                    if (p != null)
                        p.SetValue(temp, Convert.ChangeType(row[col.ColumnName], p.FieldType));
                }
                dst[i] = (ResourceTiming)temp;
            }
            return dst;
        }

        private void btnDialog_Click(object sender, EventArgs e)
        {
            if (sender == btnOK)
                this.DialogResult = DialogResult.OK;
            else if (sender == btnCancel)
                this.DialogResult = DialogResult.Cancel;
            else
                return;

            this.Close();
        }
    }
}
