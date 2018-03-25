using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Configuration.Storages
{
    public class IniFileLoader : IStorageLoader
    {
        public IStorage Load(string path)
        {
            return new IniFile(path);
        }

        public bool TryFindConfigurationFile(ref string path)
        {
            string search = path;
            if (System.IO.Path.HasExtension(search) == false || System.IO.Path.GetExtension(search).Equals(".ini", StringComparison.InvariantCultureIgnoreCase) == false)
            {
                search += ".ini";
            }

            if (System.IO.File.Exists(search))
            {
                path = search;
                return true;
            }

            return false;
        }
    }
}
