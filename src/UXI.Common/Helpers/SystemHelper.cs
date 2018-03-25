using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common.Helpers
{
    public static class SystemHelper
    {
        // TODO http://stackoverflow.com/questions/2236173/screen-resolution-problem-in-wpf
        //public static double ScreenWidth { get { return Math.Max(System.Windows.SystemParameters.VirtualScreenWidth, 1); } }
        //public static double ScreenHeight { get { return Math.Max(System.Windows.SystemParameters.VirtualScreenHeight, 1); } }

        public static bool IsAdministrator()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                isAdmin = false;
            }
            return isAdmin;
        }
    }
}
