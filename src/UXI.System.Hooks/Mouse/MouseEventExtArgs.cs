using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UXI.System.Hooks.Mouse
{
    public class MouseEventArgs
    {
        /// <summary>
        /// Creates <see cref="MouseEventExtArgs"/> from Windows Message parameters, 
        /// based upon a system-wide global hook.
        /// </summary>
        /// <param name="wParam">The first Windows Message parameter.</param>
        /// <param name="lParam">The second Windows Message parameter.</param>
        /// <returns>A new MouseEventExtArgs object.</returns>
        internal static MouseEventArgs FromRawDataGlobal(int wParam, IntPtr lParam)
        {
            MouseStruct marshalledMouseStruct = (MouseStruct)Marshal.PtrToStructure(lParam, typeof(MouseStruct));
            return FromRawDataUniversal(wParam, marshalledMouseStruct);
        }

        /// <summary>
        /// Creates <see cref="MouseEventArgs"/> from relevant mouse data. 
        /// </summary>
        /// <param name="wParam">First Windows Message parameter.</param>
        /// <param name="mouseInfo">A MouseStruct containing information from which to construct MouseEventExtArgs.</param>
        /// <returns>A new MouseEventExtArgs object.</returns>
        private static MouseEventArgs FromRawDataUniversal(int wParam, MouseStruct mouseInfo)
        {
            MouseButtons button = MouseButtons.None;
            short mouseDelta = 0;
            int clickCount = 0;

            bool isMouseKeyDown = false;
            bool isMouseKeyUp = false;


            switch (wParam)
            {
                case Messages.WM_LBUTTONDOWN:
                    isMouseKeyDown = true;
                    button = MouseButtons.Left;
                    clickCount = 1;
                    break;
                case Messages.WM_LBUTTONUP:
                    isMouseKeyUp = true;
                    button = MouseButtons.Left;
                    clickCount = 1;
                    break;
                case Messages.WM_LBUTTONDBLCLK:
                    isMouseKeyDown = true;
                    button = MouseButtons.Left;
                    clickCount = 2;
                    break;
                case Messages.WM_RBUTTONDOWN:
                    isMouseKeyDown = true;
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;
                case Messages.WM_RBUTTONUP:
                    isMouseKeyUp = true;
                    button = MouseButtons.Right;
                    clickCount = 1;
                    break;
                case Messages.WM_RBUTTONDBLCLK:
                    isMouseKeyDown = true;
                    button = MouseButtons.Right;
                    clickCount = 2;
                    break;
                case Messages.WM_MBUTTONDOWN:
                    isMouseKeyDown = true;
                    button = MouseButtons.Middle;
                    clickCount = 1;
                    break;
                case Messages.WM_MBUTTONUP:
                    isMouseKeyUp = true;
                    button = MouseButtons.Middle;
                    clickCount = 1;
                    break;
                case Messages.WM_MBUTTONDBLCLK:
                    isMouseKeyDown = true;
                    button = MouseButtons.Middle;
                    clickCount = 2;
                    break;
                case Messages.WM_MOUSEWHEEL:
                    mouseDelta = mouseInfo.MouseData;
                    break;
                case Messages.WM_XBUTTONDOWN:
                    button = mouseInfo.MouseData == 1 
                           ? MouseButtons.XButton1 
                           : MouseButtons.XButton2;
                    isMouseKeyDown = true;
                    clickCount = 1;
                    break;

                case Messages.WM_XBUTTONUP:
                    button = mouseInfo.MouseData == 1 
                           ? MouseButtons.XButton1 
                           : MouseButtons.XButton2;
                    isMouseKeyUp = true;
                    clickCount = 1;
                    break;

                case Messages.WM_XBUTTONDBLCLK:
                    isMouseKeyDown = true;
                    button = mouseInfo.MouseData == 1 
                           ? MouseButtons.XButton1 
                           : MouseButtons.XButton2;
                    clickCount = 2;
                    break;

                case Messages.WM_MOUSEHWHEEL:
                    mouseDelta = mouseInfo.MouseData;
                    break;
            }

            var e = new MouseEventArgs(
                button,
                clickCount,
                mouseInfo.Point,
                mouseDelta,
                mouseInfo.Timestamp,
                isMouseKeyDown,
                isMouseKeyUp);

            return e;
        }

        //
        // Summary:
        //     Gets which mouse button was pressed.
        //
        // Returns:
        //     One of the System.Windows.Forms.MouseButtons values.
        public MouseButtons Button { get; }
        //
        // Summary:
        //     Gets the number of times the mouse button was pressed and released.
        //
        // Returns:
        //     An System.Int32 that contains the number of times the mouse button was pressed
        //     and released.
        public int Clicks { get; }
        //
        // Summary:
        //     Gets a signed count of the number of detents the mouse wheel has rotated, multiplied
        //     by the WHEEL_DELTA constant. A detent is one notch of the mouse wheel.
        //
        // Returns:
        //     A signed count of the number of detents the mouse wheel has rotated, multiplied
        //     by the WHEEL_DELTA constant.
        public int Delta { get; }
       
     

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventExtArgs"/> class. 
        /// </summary>
        /// <param name="buttons">One of the MouseButtons values indicating which mouse button was pressed.</param>
        /// <param name="clicks">The number of times a mouse button was pressed.</param>
        /// <param name="point">The x and y -coordinate of a mouse click, in pixels.</param>
        /// <param name="delta">A signed count of the number of detents the wheel has rotated.</param>
        /// <param name="timestamp">The system tick count when the event occurred.</param>
        /// <param name="isMouseKeyDown">True if event signals mouse button down.</param>
        /// <param name="isMouseKeyUp">True if event signals mouse button up.</param>
        internal MouseEventArgs(MouseButtons buttons, int clicks, Point point, int delta, int timestamp, bool isMouseKeyDown, bool isMouseKeyUp)
        {
            Button = buttons;
            Clicks = clicks;
            Delta = delta;
            Point = point;
            IsMouseKeyDown = isMouseKeyDown;
            IsMouseKeyUp = isMouseKeyUp;
            Timestamp = timestamp;
        }

        internal MouseEventArgs ToDoubleClickEventArgs()
        {
            return new MouseEventArgs(Button, 2, Point, Delta, Timestamp, IsMouseKeyDown, IsMouseKeyUp);
        }

        ///// <summary>
        ///// Set this property to <b>true</b> inside your event handler to prevent further processing of the event in other applications.
        ///// </summary>
        //public bool Handled { get; set; }

        /// <summary>
        /// True if event contains information about wheel scroll.
        /// </summary>
        public bool WheelScrolled
        {
            get { return Delta != 0; }
        }

        /// <summary>
        /// True if event signals a click. False if it was only a move or wheel scroll.
        /// </summary>
        public bool Clicked
        {
            get { return Clicks > 0; }
        }

        /// <summary>
        /// True if event signals mouse button down.
        /// </summary>
        public bool IsMouseKeyDown { get; }

        /// <summary>
        /// True if event signals mouse button up.
        /// </summary>
        public bool IsMouseKeyUp { get; }

        /// <summary>
        /// The system tick count of when the event occurred.
        /// </summary>
        public int Timestamp { get; }

        /// <summary>
        /// 
        /// </summary>
        public Point Point { get; }
    }
}
