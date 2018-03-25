using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UXI.System.Hooks.Mouse
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct MouseStruct
    {
        /// <summary>
        /// Specifies a Point structure that contains the X- and Y-coordinates of the cursor, in screen coordinates. 
        /// </summary>
        [FieldOffset(0x00)]
        public Point Point;

        /// <summary>
        /// Specifies information associated with the message.
        /// </summary>
        /// <remarks>
        /// The possible values are:
        /// <list type="bullet">
        /// <item>
        /// <description>0 - No Information</description>
        /// </item>
        /// <item>
        /// <description>1 - X-Button1 Click</description>
        /// </item>
        /// <item>
        /// <description>2 - X-Button2 Click</description>
        /// </item>
        /// <item>
        /// <description>120 - Mouse Scroll Away from User</description>
        /// </item>
        /// <item>
        /// <description>-120 - Mouse Scroll Toward User</description>
        /// </item>
        /// </list>
        /// </remarks>
        [FieldOffset(0x0A)]
        public Int16 MouseData;

        /// <summary>
        /// Returns a Timestamp associated with the input, in System Ticks.
        /// </summary>
        [FieldOffset(0x10)]
        public Int32 Timestamp;
    }
}
