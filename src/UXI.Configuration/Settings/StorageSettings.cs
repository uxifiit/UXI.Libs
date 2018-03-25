using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;
using UXI.Configuration.Storages;

namespace UXI.Configuration.Settings
{
    public class StorageSettings : ISettings
    {

        private readonly IStorage _storage;

        public StorageSettings Child { get; private set; } = null;

        //internal ConfigurationSection(string name)
        //{
        //    Name = name;
        //}
        //internal ConfigurationSection(string name, IEnumerable<KeyValuePair<string, string>> settings)
        //{
        //    Name = name;
        //    _storage = new RuntimeConfigurationStorage(name, settings);
        //}
        internal StorageSettings(string name, IStorage storage)
        {
            Name = name;
            _storage = storage;
        }

        internal StorageSettings(StorageSettings parent, IStorage storage)
            : this(parent.ThrowIfNull(nameof(parent)).Name, storage)
        {
            parent.ThrowIf(p => p.Child != null, nameof(parent), "Parent section must not have any child yet.");

            parent.Child = this;
        }

        /// <summary>
        /// Gets the name of this configuration section.
        /// </summary>
        public string Name { get; }

        public event ConfigurationSettingChangedEventHandler SettingChanged;

        public event ConfigurationSettingMissingEventHandler SettingMissing;

        public event ConfigurationSettingWriteFailedEventHandler SettingWriteFailed;


        public bool TryGetSetting(string key, Type valueType, out object value)
        {
            key.ThrowIfNull(String.IsNullOrWhiteSpace, nameof(key));

            if (_storage.TryRead(Name, key, valueType, out value) == false)
            {
                if (Child != null)
                {
                    return Child.TryGetSetting(key, valueType, out value);
                }
                else
                {
                    SettingMissing?.Invoke(Name, key);
                    return false;
                }
            }

            return true;
        }

        public bool SetSetting(string key, object value)
        {
            key.ThrowIfNull(String.IsNullOrWhiteSpace, nameof(key));

            bool set = false;

            try
            {
                set = _storage.Write(Name, key, value);
            }
            catch { }

            if (set)
            {
                SettingChanged?.Invoke(Name, key);
            }
            else
            {
                SettingWriteFailed?.Invoke(Name, key);
            }

            return set;
        }
        public void RemoveSetting(string key)
        {
            _storage.Remove(Name, key);
        }

        public bool Contains(string key)
        {
            return _storage.ContainsKey(Name, key) || (Child != null && Child.Contains(key));
        }

        public IEnumerable<string> Keys
        {
            get
            {
                IEnumerable<string> keys = _storage.GetKeys(Name).ToList();

                if (Child != null)
                {
                    keys = keys.Concat(Child.Keys).Distinct();
                }

                return keys;
            }
        }
    }
}
