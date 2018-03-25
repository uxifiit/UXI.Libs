using System;

namespace UXI.SystemApi.Screen
{
    public class ScreenParametersProvider : IScreenParametersProvider
    {
        public bool TryGetScreenInfo(int pointLeft, int pointTop, out ScreenInfo screenInfo)
        {
            var point = new Point(pointLeft, pointTop);
            var monitor = ScreenInterop.MonitorFromPoint(point, ScreenInterop.MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                screenInfo = GetScreenInfo(monitor);
                return true;
            }

            screenInfo = ScreenInfo.Empty;
            return false;
        }


        protected ScreenInfo GetScreenInfo(IntPtr monitor)
        {
            var monitorInfo = new ScreenInterop.NativeMonitorInfo();
            ScreenInterop.GetMonitorInfo(monitor, monitorInfo);

            var left = monitorInfo.Monitor.Left;
            var top = monitorInfo.Monitor.Top;
            var width = (monitorInfo.Monitor.Right - monitorInfo.Monitor.Left);
            var height = (monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top);

            return new ScreenInfo(left, top, width, height);
        }


        public void GetScreenDpi(ScreenInfo screen, ScreenInterop.DpiType dpiType, out uint dpiX, out uint dpiY)
        {
            var point = new Point(screen.Left + 1, screen.Top + 1);
            var monitor = ScreenInterop.MonitorFromPoint(point, ScreenInterop.MONITOR_DEFAULTTONEAREST);

            ScreenInterop.GetDpiForMonitor(monitor, dpiType, out dpiX, out dpiY);
        }
    }
}
