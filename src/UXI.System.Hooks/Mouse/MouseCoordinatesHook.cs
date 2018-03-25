using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UXI.System.Hooks.Mouse
{
    public class MouseCoordinatesHook
    {
        public bool TryGetCursorPosition(out Point point)
        {
            return WinApi.GetPhysicalCursorPos(out point);
        }
    }
}
