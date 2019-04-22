using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NvTimingsEd
{
    class ResourceManager
    {
        [System.Flags]
        enum LoadLibraryFlags : uint
        {
            DONT_RESOLVE_DLL_REFERENCES = 0x00000001,
            LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010,
            LOAD_LIBRARY_AS_DATAFILE = 0x00000002,
            LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 0x00000040,
            LOAD_LIBRARY_AS_IMAGE_RESOURCE = 0x00000020,
            LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200,
            LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000,
            LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100,
            LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800,
            LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400,
            LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, LoadLibraryFlags dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr FindResource(IntPtr hModule, IntPtr lpName, IntPtr lpType);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LockResource(IntPtr hResData);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint SizeofResource(IntPtr hModule, IntPtr hResInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool EnumResourceNames(IntPtr hModule, string lpType, IntPtr lpEnumFunc, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr BeginUpdateResource(string pFileName, [MarshalAs(UnmanagedType.Bool)] bool bDeleteExistingResources);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool UpdateResource(IntPtr hUpdate, IntPtr lpType, IntPtr lpName, ushort wLanguage, IntPtr lpData, uint cbData);

        public static bool UpdateResourceWithBytes(IntPtr hUpdate, IntPtr lpType, IntPtr lpName, ushort wLanguage, byte[] srcByteArr)
        {
            if (srcByteArr == null) //this deletes the resource
                return UpdateResource(hUpdate, lpType, lpName, wLanguage, IntPtr.Zero, 0);

            bool retVal = false;
            IntPtr hRawBytes = IntPtr.Zero;
            try
            {
                var allocSize = srcByteArr.Length;
                hRawBytes = Marshal.AllocHGlobal(allocSize);
                Marshal.Copy(srcByteArr, 0, hRawBytes, allocSize);
                retVal = UpdateResource(hUpdate, lpType, lpName, wLanguage, hRawBytes, (uint)allocSize);
            }
            finally
            {
                if (hRawBytes != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(hRawBytes);
                    hRawBytes = IntPtr.Zero;
                }
            }
            return retVal;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        public static byte[] GetResourceFromExecutable(string lpFileName, IntPtr lpName, IntPtr lpType)
        {
            byte[] uiBytes = null;
            IntPtr hModule = LoadLibraryEx(lpFileName, IntPtr.Zero, (uint)LoadLibraryFlags.LOAD_LIBRARY_AS_DATAFILE);
            if (hModule != IntPtr.Zero)
            {
                try
                {
                    IntPtr hResource = FindResource(hModule, lpName, lpType);
                    if (hResource != IntPtr.Zero)
                    {
                        uint resSize = SizeofResource(hModule, hResource);
                        IntPtr resData = LoadResource(hModule, hResource);
                        if (resData != IntPtr.Zero)
                        {
                            uiBytes = new byte[resSize];
                            IntPtr ipMemorySource = LockResource(resData);
                            Marshal.Copy(ipMemorySource, uiBytes, 0, (int)resSize);
                        }
                    }
                }
                finally
                {
                    FreeLibrary(hModule);
                    hModule = IntPtr.Zero;
                }
            }
            return uiBytes;
        }
    }
}
