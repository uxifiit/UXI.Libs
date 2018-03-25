using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UXI.SystemApi
{
    class HookInterop
    {
        #region MouseCoordinates
        /// <summary>
        /// Ziska aktualne koordinaty mysi na ploche
        /// </summary>
        /// <param name="pt">Koordinaty</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern bool GetPhysicalCursorPos(out Point pt);
        #endregion
    }
}
