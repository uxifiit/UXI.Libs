using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Configuration.Settings
{
    public class DictionarySettings : ISettings
    {
        private readonly ConcurrentDictionary<string, object> _settings;

        public DictionarySettings(IDictionary<string, object> settings)
        {
            _settings = new ConcurrentDictionary<string, object>(settings);
        }

        public DictionarySettings(IEnumerable<KeyValuePair<string, object>> settings)
        {
            _settings = new ConcurrentDictionary<string, object>(settings);
        }   

        public DictionarySettings()
        {
            _settings = new ConcurrentDictionary<string, object>();
        }

        public void RemoveSetting(string key)
        {
            object value;
            _settings.TryRemove(key, out value);
        }

        public bool SetSetting(string key, object value)
        {
            _settings.AddOrUpdate(key, value, (_, __) => value);
            return true;
        }

        public bool TryGetSetting(string key, Type valueType, out object value)
        {
            return _settings.TryGetValue(key, out value);
        }

        public IEnumerable<string> Keys
        {
            get { return _settings.Keys; }
        }

        public bool Contains(string key)
        {
            return _settings.ContainsKey(key);
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>(_settings);
        }
    }
}
