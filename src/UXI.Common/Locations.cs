using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Common
{
    public static class Locations
    {
        public static string ExecutingAssemblyLocationPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        public static string CallingAssemblyLocationPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            }
        }

        public static string EntryAssemblyLocationPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
        }
    }
}
