using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Configuration.Settings;
using UXI.Configuration.Storages;

namespace UXI.Configuration
{
    public interface IConfigurationSource
    {
        bool HasSection(string name);

        IEnumerable<string> AddStorage(IStorage storage);

        IEnumerable<string> AddFile(string path);

        ISettings GetSection(string name);

        IEnumerable<string> Sections { get; } 
    }
}
