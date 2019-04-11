using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NvTimingsEd
{
    public partial class MainWindow : Form
    {
        private RegistryKey parametersKey;
        private SharedDisposable<RegistryKey> monitorSubKey;

        public MainWindow(RegistryKey parametersKey)
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.parametersKey = parametersKey;
            btnRefreshMonitors_Click(btnRefreshMonitors, null);
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (monitorSubKey != null)
            {
                monitorSubKey.Dispose();
                monitorSubKey = null;
            }
            if (parametersKey != null)
            {
                parametersKey.Dispose();
                parametersKey = null;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var about = new AboutDialog())
            {
                about.ShowDialog();
            }
        }

        private void btnRefreshMonitors_Click(object sender, EventArgs e)
        {
            var oldSelection = (string)comboMonitors.SelectedItem;
            string[] newDataSource = null;
            try { newDataSource = parametersKey.GetSubKeyNames(); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error listing monitors from registry key!"); }

            try
            {
                suppressComboSelectedIndexChanged = true;
                comboMonitors.DataSource = newDataSource;
            }
            finally { suppressComboSelectedIndexChanged = false; }
            
            if (newDataSource != null && oldSelection != null)
                comboMonitors.SelectedIndex = Array.FindIndex(newDataSource, s => s.Equals(oldSelection, StringComparison.OrdinalIgnoreCase));
            else
                comboMonitors.SelectedIndex = -1;
        }

        private bool suppressComboSelectedIndexChanged = false;
        private void comboMonitors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suppressComboSelectedIndexChanged)
                return;

            var currMonitorName = (string)comboMonitors.SelectedItem;
            if (currMonitorName == null)
            {
                listMonitorTimings.SelectedIndex = -1;
                listMonitorTimings.DataSource = null;
                btnCopyMonitor.Enabled = false;
                btnDeleteMonitor.Enabled = false;
                listMonitorTimings_SelectedIndexChanged(listMonitorTimings, null);
                return;
            }
            else
            {
                btnCopyMonitor.Enabled = true;
                btnDeleteMonitor.Enabled = true;
            }

            try
            {
                if (monitorSubKey != null)
                    monitorSubKey.Dispose();
            }
            catch (Exception) { }
            finally { monitorSubKey = null; }

            try
            {
                monitorSubKey = new SharedDisposable<RegistryKey>( ()=>parametersKey.OpenSubKey(currMonitorName, true) );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Error opening registry subkey for monitor {0}", currMonitorName));
                return;
            }

            btnRefreshTimings_Click(null, null);
        }

        private void btnRefreshTimings_Click(object sender, EventArgs e)
        {
            string oldSelection = null;
            var oldDataSource = (ICollection<EncodedMonitorTimings>)(listMonitorTimings.DataSource);
            if (oldDataSource != null && listMonitorTimings.SelectedIndex >= 0 && listMonitorTimings.SelectedIndex < oldDataSource.Count)
                oldSelection = oldDataSource.ElementAt(listMonitorTimings.SelectedIndex).encodedName;

            EncodedMonitorTimings[] newDataSource = null;
            if (monitorSubKey != null)
                newDataSource = Array.ConvertAll(monitorSubKey.Value.GetValueNames(), s => new EncodedMonitorTimings(s));

            listMonitorTimings.DataSource = newDataSource;
            if (newDataSource != null && oldSelection != null)
                listMonitorTimings.SelectedIndex = Array.FindIndex(newDataSource, s => s.encodedName.Equals(oldSelection, StringComparison.OrdinalIgnoreCase));
            else
                listMonitorTimings.SelectedIndex = -1;

            if (listMonitorTimings.SelectedIndex < 0)
                listMonitorTimings_SelectedIndexChanged(null, null);
        }

        private void listMonitorTimings_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currDataSource = (ICollection<EncodedMonitorTimings>)(listMonitorTimings.DataSource);
            if (currDataSource != null)
            {
                int currIndex = listMonitorTimings.SelectedIndex;
                if (currIndex >= 0 && currIndex < currDataSource.Count)
                {
                    EncodedMonitorTimings currTiming = currDataSource.ElementAt(currIndex);
                    if (currTiming.ToString().StartsWith("ERROR"))
                    {
                        btnEditTiming.Enabled = false;
                        btnCopyTiming.Enabled = false;
                    }
                    else
                    {
                        btnEditTiming.Enabled = true;
                        btnCopyTiming.Enabled = true;
                    }                    
                    btnDeleteTiming.Enabled = true;
                    return;
                }
            }
            btnEditTiming.Enabled = false;
            btnCopyTiming.Enabled = false;
            btnDeleteTiming.Enabled = false;
        }

        private bool CheckMonitorKeyExists(string monitorName)
        {
            bool retVal = false;

            RegistryKey existingKey = null;
            try
            {
                existingKey = parametersKey.OpenSubKey(monitorName, false);
            }
            catch (Exception) {}
            
            if (existingKey != null)
            {
                existingKey.Dispose();
                existingKey = null;
                retVal = true;
            }

            return retVal;
        }

        private void btnNewOrCopyMonitor_Click(object sender, EventArgs e)
        {
            var srcMonitorName = (string)comboMonitors.SelectedItem;
            if (sender != btnCopyMonitor)
                srcMonitorName = null;

            using (var prompt = new PromptForm((srcMonitorName != null) ? ("Copy " + srcMonitorName + " as") : "Create new monitor", "Name:"))
            {
                while (true)
                {
                    if (prompt.ShowDialog() != DialogResult.OK)
                        return;

                    var newKeyName = prompt.Value.Trim();
                    prompt.Value = newKeyName;

                    if (CheckMonitorKeyExists(newKeyName))
                    {
                        MessageBox.Show("Monitor " + newKeyName + " already exists! Try another.", 
                                        (srcMonitorName == null) ? "Can't copy monitor" : "Can't create new monitor");
                        continue;
                    }

                    RegistryKey newMonitorKey = null;
                    try
                    {
                        newMonitorKey = parametersKey.CreateSubKey(newKeyName, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error creating new registry key");
                        continue;
                    }
                    if (newMonitorKey == null)
                    {
                        MessageBox.Show("Cannot create new registry key for monitor " + newKeyName, "Error creating new registry key");
                        continue;
                    }
                    else
                    {
                        if (srcMonitorName != null)
                            monitorSubKey.Value.CopyTo(newMonitorKey);

                        newMonitorKey.Dispose();
                        newMonitorKey = null;
                    }

                    try
                    {
                        suppressComboSelectedIndexChanged = true;
                        comboMonitors.DataSource = new string[1] { newKeyName };
                        comboMonitors.SelectedItem = newKeyName;
                    }
                    finally { suppressComboSelectedIndexChanged = false; }
                    btnRefreshMonitors_Click(null, null);
                    break;
                }
            }
        }

        private void btnDeleteMonitor_Click(object sender, EventArgs e)
        {
            var currMonitorName = (string)comboMonitors.SelectedItem;
            if (currMonitorName == null)
                return;

            if (MessageBox.Show("Delete monitor " + currMonitorName + " ?", "Confirm monitor deletion", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            if (monitorSubKey != null)
            {
                monitorSubKey.Dispose();
                monitorSubKey = null;
            }

            try
            {
                parametersKey.DeleteSubKeyTree(currMonitorName);
                comboMonitors.SelectedIndex--;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error deleting registry key");
            }

            btnRefreshMonitors_Click(null, null);
        }

        private void btnDeleteTiming_Click(object sender, EventArgs e)
        {
            if (monitorSubKey == null || listMonitorTimings.SelectedItem == null)
                return;

            var currTiming = (EncodedMonitorTimings)listMonitorTimings.SelectedItem;
            if (MessageBox.Show("Delete timing '" + currTiming.ToString() + "' ?", "Confirm timing deletion", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                monitorSubKey.Value.DeleteValue(currTiming.encodedName);
                listMonitorTimings.SelectedIndex--;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error deleting timing value");
            }

            btnRefreshTimings_Click(null, null);
        }

        private void exportToNvtimingsiniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (parametersKey == null)
                return;

            string theFileName = null;
            using (var dlg = new SaveFileDialog())
            {
                dlg.FileName = "nvtimings";
                dlg.DefaultExt = ".ini";
                dlg.Filter = "Ini files (.ini)|*.ini";
                if (dlg.ShowDialog() == DialogResult.OK)
                    theFileName = dlg.FileName;
                else
                    return;
            }

            try
            {
                using (var writer = File.CreateText(theFileName))
                {
                    writer.WriteLine("[Version]");
                    writer.WriteLine("Signature=\"$Windows NT$\"");
                    writer.WriteLine();
                    writer.WriteLine("[NvStUSB.Param]");
                    foreach (var monitorName in parametersKey.GetSubKeyNames())
                    {
                        using (var currSubKey = parametersKey.OpenSubKey(monitorName, false))
                        {
                            var currKeyName = currSubKey.Name.Split('\\');
                            if (currKeyName[0].Equals("HKEY_LOCAL_MACHINE", StringComparison.OrdinalIgnoreCase))
                                currKeyName[0] = "HKLM";
                            if (currKeyName[1].Equals("System", StringComparison.OrdinalIgnoreCase))
                                currKeyName[1] = "System";

                            var outKeyName = currKeyName[0] + "," + String.Join("\\", currKeyName, 1, currKeyName.Length - 1) + ",";                            
                            foreach (var valueName in currSubKey.GetValueNames())
                            {
                                var val = (byte[])currSubKey.GetValue(valueName, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
                                if (val == null)
                                    continue;

                                writer.WriteLine(outKeyName + valueName + ",1," + "0x" + BitConverter.ToString(val).Replace("-", ",0x").ToLowerInvariant());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error writing to nvtimings.ini");
            }
        }

        private void SpawnTimingsEditor(string existingTiming, bool isCopy)
        {
            var subKeyCopy = monitorSubKey.Share();
            byte[] existingGlasses = null;
            if (existingTiming != null)
            {
                try { existingGlasses = (byte[])subKeyCopy.Value.GetValue(existingTiming); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Error getting glasses timings"); }
            }
            new TimingsEditor(this, subKeyCopy, isCopy, existingTiming, existingGlasses).Show();
        }

        private void listMonitorTimings_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listMonitorTimings.IndexFromPoint(e.Location);
            if (index >= 0)
            {
                var listDs = (ICollection<EncodedMonitorTimings>)listMonitorTimings.DataSource;
                if (index < listDs.Count)
                    SpawnTimingsEditor(listDs.ElementAt(index).encodedName, false);
            }
        }        

        private void btnNewEditCopyTiming_Click(object sender, EventArgs e)
        {
            bool isCopy = sender == btnCopyTiming;
            string existingTiming = null;
            if (sender != btnNewTiming)
            {
                var listDs = (ICollection<EncodedMonitorTimings>)listMonitorTimings.DataSource;
                var listIdx = listMonitorTimings.SelectedIndex;
                if (listIdx >= 0 && listIdx < listDs.Count)
                    existingTiming = listDs.ElementAt(listIdx).encodedName;
            }

            SpawnTimingsEditor(existingTiming, isCopy);
        }

        public void TimingChangedCallback(string oldTiming, string newTiming)
        {
            if (oldTiming != null && newTiming.Equals(oldTiming, StringComparison.OrdinalIgnoreCase))
                return;

            var listDs = (EncodedMonitorTimings[])listMonitorTimings.DataSource;
            EncodedMonitorTimings[] newDs = null;
            if (oldTiming != null)
            {
                newDs = new EncodedMonitorTimings[listDs.Length];
                Array.Copy(listDs, newDs, listDs.Length);
                int changedIndex = Array.FindIndex(newDs, x => x.encodedName.Equals(oldTiming, StringComparison.OrdinalIgnoreCase));
                if (changedIndex >= 0 && changedIndex < newDs.Length)
                    newDs[changedIndex].encodedName = newTiming;
            }
            else
            {
                newDs = new EncodedMonitorTimings[(listDs == null) ? 1 : (listDs.Length+1)];
                if (listDs != null)
                    Array.Copy(listDs, newDs, listDs.Length);

                newDs[newDs.Length - 1] = new EncodedMonitorTimings(newTiming);
            }

            var oldSelection = listMonitorTimings.SelectedIndex;
            listMonitorTimings.DataSource = newDs;
            listMonitorTimings.SelectedIndex = oldSelection;
        }
    }
}
