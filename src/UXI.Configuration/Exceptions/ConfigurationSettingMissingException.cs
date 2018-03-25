using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UXI.Configuration.Exceptions
{

    [Serializable]
    public class ConfigurationSettingMissingException : Exception
    {
        private static string CreateMessage(string section, string key)
        {
            return $"Missing key \"{key}\" in section \"{section}\"";
        }

        public string SectionName { get; }
        public string SettingKey { get; }

        public ConfigurationSettingMissingException(string section, string key) : base(CreateMessage(section, key)) { SectionName = section; SettingKey = key; }
        public ConfigurationSettingMissingException(string section, string key, Exception inner) : base(CreateMessage(section, key), inner) { SectionName = section; SettingKey = key; }
    }
}
