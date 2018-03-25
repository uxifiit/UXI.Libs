using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using UXI.SystemApi.Screen;

namespace UXI.SystemApi.Wpf
{
    public class WpfScreenParametersProvider : ScreenParametersProvider
    {
        //public bool TryGetDPIFactors(Window window, out Size dpiFactors)
        //{
        //    if (window != null)
        //    {
        //        PresentationSource presentation = PresentationSource.FromVisual(window);
        //        Matrix m = presentation.CompositionTarget.TransformToDevice;
        //        dpiFactors = new Size(m.M11, m.M22);

        //        //double ScreenHeight = SystemParameters.PrimaryScreenHeight * DpiHeightFactor;
        //        //double ScreenWidth = SystemParameters.PrimaryScreenWidth * DpiWidthFactor;
        //        return true;
        //    }

        //    dpiFactors = default(Size);
        //    return false;
        //}

        //public bool TryGetDPIFactors(out Size dpiFactors)
        //{
        //    return TryGetDPIFactors(Application.Current.MainWindow, out dpiFactors);
        //}

        public bool TryGetScreenInfo(out ScreenInfo screenInfo)
        {
            return TryGetScreenInfo(Application.Current.MainWindow, out screenInfo);
        }

      
        public bool TryGetScreenInfo(Window window, out ScreenInfo screenInfo)
        {
            var hwnd = new WindowInteropHelper(window).EnsureHandle();
            var monitor = ScreenInterop.MonitorFromWindow(hwnd, ScreenInterop.MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                screenInfo = GetScreenInfo(monitor);
                return true;
            }

            screenInfo = ScreenInfo.Empty;
            return false;
        }
    }
}
