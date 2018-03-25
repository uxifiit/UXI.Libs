using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Configuration.Storages
{
    public interface IStorage
    {
        IEnumerable<string> Sections { get; }

        IEnumerable<string> GetKeys(string section);

        bool ContainsKey(string section, string key);

        bool TryRead(string section, string key, Type valueType, out object value);

        bool Write(string section, string key, object value);

        void Remove(string section, string key);
    }
}
