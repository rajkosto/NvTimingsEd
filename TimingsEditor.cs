using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NvTimingsEd
{
    public partial class TimingsEditor : Form
    {
        static readonly ICollection<GlassesCommand> defaultGlassesCmds;

        static TimingsEditor()
        {
            var glassesCmds = new GlassesCommand[4];
            for (int i = 0; i < glassesCmds.Length; i++)
                glassesCmds[i].cmdIdx = (byte)(i + 1);

            defaultGlassesCmds = glassesCmds;
        }

        private MainWindow parent;
        private string previousName;
        private SharedDisposable<RegistryKey> monitorKey;
        public TimingsEditor(MainWindow parent, SharedDisposable<RegistryKey> monitorKey, bool isCopy, string encodedMonitor, byte[] encodedGlasses)
        {
            InitializeComponent();
            listWaveforms.DataSource = defaultGlassesCmds;

            this.parent = parent;
            this.monitorKey = monitorKey;
            this.previousName = isCopy ? null : encodedMonitor;

            if (encodedMonitor != null)
                this.textDisplayTimings.Text = encodedMonitor;
            if (encodedGlasses!= null)
                this.textGlassesTimings.Text = StructureExtensions.ByteArrayToString(encodedGlasses);

            if (encodedMonitor != null)
                btnDecode_Click(btnDecodeDisplayTimings, null);
            if (encodedGlasses != null)
                btnDecode_Click(btnDecodeGlassesTimings, null);

            UpdateListBoxButtons();
            UpdateSliderParams();            
        }

        private void TimingsEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (monitorKey != null)
            {
                monitorKey.Dispose();
                monitorKey = null;
            }
        }

        static readonly string str_Rescale = "Rescale to this";
        static readonly string str_ResetScale = "Reset scale";
        static readonly int int_DefaultSliderMaximum = 200000;

        private void UpdateSliderParams()
        {
            numX.Maximum = GlassesTimings.TicksToMicros((uint)sliderX.Maximum);
            numY.Maximum = GlassesTimings.TicksToMicros((uint)sliderY.Maximum);
            numZ.Maximum = GlassesTimings.TicksToMicros((uint)sliderZ.Maximum);
            numW.Maximum = GlassesTimings.TicksToMicros((uint)sliderW.Maximum);

            if (numActualRefresh.Value >= 1)
                btnCopyFromRefresh.Enabled = (1 / numActualRefresh.Value) * 1000000 < numZ.Maximum;
            else
                btnCopyFromRefresh.Enabled = false;

            if (sliderZ.Value >= 1)
            {
                btnRescaleOthers.Enabled = sliderX.Value <= sliderZ.Value &&
                                            sliderY.Value <= sliderZ.Value &&
                                            sliderW.Value <= sliderZ.Value;
            }
            else
                btnRescaleOthers.Enabled = false;

            if (sliderW.Maximum == sliderZ.Value && 
                sliderY.Maximum == sliderZ.Value &&
                sliderX.Maximum == sliderZ.Value)
            {
                btnRescaleOthers.Text = str_ResetScale;
                btnRescaleOthers.Enabled = true;
            }                
            else
                btnRescaleOthers.Text = str_Rescale;
        }

        private void UpdateListBoxButtons()
        {
            var dataSrc = (ICollection<GlassesCommand>)(listWaveforms.DataSource);

            btnUp.Enabled = listWaveforms.SelectedIndex > 0;
            btnDown.Enabled = listWaveforms.SelectedIndex < (dataSrc.Count()-1);
        }

        private void listWaveforms_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateListBoxButtons();
        }

        private void btnUpDown_Click(object sender, EventArgs e)
        {
            int oldIndex = listWaveforms.SelectedIndex;
            int newIndex = oldIndex;
            
            if (sender == btnUp)
                newIndex--;
            else if (sender == btnDown)
                newIndex++;

            if (newIndex != oldIndex)
            {
                var dataSrcOrig = (ICollection<GlassesCommand>)(listWaveforms.DataSource);
                var dataSrc = dataSrcOrig.ToArray();
                var tmp = dataSrc[oldIndex];
                dataSrc[oldIndex] = dataSrc[newIndex];
                dataSrc[newIndex] = tmp;

                listWaveforms.DataSource = dataSrc;
                listWaveforms.SelectedIndex = newIndex;
            }            
        }

        private bool suppressValueChangedEvent = false;
        private void NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown widget = (NumericUpDown)sender;
            if (widget == numActualRefresh)
            {
                UpdateSliderParams();
                return;
            }

            if (suppressValueChangedEvent)
                return;

            bool horizontalData=false, verticalData=false;
            int currFrontPorch=0, currSyncWidth=0, currVisible=0, currBorder=0;
            if (widget == numHorBlanking || widget == numHorBackPorch || widget == numHorTotal ||
                widget == numHorFrontPorch || widget == numHorSyncWidth || widget == numHorVisible || widget == numHorBorder)
            {
                currFrontPorch = (int)numHorFrontPorch.Value;
                currSyncWidth = (int)numHorSyncWidth.Value;
                currVisible = (int)numHorVisible.Value;
                currBorder = (int)numHorBorder.Value;

                horizontalData = true;
            }
            else if (widget == numVerBlanking || widget == numVerBackPorch || widget == numVerTotal ||
                widget == numVerFrontPorch || widget == numVerSyncWidth || widget == numVerVisible || widget == numVerBorder)
            {
                currFrontPorch = (int)numVerFrontPorch.Value;
                currSyncWidth = (int)numVerSyncWidth.Value;
                currVisible = (int)numVerVisible.Value;
                currBorder = (int)numVerBorder.Value;

                verticalData = true;
            }

            int newBlanking = 0, newBackPorch = 0, newTotal = 0;
            if (radBlanking.Checked)
            {
                newBlanking = verticalData ? (int)numVerBlanking.Value : (int)numHorBlanking.Value;
                newBackPorch = newBlanking - currFrontPorch - currSyncWidth;
                newTotal = currVisible + currBorder + newBlanking;
            }
            else if (radTotal.Checked)
            {
                newTotal = verticalData ? (int)numVerTotal.Value : (int)numHorTotal.Value;
                newBlanking = newTotal - currVisible - currBorder;
                newBackPorch = newBlanking - currFrontPorch - currSyncWidth;
            }
            else if (radBackPorch.Checked)
            {
                newBackPorch = verticalData ? (int)numVerBackPorch.Value : (int)numHorBackPorch.Value;
                newBlanking = newBackPorch + currFrontPorch + currSyncWidth;
                newTotal = currVisible + currBorder + newBlanking;
            }
            else
            {
                newBlanking = verticalData ? (int)numVerBlanking.Value : (int)numHorBlanking.Value;
                newBackPorch = verticalData ? (int)numVerBackPorch.Value : (int)numHorBackPorch.Value;
                newTotal = verticalData ? (int)numVerTotal.Value : (int)numHorTotal.Value;
            }

            suppressValueChangedEvent = true;
            try
            {
                if (verticalData)
                {
                    numVerBlanking.Value = newBlanking;
                    numVerBackPorch.Value = newBackPorch;
                    numVerTotal.Value = newTotal;
                }
                else if (horizontalData)
                {
                    numHorBlanking.Value = newBlanking;
                    numHorBackPorch.Value = newBackPorch;
                    numHorTotal.Value = newTotal;
                }

                decimal frequency = Math.Round(numWantedClock.Value * 100) / 100;
                decimal horizontal = Math.Round(numWantedHoriz.Value * 1000) / 1000;
                decimal refresh = Math.Round(numWantedRefresh.Value * 1000) / 1000;
                decimal totalPixels = (numHorTotal.Value * numVerTotal.Value);
                if (radPixelClock.Checked)
                {       
                    if (numHorTotal.Value >= 1)
                        horizontal = (frequency * 1000) / numHorTotal.Value;
                    if (totalPixels >= 1)
                        refresh = (frequency * 1000000) / totalPixels;
                }
                else if (radHorizontal.Checked)
                {
                    frequency = (horizontal * numHorTotal.Value) / 1000;
                    if (totalPixels >= 1)
                        refresh = (frequency * 1000000) / totalPixels;
                }
                else if (radRefreshRate.Checked)
                {
                    frequency = (refresh * totalPixels) / 1000000;
                    if (numHorTotal.Value >= 1)
                        horizontal = (frequency * 1000) / numHorTotal.Value;
                }

                if (frequency >= numWantedClock.Minimum)
                    numWantedClock.Value = frequency;
                if (horizontal >= numWantedHoriz.Minimum)
                    numWantedHoriz.Value = horizontal;
                if (refresh >= numWantedRefresh.Value)
                    numWantedRefresh.Value = refresh;

                frequency = Math.Round(frequency * 100) / 100;
                if (numHorTotal.Value >= 1)
                    horizontal = (frequency * 1000) / numHorTotal.Value;
                if (totalPixels >= 1)
                    refresh = (frequency * 1000000) / totalPixels;

                if (frequency >= numWantedClock.Minimum)
                    numActualClock.Value = frequency;
                if (horizontal >= numWantedHoriz.Minimum)
                    numActualHoriz.Value = horizontal;
                if (refresh >= numWantedRefresh.Value)
                    numActualRefresh.Value = refresh;
            }
            finally { suppressValueChangedEvent = false; }
        }

        public void ApplyHorTimings(ExpandedTimingsPart hor)
        {
            suppressValueChangedEvent = true;
            try
            {
                numHorVisible.Value = hor.visible;
                numHorBorder.Value = hor.border;
                numHorFrontPorch.Value = hor.frontPorch;
                numHorSyncWidth.Value = hor.syncWidth;
                numHorBackPorch.Value = hor.backPorch;
                numHorBlanking.Value = hor.blanking;
                numHorTotal.Value = hor.total;
            }
            finally { suppressValueChangedEvent = false; }
        }
        public void ApplyVerTimings(ExpandedTimingsPart ver)
        {
            suppressValueChangedEvent = true;
            try
            {
                numVerVisible.Value = ver.visible;
                numVerBorder.Value = ver.border;
                numVerFrontPorch.Value = ver.frontPorch;
                numVerSyncWidth.Value = ver.syncWidth;
                numVerBackPorch.Value = ver.backPorch;
                numVerBlanking.Value = ver.blanking;
                numVerTotal.Value = ver.total;
            }
            finally { suppressValueChangedEvent = false; }
        }

        public void ApplyMonitorTimings(MonitorTimings all)
        {
            var frequency = (decimal)(all.frequency)/100;
            var hor = ExpandedTimingsPart.CreateFromTimingsPart(all.hor);
            var ver = ExpandedTimingsPart.CreateFromTimingsPart(all.ver);
            var horizontal = (frequency*1000) / hor.total;
            var refresh = (frequency*1000000) / (hor.total*ver.total);

            suppressValueChangedEvent = true;
            try
            {
                numWantedClock.Value = numActualClock.Value = frequency;
                numWantedHoriz.Value = numActualHoriz.Value = horizontal;
                numWantedRefresh.Value = numActualRefresh.Value = refresh;

                ApplyHorTimings(hor);
                ApplyVerTimings(ver);
            }
            finally { suppressValueChangedEvent = false; }
        }

        public MonitorTimings GetMonitorTimings()
        {
            var hor = new ExpandedTimingsPart();
            hor.visible = (UInt16)numHorVisible.Value;
            hor.border = (UInt16)numHorBorder.Value;
            hor.frontPorch = (UInt16)numHorFrontPorch.Value;
            hor.syncWidth = (UInt16)numHorSyncWidth.Value;
            hor.backPorch = (UInt16)numHorBackPorch.Value;
            hor.blanking = (UInt16)numHorBlanking.Value;
            hor.total = (UInt16)numHorTotal.Value;
            var ver = new ExpandedTimingsPart();
            ver.visible = (UInt16)numVerVisible.Value;
            ver.border = (UInt16)numVerBorder.Value;
            ver.frontPorch = (UInt16)numVerFrontPorch.Value;
            ver.syncWidth = (UInt16)numVerSyncWidth.Value;
            ver.backPorch = (UInt16)numVerBackPorch.Value;
            ver.blanking = (UInt16)numVerBlanking.Value;
            ver.total = (UInt16)numVerTotal.Value;

            var dst = new MonitorTimings();
            dst.frequency = (UInt32)(numActualClock.Value * 100);
            dst.hor = hor.ToTimingsPart();
            dst.ver = ver.ToTimingsPart();
            return dst;
        }

        private void GlassesNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown widget = (NumericUpDown)sender;
            if (suppressValueChangedEvent)
                return;

            suppressValueChangedEvent = true;
            try
            {
                if (widget == numX)
                    sliderX.Value = (int)GlassesTimings.MicrosToTicks(widget.Value);
                else if (widget == numY)
                    sliderY.Value = (int)GlassesTimings.MicrosToTicks(widget.Value);
                else if (widget == numZ)
                    sliderZ.Value = (int)GlassesTimings.MicrosToTicks(widget.Value);
                else if (widget == numW)
                    sliderW.Value = (int)GlassesTimings.MicrosToTicks(widget.Value);
            }
            finally { suppressValueChangedEvent = false; }
            UpdateSliderParams();
        }

        private void GlassesSlider_ValueChanged(object sender, EventArgs e)
        {
            TrackBar widget = (TrackBar)sender;
            if (suppressValueChangedEvent)
                return;

            suppressValueChangedEvent = true;
            try
            {
                if (widget == sliderX)
                    numX.Value = GlassesTimings.TicksToMicros((UInt32)widget.Value);
                else if (widget == sliderY)
                    numY.Value = GlassesTimings.TicksToMicros((UInt32)widget.Value);
                else if (widget == sliderZ)
                    numZ.Value = GlassesTimings.TicksToMicros((UInt32)widget.Value);
                else if (widget == sliderW)
                    numW.Value = GlassesTimings.TicksToMicros((UInt32)widget.Value);
            }
            finally { suppressValueChangedEvent = false; }
            UpdateSliderParams();
        }

        public void ApplyGlassesTimings(GlassesTimings tim)
        {
            sliderZ.Maximum = int_DefaultSliderMaximum;
            sliderX.Maximum = sliderZ.Maximum;
            sliderY.Maximum = sliderZ.Maximum;
            sliderW.Maximum = sliderZ.Maximum;

            sliderZ.Value = (int)tim.z;
            sliderW.Value = (int)tim.w;
            sliderX.Value = (int)tim.x;
            sliderY.Value = (int)tim.y;

            var newDataSrc = new GlassesCommand[defaultGlassesCmds.Count()];
            newDataSrc[0] = defaultGlassesCmds.FirstOrDefault(s => s.cmdIdx == tim.cmd1);
            newDataSrc[1] = defaultGlassesCmds.FirstOrDefault(s => s.cmdIdx == tim.cmd2);
            newDataSrc[2] = defaultGlassesCmds.FirstOrDefault(s => s.cmdIdx == tim.cmd3);
            newDataSrc[3] = defaultGlassesCmds.FirstOrDefault(s => s.cmdIdx == tim.cmd4);

            listWaveforms.SelectedIndex = 0;
            listWaveforms.DataSource = newDataSrc;
            UpdateListBoxButtons();
            UpdateSliderParams();
        }

        public GlassesTimings GetGlassesTimings()
        {
            var dst = new GlassesTimings();
            dst.z = (UInt32)sliderZ.Value;
            dst.w = (UInt32)sliderW.Value;
            dst.x = (UInt32)sliderX.Value;
            dst.y = (UInt32)sliderY.Value;

            var dataSrc = (ICollection<GlassesCommand>)listWaveforms.DataSource;
            dst.cmd1 = dataSrc.ElementAt(0).cmdIdx;
            dst.cmd2 = dataSrc.ElementAt(1).cmdIdx;
            dst.cmd3 = dataSrc.ElementAt(2).cmdIdx;
            dst.cmd4 = dataSrc.ElementAt(3).cmdIdx;

            return dst;
        }

        void HexBox_TextChanged(object sender, EventArgs e)
        {
            TextBox widget = (TextBox)sender;
            var newText = widget.Text;
            char[] delimiters = { ' ', '\t', '\r', '\n' };
            string[] words = newText.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            newText = "";
            for (int i=0; i<words.Length; i++)
            {
                string currWord = words[i].Trim();
                if (currWord.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                    currWord = currWord.Substring(2);

                {
                    char[] arr = currWord.ToCharArray();
                    arr = Array.FindAll<char>(arr, (c => (char.IsDigit(c) || (char.ToLowerInvariant(c) >= 'a' && char.ToLowerInvariant(c) <= 'f'))));
                    currWord = new string(arr);
                }               

                if (currWord.Length > 0)
                    newText += currWord;
            }
            widget.Text = newText;
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            if (sender == btnDecodeDisplayTimings)
            {
                try
                {
                    var timings = MonitorTimings.CreateFromByteArray(StructureExtensions.StringToByteArrayFastest(textDisplayTimings.Text));
                    ApplyMonitorTimings(timings);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error parsing encoded display timings");
                    return;
                }
            }
            else if (sender == btnDecodeGlassesTimings)
            {
                try
                {
                    var timings = GlassesTimings.CreateFromByteArray(StructureExtensions.StringToByteArrayFastest(textGlassesTimings.Text));
                    ApplyGlassesTimings(timings);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error parsing encoded glasses timings");
                    return;
                }
            }
        }

        private void btnEncode_Click(object sender, EventArgs e)
        {
            if (sender == btnEncodeDisplayTimings)
            {
                var timings = GetMonitorTimings();
                textDisplayTimings.Text = StructureExtensions.ByteArrayToString(timings.ToByteArray());
            }
            else if (sender == btnEncodeGlassesTimings)
            {
                var timings = GetGlassesTimings();
                textGlassesTimings.Text = StructureExtensions.ByteArrayToString(timings.ToByteArray());
            }            
        }

        private void NumericUpDown_Enter(object sender, EventArgs e)
        {
            var widget = (NumericUpDown)sender;
            widget.Select(0, widget.Text.Length);
        }

        private void RadioCheckedChanged(object sender, EventArgs e)
        {
            numHorBackPorch.ReadOnly = numVerBackPorch.ReadOnly = !radBackPorch.Checked;
            numHorTotal.ReadOnly = numVerTotal.ReadOnly = !radTotal.Checked;
            numHorBlanking.ReadOnly = numVerBlanking.ReadOnly = !radBlanking.Checked;
            numWantedRefresh.ReadOnly = !radRefreshRate.Checked;
            numWantedHoriz.ReadOnly = !radHorizontal.Checked;
            numWantedClock.ReadOnly = !radPixelClock.Checked;
        }

        private void btnCopyFromRefresh_Click(object sender, EventArgs e)
        {
            numZ.Value = (1 / numActualRefresh.Value) * 1000000;
        }

        private void btnRescaleOthers_Click(object sender, EventArgs e)
        {
            Button widget = (Button)sender;

            if (str_ResetScale.Equals(widget.Text, StringComparison.OrdinalIgnoreCase))
            {
                sliderZ.Maximum = int_DefaultSliderMaximum;
                sliderX.Maximum = int_DefaultSliderMaximum;
                sliderY.Maximum = int_DefaultSliderMaximum;
                sliderW.Maximum = int_DefaultSliderMaximum;
            }
            else
            {
                sliderZ.Maximum = sliderZ.Value;
                sliderX.Maximum = sliderZ.Value;
                sliderY.Maximum = sliderZ.Value;
                sliderW.Maximum = sliderZ.Value;
            }
            
            UpdateSliderParams();
        }

        private bool ApplyChangesAndNotify()
        {
            try
            {
                var monitorTimings = StructureExtensions.ByteArrayToString(GetMonitorTimings().ToByteArray());
                var glassesTimings = GetGlassesTimings().ToByteArray();

                if (previousName == null || !previousName.Equals(monitorTimings, StringComparison.OrdinalIgnoreCase))
                {
                    if (monitorKey.Value.GetValue(monitorTimings) != null)
                    {
                        var dlgRes = MessageBox.Show("This monitor timing already exists. Do you wish to overwrite it?", 
                                                        "Overwrite monitor timing?", MessageBoxButtons.YesNo);
                        if (dlgRes != DialogResult.Yes)
                            return false;
                    }
                }

                monitorKey.Value.SetValue(monitorTimings, glassesTimings, RegistryValueKind.Binary);
                if (previousName != null && !previousName.Equals(monitorTimings, StringComparison.OrdinalIgnoreCase))
                    monitorKey.Value.DeleteValue(previousName, false);

                parent.TimingChangedCallback(previousName, monitorTimings);
                previousName = monitorTimings;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error writing new values to registry");
                return false;
            }
        }

        private void DialogEndBtn_Click(object sender, EventArgs e)
        {
            if (sender == btnOK)
            {
                if (ApplyChangesAndNotify())
                    this.DialogResult = DialogResult.OK;
                else
                    return;
            }
            else if (sender == btnCancel)
                this.DialogResult = DialogResult.Cancel;
            else
                return;

            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyChangesAndNotify();
        }
    }
}
