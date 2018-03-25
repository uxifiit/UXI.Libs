using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UXI.System.Hooks.Mouse
{
    class MouseEventsHook : HookBase
    {
        private const int WH_MOUSE_LL = 14;

        protected override int GetHookId()
        {
            return WH_MOUSE_LL;
        }

        private Point m_PreviousPosition;
        private int m_PreviousClickedTime;
        private MouseButtons m_DownButtonsWaitingForMouseUp;
        private MouseButtons m_SuppressButtonUpFlags;
        private MouseButtons m_PreviousClicked;
        private int m_SystemDoubleClickTime;

     
        //public event MouseEventHandler MouseMove;
        //public event EventHandler<MouseEventExtArgs> MouseMoveExt;
        //public event MouseEventHandler MouseClick;
        [Obsolete("To suppress mouse clicks use MouseDownExt event instead.")]
        //public event EventHandler<MouseEventExtArgs> MouseClickExt;
        //public event MouseEventHandler MouseDown;
        public event EventHandler<MouseEventExtArgs> MouseDownExt;
        public event MouseEventHandler MouseUp;
        public event MouseEventHandler MouseWheel;
        //public event MouseEventHandler MouseDoubleClick;
        private Point m_PreviousClickedPosition;


        public MouseEventsHook()
        {
            m_PreviousPosition = new Point(-1, -1);
            m_PreviousClickedTime = 0;
            m_DownButtonsWaitingForMouseUp = MouseButtons.None;
            m_SuppressButtonUpFlags = MouseButtons.None;
            m_PreviousClicked = MouseButtons.None;
            m_SystemDoubleClickTime = WinApi.GetDoubleClickTime();
        }

   
        protected override bool ProcessCallback(int wParam, IntPtr lParam)
        {
            MouseEventArgs e = MouseEventArgs.FromRawDataGlobal(wParam, lParam);

            if (e.IsMouseKeyDown)
            {
                ProcessMouseDown(ref e);
            }

            if (e.Clicks == 1 && e.IsMouseKeyUp && !e.Handled)
            {
                ProcessMouseClick(ref e);
            }

            if (e.Clicks == 2 && !e.Handled)
            {
                OnMouseEventHandler(MouseDoubleClick, e);
            }

            if (e.IsMouseKeyUp)
            {
                ProcessMouseUp(ref e);
            }

            if (e.WheelScrolled)
            {
                OnMouseEventHandler(MouseWheel, e);
            }

            if (HasMoved(e.Point))
            {
                ProcessMouseMove(ref e);
            }

            return !e.Handled;
        }

        private void ProcessMouseDown(ref MouseEventArgs e)
        {
            ProcessPossibleDoubleClick(ref e);
            

            OnMouseEventHandler(MouseDown, e);
            OnMouseEventHandlerExt(MouseDownExt, e);
            if (e.Handled)
            {
                SetSupressButtonUpFlag(e.Button);
                e.Handled = true;
            }
        }

        private void ProcessPossibleDoubleClick(ref MouseEventExtArgs e)
        {
            if (IsDoubleClick(e.Button, e.Timestamp, e.Point))
            {
                e = e.ToDoubleClickEventArgs();
                m_DownButtonsWaitingForMouseUp = MouseButtons.None;
                m_PreviousClicked = MouseButtons.None;
                m_PreviousClickedTime = 0;
            }
            else
            {
                m_DownButtonsWaitingForMouseUp |= e.Button;
                m_PreviousClickedTime = e.Timestamp;
            }
        }

        private void ProcessMouseClick(ref MouseEventExtArgs e)
        {
            if ((m_DownButtonsWaitingForMouseUp & e.Button) != MouseButtons.None)
            {
                m_PreviousClicked = e.Button;
                m_PreviousClickedPosition = e.Point;
                m_DownButtonsWaitingForMouseUp = MouseButtons.None;
                OnMouseEventHandler(MouseClick, e);
                OnMouseEventHandlerExt(MouseClickExt, e);
            }
        }

        private void ProcessMouseUp(ref MouseEventExtArgs e)
        {
            if (!HasSupressButtonUpFlag(e.Button))
            {
                OnMouseEventHandler(MouseUp, e);
            }
            else
            {
                RemoveSupressButtonUpFlag(e.Button);
                e.Handled = true;
            }
        }

        private void ProcessMouseMove(ref MouseEventExtArgs e)
        {
            m_PreviousPosition = e.Point;

            OnMouseEventHandler(MouseMove, e);
            OnMouseEventHandlerExt(MouseMoveExt, e);
        }

        private bool HasMoved(Point actualPoint)
        {
            return m_PreviousPosition != actualPoint;
        }

        private void OnMouseEventHandler(MouseEventHandler handler, MouseEventArgs e)
        {
            handler?.Invoke(this, e);
        }


        private void OnMouseEventHandlerExt(EventHandler<MouseEventArgs> handler, MouseEventArgs e)
        {
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void SetSupressButtonUpFlag(MouseButtons button)
        {
            m_SuppressButtonUpFlags = m_SuppressButtonUpFlags | button;
        }

        private bool IsDoubleClick(MouseButtons button, int timestamp, Point pos)
        {
            return
                button == m_PreviousClicked &&
                pos == m_PreviousClickedPosition && // Click-move-click exception, see Patch 11222
                timestamp - m_PreviousClickedTime <= m_SystemDoubleClickTime; // Mouse.GetDoubleClickTime();
        }

        private bool HasSupressButtonUpFlag(MouseButtons button)
        {
            return (m_SuppressButtonUpFlags & button) != 0;
        }

        private void RemoveSupressButtonUpFlag(MouseButtons button)
        {
            m_SuppressButtonUpFlags = m_SuppressButtonUpFlags ^ button;
        }

        
    }
}
