using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace NvTimingsEd
{
    public static class StructureExtensions
    {
        public static byte[] ToByteArray<T>(this T structure) where T : struct
        {
            var bufferSize = Marshal.SizeOf(structure);
            var byteArray = new byte[bufferSize];

            IntPtr handle = Marshal.AllocHGlobal(bufferSize);
            try
            {
                Marshal.StructureToPtr(structure, handle, true);
                Marshal.Copy(handle, byteArray, 0, bufferSize);
            }
            finally
            {
                Marshal.FreeHGlobal(handle);
                handle = IntPtr.Zero;
            }

            return byteArray;
        }

        public static T ToStructure<T>(this byte[] byteArray) where T : struct
        {
            var packet = new T();
            var bufferSize = Marshal.SizeOf(packet);
            IntPtr handle = Marshal.AllocHGlobal(bufferSize);
            T retStruct;
            try
            {
                Marshal.Copy(byteArray, 0, handle, bufferSize);
                retStruct = (T)Marshal.PtrToStructure(handle, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(handle);
                handle = IntPtr.Zero;
            }

            return retStruct;
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new ArgumentException(String.Format("The hex-encoded string cannot have an odd number of digits: {0}", hex));

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TimingsPart
    {
        public UInt16 total;
        public UInt16 visible;
        public UInt16 firstInactive;
        public UInt16 numBlanking;
        public UInt16 firstSync;
        public UInt16 numSync;
    }

    public struct ExpandedTimingsPart
    {
        public UInt16 visible;
        public UInt16 border;
        public UInt16 frontPorch;
        public UInt16 syncWidth;
        public UInt16 backPorch;
        public UInt16 blanking;
        public UInt16 total;

        public void FromTimingsPart(TimingsPart src)
        {
            total = src.total;
            visible = src.visible;
            border = (UInt16)(src.firstInactive - src.visible - 1);
            frontPorch = (UInt16)(src.firstSync - src.firstInactive);
            syncWidth = src.numSync;
            blanking = src.numBlanking;
            backPorch = (UInt16)(blanking - frontPorch - syncWidth);
        }

        public static ExpandedTimingsPart CreateFromTimingsPart(TimingsPart src)
        {
            ExpandedTimingsPart dst = new ExpandedTimingsPart();
            dst.FromTimingsPart(src);
            return dst;
        }

        public TimingsPart ToTimingsPart()
        {
            TimingsPart dst;
            dst.total = total;
            dst.visible = visible;
            dst.firstInactive = (UInt16)(visible + border + 1);
            dst.numBlanking = blanking;
            dst.firstSync = (UInt16)(dst.firstInactive + frontPorch);
            dst.numSync = syncWidth;
            return dst;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MonitorTimings
    {
        public static MonitorTimings CreateFromByteArray(byte[] bytes)
        {
            var timingsStr = new MonitorTimings();
            var requiredLength = Marshal.SizeOf(timingsStr);
            if (bytes.Length != requiredLength)
            {
                throw new ArgumentException(String.Format("Encoded display timing parameters must be {0} bytes long: {1}",
                    requiredLength, StructureExtensions.ByteArrayToString(bytes)));
            }

            timingsStr = StructureExtensions.ToStructure<MonitorTimings>(bytes);
            return timingsStr;
        }

        public byte[] ToByteArray()
        {
            return StructureExtensions.ToByteArray(this);
        }

        public UInt16 frequency; //in 10s of khz
        private UInt16 zeroPadding; //for future possible expansion
        public TimingsPart hor;
        public TimingsPart ver;
    }

    public struct EncodedMonitorTimings
    {
        public EncodedMonitorTimings(string encodedName)
        {
            this.encodedName = encodedName;
        }

        public override string ToString()
        {
            try
            {
                var timings = MonitorTimings.CreateFromByteArray(StructureExtensions.StringToByteArrayFastest(encodedName));
                var frequency = (decimal)(timings.frequency) / 100;
                var refresh = (frequency * 1000000) / (timings.hor.total*timings.ver.total);
                refresh = Math.Round(refresh * 1000) / 1000;

                return String.Format("{0}x{1} ({2}x{3}) @ {4}Hz ({5}MHz)", timings.hor.visible, timings.ver.visible,
                                                                            timings.hor.total, timings.ver.total,
                                                                            refresh, frequency);
            }
            catch (Exception)
            {
                return "ERROR:" + encodedName;
            }
        }

        public string encodedName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GlassesTimings
    {
        public static GlassesTimings CreateFromByteArray(byte[] bytes)
        {
            var timingsStr = new GlassesTimings();
            var requiredLength = Marshal.SizeOf(timingsStr);
            if (bytes.Length != requiredLength)
            {
                throw new ArgumentException(String.Format("Encoded glasses timing parameters must be {0} bytes long: {1}",
                    requiredLength, StructureExtensions.ByteArrayToString(bytes)));
            }

            timingsStr = StructureExtensions.ToStructure<GlassesTimings>(bytes);
            return timingsStr;
        }

        public byte[] ToByteArray()
        {
            return StructureExtensions.ToByteArray(this);
        }

        public static UInt32 MicrosToTicks(decimal micros)
        {
            return (UInt32)Math.Round(micros * 12);
        }

        public static decimal TicksToMicros(UInt32 ticks)
        {
            return (decimal)(ticks) / 12;
        }

        public UInt32 z, w, x, y;
        public byte cmd1, cmd2, cmd3, cmd4;
    }

    public struct GlassesCommand
    {
        public byte cmdIdx;

        public override string ToString() 
        {
            if (cmdIdx == 1)
                return "LEFT_OFF";
            else if (cmdIdx == 2)
                return "LEFT_ON";
            else if (cmdIdx == 3)
                return "RIGHT_OFF";
            else if (cmdIdx == 4)
                return "RIGHT_ON";
            else
                return "UNKNOWN_CMD";
        }
    }
}
