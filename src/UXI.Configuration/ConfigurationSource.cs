using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UXI.Configuration.Settings;
using UXI.Configuration.Storages;

namespace UXI.Configuration
{
    public class ConfigurationSource : IConfigurationSource
    {
        private readonly List<IStorage> _storages = new List<IStorage>();
        private readonly ConcurrentDictionary<string, StorageSettings> sections = new ConcurrentDictionary<string, StorageSettings>();

        public IEnumerable<string> Sections { get { return sections.Keys; } }

        private readonly IEnumerable<IStorageLoader> _storageLoaders;

        public ConfigurationSource(IEnumerable<IStorageLoader> storageLoaders)
        {
            _storageLoaders = storageLoaders?.ToArray() ?? Enumerable.Empty<IStorageLoader>();
        }

        public bool HasSection(string name)
        {
            return sections.ContainsKey(name);
        }

        public ISettings GetSection(string name)
        {
            return sections[name];
        }

        public IEnumerable<string> AddStorage(IStorage storage)
        {
            _storages.Add(storage);
            List<string> addedSections = new List<string>();

            foreach (var sectionName in storage.Sections)
            {
                sections.AddOrUpdate(
                    key: sectionName, 
                    addValueFactory: name => new StorageSettings(name, storage), 
                    updateValueFactory: (name, current) =>
                {
                    StorageSettings parent = current;
                    // get the last child of the section hierarchy
                    while (parent.Child != null)
                    {
                        parent = parent.Child;
                    }

                    // add new child to the hierachy
                    new StorageSettings(parent, storage);
                    return current;
                });

                addedSections.Add(sectionName);
            }
            return addedSections;
        }


        public IEnumerable<string> AddFile(string path)
        {
            string filepath = path;
            var loader = _storageLoaders.FirstOrDefault(l => l.TryFindConfigurationFile(ref filepath));
            if (loader != null)
            {
                var storage = loader.Load(filepath);
                return AddStorage(storage);
            }
            // TODO exception or failed to load

            return Enumerable.Empty<string>();
        }
    }
}
