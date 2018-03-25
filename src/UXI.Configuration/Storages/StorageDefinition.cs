using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Configuration.Storages
{
    public enum StorageType
    {
        File,
        Runtime
    }
    public class StorageDefinition
    {
        internal StorageType StorageType { get; private set; }
       
        public IStorage Storage { get; private set; } 
        
        internal string FilePath { get; private set; }

        public static StorageDefinition Create(IStorage storage)
        {
            return new StorageDefinition()
            {
                StorageType = StorageType.Runtime,
                Storage = storage
            };
        }

        public static StorageDefinition Create(string filepath)
        {
            return new StorageDefinition()
            {
                StorageType = StorageType.File,
                FilePath = filepath
            };
        }

    }
}
