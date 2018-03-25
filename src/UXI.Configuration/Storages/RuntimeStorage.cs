using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UXI.Common.Extensions;
using UXI.Configuration.Settings;

namespace UXI.Configuration.Storages
{
    public class RuntimeStorage : IStorage
    {
        private readonly ConcurrentDictionary<string, DictionarySettings> _sections = new ConcurrentDictionary<string, DictionarySettings>();

        public RuntimeStorage(string section)
        {
            _sections.TryAdd(section, new DictionarySettings());
        }

        public RuntimeStorage(IEnumerable<string> sections)
        {
            foreach (var section in sections)
            {
                _sections.TryAdd(section, new DictionarySettings());
            }
        }

        public RuntimeStorage()
        {
           
        }

        public RuntimeStorage(string section, IEnumerable<KeyValuePair<string, object>> settings)
        {
            _sections.TryAdd(section, new DictionarySettings(settings));
        }

        public RuntimeStorage(IEnumerable<KeyValuePair<string, IEnumerable<KeyValuePair<string, object>>>> sections)
        {
            foreach (var section in sections)
            {
                _sections.TryAdd(section.Key, new DictionarySettings(section.Value));            
            }
        }

        public IEnumerable<string> Sections { get { return _sections.Keys; } }


        public IEnumerable<string> GetKeys(string section)
        {
            return _sections.ContainsKey(section) ? _sections[section].Keys : Enumerable.Empty<string>(); 
        }

        public bool TryRead(string section, string key, Type valueType, out object value)
        {
            DictionarySettings sectionSettings;

            if (_sections.TryGetValue(section, out sectionSettings)
                && sectionSettings.TryGetSetting(key, valueType, out value))
            {
                return true;
            }
            value = valueType.GetDefault();
            return false;
        }

        public bool Write(string section, string key, object value)
        {
            var sectionSettings = _sections.GetOrAdd(section, s => new DictionarySettings());

            sectionSettings.SetSetting(key, value);

            return true;
        }

        public void Remove(string section, string key)
        {
            DictionarySettings sectionSettings;

            if (_sections.TryGetValue(section, out sectionSettings))
            {
                sectionSettings.RemoveSetting(key);
            }
        }

        public bool ContainsKey(string section, string key)
        {
            DictionarySettings sectionSettings;

            return _sections.TryGetValue(section, out sectionSettings) && sectionSettings.Contains(key);
        }

        public bool ContainsSection(string section)
        {
            return _sections.ContainsKey(section);
        }

        public bool TryGetSection(string section, out ISettings settings)
        {
            DictionarySettings dictionary;
            if (_sections.TryGetValue(section, out dictionary))
            {
                settings = dictionary;
                return true;
            }

            settings = null;
            return false;
        }


        public Dictionary<string, Dictionary<string, object>> ToDictionary()
        {
            return _sections.ToDictionary(s => s.Key, s => s.Value.ToDictionary());
        }
    }
}
