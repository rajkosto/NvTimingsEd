using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NvTimingsEd
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RegistryKey parametersKey = null;
            try
            {
                parametersKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\NvStUSB\Parameters", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " Are you running this program as administrator?", "Error opening NvStUSB parameters key");
                return;
            }

            if (parametersKey == null)
            {
                MessageBox.Show("Required registry key is missing. Is the nVidia stereoscopic driver installed?", "Error opening NvStUSB parameters key");
                return;
            }

            Application.Run(new MainWindow(parametersKey));
        }
    }
}
