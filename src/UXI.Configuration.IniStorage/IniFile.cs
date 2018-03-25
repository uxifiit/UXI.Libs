using IniParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UXI.Common.Extensions;

namespace UXI.Configuration.Storages
{
    public class IniFile : IStorage
    {
        private readonly string _path;
        private readonly object _lock = new object();
        private readonly FileIniDataParser _parser;
        private readonly IniParser.Model.IniData _data;


        internal IniFile(string path)
        {
            _path = path;

            _parser = new FileIniDataParser();
            _data = _parser.ReadFile(_path);
        }

        public IEnumerable<string> Sections { get { return _data.Sections.Select(s => s.SectionName); } }

        public IEnumerable<string> GetKeys(string section)
        {
            return _data.Sections.ContainsSection(section) ? _data[section].Select(d => d.KeyName) : Enumerable.Empty<string>();
        }

        public bool Write(string section, string key, object value)
        {
            lock (_lock)
            {
                if (_data.Sections.ContainsSection(section) || _data.Sections.AddSection(section))
                {
                    var sectionSettings = _data[section];
                    string serializedValue = (string)Convert.ChangeType(value, typeof(string), System.Globalization.CultureInfo.InvariantCulture);
                    if (sectionSettings.ContainsKey(key))
                    {
                        sectionSettings[key] = serializedValue;
                    }
                    else
                    {
                        sectionSettings.AddKey(key, serializedValue);
                    }
                    _parser.WriteFile(_path, _data);
                    return true;
                }
            }
            return false;
        }


        public bool TryRead(string section, string key, Type valueType, out object value)
        {
            lock (_lock)
            {
                if (_data.Sections.ContainsSection(section))
                {
                    var iniSection = _data[section];
                    if (iniSection.ContainsKey(key))
                    {
                        string serializedValue = iniSection[key];

                        try
                        {
                            value = Convert.ChangeType(serializedValue, valueType, System.Globalization.CultureInfo.InvariantCulture);
                            return true;
                        }
                        catch { }
                    }
                }
            }
            value = valueType.GetDefault();
            return false;
        }


        public void Remove(string section, string key)
        {
            lock (_lock)
            {
                if (_data.Sections.ContainsSection(section))
                {
                    var iniSection = _data[section];
                    if (iniSection.ContainsKey(key))
                    {
                        iniSection.RemoveKey(key);
                    }
                }
            }
        }


        public bool ContainsKey(string section, string key)
        {
            return _data.Sections.ContainsSection(section) && _data[section].ContainsKey(key);
        }
    }
}
