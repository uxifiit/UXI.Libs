using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Configuration.Storages;

namespace UXI.Configuration
{
    public abstract class ConfigurationInitializer
    {
        //protected ConfigurationInitializer() { }

        protected ConfigurationInitializer(IConfigurationSource source)
        {
            Initialize(source);
        }


        protected abstract IEnumerable<StorageDefinition> Storages
        {
            get;
        }

        protected void Initialize(IConfigurationSource source)
        {
            foreach (var storage in Storages)
            {
                if (storage.StorageType == StorageType.File)
                {
                    source.AddFile(storage.FilePath);
                }
                else if (storage.StorageType == StorageType.Runtime)
                {
                    source.AddStorage(storage.Storage);
                }
            }
        }
    }
}
