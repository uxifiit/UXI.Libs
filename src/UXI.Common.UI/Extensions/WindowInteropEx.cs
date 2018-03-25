using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using UXI.Common.Helpers;

namespace UXI.Common.UI.Extensions
{
    public static class WindowInteropEx
    {
        public static void MakeTransparent(this Window window)
        {
            SetIsTransparent(window, true);
        }


        public static void MakeNormal(this Window window)
        {
            SetIsTransparent(window, false);
        }


        private static IntPtr GetOrEnsureHandle(Window window)
        {
            IntPtr hwnd = GetHandle(window);
            if (hwnd == IntPtr.Zero)
            {
                hwnd = new WindowInteropHelper(window).Handle;
                SetHandle(window, hwnd);
            }

            return hwnd;
        }



        public static IntPtr GetHandle(DependencyObject obj)
        {
            return (IntPtr)obj.GetValue(HandleProperty);
        }

        public static void SetHandle(DependencyObject obj, IntPtr value)
        {
            obj.SetValue(HandleProperty, value);
        }

        // Using a DependencyProperty as the backing store for Handle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HandleProperty =
            DependencyProperty.RegisterAttached("Handle", typeof(IntPtr), typeof(WindowInteropEx), new PropertyMetadata(IntPtr.Zero));



        public static bool GetIsTransparent(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsTransparentProperty);
        }

        public static void SetIsTransparent(DependencyObject obj, bool value)
        {
            obj.SetValue(IsTransparentProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsTransparent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTransparentProperty =
            DependencyProperty.RegisterAttached("IsTransparent", typeof(bool), typeof(WindowInteropEx), new PropertyMetadata(false, async (s, e) =>
            {
                var window = s as Window;

                if (window != null)
                {
                    if (window.IsLoaded == false)
                    {
                        await window.WaitUntilLoadedAsync();
                    }

                    bool transparent = (bool)e.NewValue;
                    IntPtr hwnd = GetOrEnsureHandle(window);
                    if (transparent)
                    {
                        WindowInterop.MakeTransparent(hwnd);
                    }
                    else
                    {
                        WindowInterop.MakeNormal(hwnd);
                    }
                }
            }));
    }
}
