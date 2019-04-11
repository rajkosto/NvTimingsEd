using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace NvTimingsEd
{
    static class RegistryExtensions
    {
        public static void CopyTo(this RegistryKey src, RegistryKey dst)
        {
            // copy the values
            foreach (var name in src.GetValueNames())
            {
                dst.SetValue(name, src.GetValue(name, null, RegistryValueOptions.DoNotExpandEnvironmentNames), src.GetValueKind(name));
            }

            // copy the subkeys
            foreach (var name in src.GetSubKeyNames())
            {
                using (var srcSubKey = src.OpenSubKey(name, false))
                {
                    var dstSubKey = dst.CreateSubKey(name);
                    srcSubKey.CopyTo(dstSubKey);
                }
            }
        }

        public static void MoveTo(this RegistryKey src, RegistryKey dst)
        {
            src.CopyTo(dst);
            src.Delete();
        }

        public static void Delete(this RegistryKey key)
        {
            using (var parentKey = key.GetParent(true))
            {
                var keyName = key.Name.Split('\\').Last();
                parentKey.DeleteSubKeyTree(keyName);
            }
        }

        public static RegistryKey GetParent(this RegistryKey key)
        {
            return key.GetParent(false);
        }

        public static RegistryKey GetParent(this RegistryKey key, bool writable)
        {
            var items = key.Name.Split('\\');
            var hiveName = items.First();
            var parentKeyName = String.Join("\\", items.Skip(1).Reverse().Skip(1).Reverse());

            var hive = GetHive(hiveName);
            using (var baseKey = RegistryKey.OpenBaseKey(hive, key.View))
            {
                return baseKey.OpenSubKey(parentKeyName, writable);
            }
        }

        private static RegistryHive GetHive(string hiveName)
        {
            if (hiveName.Equals("HKEY_CLASSES_ROOT", StringComparison.OrdinalIgnoreCase))
                return RegistryHive.ClassesRoot;
            else if (hiveName.Equals("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase))
                return RegistryHive.CurrentUser;
            else if (hiveName.Equals("HKEY_LOCAL_MACHINE", StringComparison.OrdinalIgnoreCase))
                return RegistryHive.LocalMachine;
            else if (hiveName.Equals("HKEY_USERS", StringComparison.OrdinalIgnoreCase))
                return RegistryHive.Users;
            else if (hiveName.Equals("HKEY_PERFORMANCE_DATA", StringComparison.OrdinalIgnoreCase))
                return RegistryHive.PerformanceData;
            else if (hiveName.Equals("HKEY_CURRENT_CONFIG", StringComparison.OrdinalIgnoreCase))
                return RegistryHive.CurrentConfig;
            else if (hiveName.Equals("HKEY_DYN_DATA", StringComparison.OrdinalIgnoreCase))
                return RegistryHive.DynData;
            else
                throw new NotImplementedException(hiveName);
        }
    }
}
