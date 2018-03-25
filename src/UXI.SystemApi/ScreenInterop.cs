using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UXI.SystemApi
{
    public static class ScreenInterop
    {
        public const int MONITOR_DEFAULTTOPRIMERTY = 0x00000001;
        public const int MONITOR_DEFAULTTONEAREST = 0x00000002;


        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);


        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, NativeMonitorInfo lpmi);


        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct NativeRectangle
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }


            public NativeRectangle(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public sealed class NativeMonitorInfo
        {
            public int Size { get; } = Marshal.SizeOf(typeof(NativeMonitorInfo));
            public NativeRectangle Monitor { get; set; }
            public NativeRectangle Work { get; set; }
            public int Flags { get; set; }
        }



        //https://msdn.microsoft.com/en-us/library/windows/desktop/dd145062(v=vs.85).aspx
        [DllImport("User32.dll")]
        public static extern IntPtr MonitorFromPoint([In]Point pt, [In]uint dwFlags);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510(v=vs.85).aspx
        [DllImport("Shcore.dll")]
        public static extern IntPtr GetDpiForMonitor([In]IntPtr hmonitor, [In]DpiType dpiType, [Out]out uint dpiX, [Out]out uint dpiY);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/dn280511(v=vs.85).aspx
        public enum DpiType
        {
            Effective = 0,
            Angular = 1,
            Raw = 2,
        }
    }
}
